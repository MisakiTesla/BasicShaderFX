Shader "Custom/BgBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", float) = 1
    }
	
	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float4 blurTexcoord: TEXCOORD1;
	};

	float4 _BlurOffset;
	uniform float2 _MainTex_TexelSize;
	uniform float _Radius;

	v2f vert5Horizontal(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		float2 offset = float2(_MainTex_TexelSize.x * _Radius * 1.33333333, 0.0);
		o.blurTexcoord = o.uv.xyxy + _BlurOffset.xyxy * float4(1, 1, -1, -1);
		o.blurTexcoord.xy = v.uv + offset;
		o.blurTexcoord.zw = v.uv - offset;
		return o;
	}

	v2f vert5Vertical(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		float2 offset = float2(0.0, _MainTex_TexelSize.y * _Radius * 1.33333333);
		o.blurTexcoord = o.uv.xyxy + _BlurOffset.xyxy * float4(1, 1, -1, -1);
		o.blurTexcoord.xy = v.uv + offset;
		o.blurTexcoord.zw = v.uv - offset;
		return o;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f i) : SV_Target
	{
		//fixed4 col = tex2D(_MainTex, i.uv);

		fixed3 col = tex2D(_MainTex, i.uv).xyz * 0.29411764;
		col += tex2D(_MainTex, i.blurTexcoord.xy).xyz * 0.35294117;
		col += tex2D(_MainTex, i.blurTexcoord.zw).xyz * 0.35294117;
		return fixed4(col, 1.0);
	}

	ENDCG
	
    SubShader
    {
    	
	    Cull Off ZWrite Off ZTest Always

	    Pass
		{
			Name "Horizontal" // 0
			CGPROGRAM
			#pragma vertex vert5Horizontal
			#pragma fragment frag
			ENDCG
		}

		Pass
		{
			Name "Vertical" // 1
			CGPROGRAM
			#pragma vertex vert5Vertical
			#pragma fragment frag
			ENDCG
		}
    }
}
