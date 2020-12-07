Shader "Hidden/Error"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Indensity ("Indensity", Float) = 0.05
        _Amplitude ("Amplitude", Float) = 1
        _Amount ("Amount", Float) = 1
        _BlockSize ("BlockSize", Float) = 1
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

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            sampler2D _MainTex;
            float _Indensity;
            float _Amplitude;
            float _Amount;
            float _BlockSize;

            float randomNoise(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }   

            fixed4 frag_rgbsplit (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //v1
                float splitAmountV1 = _Indensity * randomNoise(_Time.x, 2);

                //v2
                //基于三角函数和pow方法控制抖动的间隔、幅度，以及抖动的曲线
                // float splitAmoutV2 = (1.0 + sin(_Time.x * 6.0)) * 0.5;
                // splitAmoutV2 *= 1.0 + sin(_Time.x * 16.0) * 0.5;
                // splitAmoutV2 *= 1.0 + sin(_Time.x * 19.0) * 0.5;
                // splitAmoutV2 *= 1.0 + sin(_Time.x * 27.0) * 0.5;
                // splitAmoutV2 = pow(splitAmoutV2, _Amplitude);
                // splitAmoutV2 *= (0.05 * _Amount);

                // just invert the colors
                // col.rgb = 1 - col.rgb;
                col.r = tex2D( _MainTex, float2(i.uv.x + splitAmountV1, i.uv.y));
                col.g = tex2D( _MainTex, i.uv);
                col.b = tex2D( _MainTex, float2(i.uv.x - splitAmountV1, i.uv.y));
                // return half4(ColorR.r, ColorG.g, ColorB.b, 1);
                return col;
            }

            fixed4 frag_block (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //v1
                float splitAmountV1 = _Indensity * randomNoise(_Time.x, 2);
                half2 block = randomNoise(floor(i.uv * _BlockSize).x, (floor(i.uv * _BlockSize).y));
                half displaceNoise = pow(block.x, 8.0) * pow(block.x, 3.0);

                col.r = tex2D( _MainTex, float2(i.uv.x + displaceNoise, i.uv.y));
                col.g = tex2D( _MainTex, i.uv);
                col.b = tex2D( _MainTex, float2(i.uv.x - displaceNoise, i.uv.y));


                // col = tex2D(_MainTex,block);
                return col;
            }

    ENDCG
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "RGB Split Glitch"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_rgbsplit

            ENDCG
        }
        Pass
        {
            Name "Image Block Glitch"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag_block

            ENDCG
        }
    }
}
