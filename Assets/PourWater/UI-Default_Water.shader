// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/Default_Water"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
        

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        
                //高光
        [Toggle(GUIDE_HIGHLIGHT)] _HighLight ("高光", Float) = 0

        		//液体部分
		_Height3("Height3",float)=0.8
		_Height2("Height2",float)=0.6
		_Height1("Height1",float)=0.4
		_Height0("Height0",float)=0.2
		_WorldPosBottom("WorldPosBottom",vector)=(0,0,0,0)
		_WorldPosTop("WorldPosTop",vector)=(0,0,0,0)
		_WorldWaveStartPos("WorldWaveStartPos",vector)=(0,0,0,0)
		_WaveOffset("WaveOffset",float)=0
		_WaveWidth("WaveWidth",float)=10
        _Color3 ("Color3", Color) = (1,0,1,1)
        _Color2 ("Color2", Color) = (0,0,1,1)
        _Color1 ("Color1", Color) = (0,1,0,1)
        _Color0 ("Color0", Color) = (1,0,0,1)
//        _MaskTex ("Mask Texture", 2D) = "white" {}


    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float4 _WorldPosBottom;
            float4 _WorldPosTop;
            float4 _WorldWaveStartPos;
            float _WaveOffset;
            float _WaveWidth;

            float _Height0;
            float _Height1;
            float _Height2;
            float _Height3;

            fixed4 _Color0;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _Color3;
            sampler2D _MaskTex;


            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            float GetWaterWorldYPosByNormalizedHeight(float normalizedheight, float topToBottomDist)
            {
                return (_WorldPosBottom.y + normalizedheight * topToBottomDist);
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                //half4 mask = (tex2D(_MaskTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                float3 worldPos = mul(unity_ObjectToWorld,IN.worldPosition);

                float topToBottomDist = _WorldPosTop.y - _WorldPosBottom.y;

                float waterPos0 = GetWaterWorldYPosByNormalizedHeight(_Height0,topToBottomDist);
                float waterPos1 = GetWaterWorldYPosByNormalizedHeight(_Height1,topToBottomDist);
                float waterPos2 = GetWaterWorldYPosByNormalizedHeight(_Height2,topToBottomDist);
                float waterPos3 = GetWaterWorldYPosByNormalizedHeight(_Height3,topToBottomDist);

                float side =  normalize(_WorldPosTop.x - worldPos.x);
                float distToWaveStartPos = abs(_WorldPosTop.x - worldPos.x);

                // float waveOffset = sin(worldPos.x + _Time.z) + abs(_WorldWaveStartPos.x - worldPos.x);
                // float waveOffset = sin(worldPos.x + _Time.z) + (1+smoothstep(_WaveOffset,_WaveOffset+1, distToWaveStartPos)*5 - smoothstep(_WaveOffset+ _WaveWidth,_WaveOffset+_WaveWidth+1,distToWaveStartPos)*5);
                // float waveOffset = cos(_WorldPosTop.x - worldPos.x + _WaveOffset +_Time.z)* (1+clamp(10/ abs(_WorldPosTop.x - worldPos.x + _WaveOffset),0,5));
                float waveOffset = 0;
                
                //step(0,1) = 1
                float weight0 = step(worldPos.y, waterPos0);
                float weight1 = step(waterPos0, worldPos.y) * step(worldPos.y, waterPos1);
                float weight2 = step(waterPos1, worldPos.y) * step(worldPos.y, waterPos2);
                float weight3 = step(waterPos2, worldPos.y);
                
                color = weight0 * _Color0 + weight1 * _Color1 + weight2 * _Color2 + weight3 * _Color3;
                
                color.a *= step( worldPos.y  , waterPos3+ waveOffset);
                
                // color.a = IN.worldPosition.x;
                // color.g = IN.vertex.y/1280 >;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

               
                return color;
            }
        ENDCG
        }
    }
}
