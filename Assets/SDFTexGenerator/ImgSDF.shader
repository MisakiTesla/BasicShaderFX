// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Wunder/ImgSDF"
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

        _SDF ("SDF", Float) = 1
        _DistanceMark("Distance Mark", float) = .5
        _OutlineDistanceMark("Outline Distance Mark", float) = .25
        _GlowDistanceMark("Glow Distance Mark", float) = .25
        _SmoothDelta("Smooth Delta", float) = .25
        _ShadowSmoothDelta("Shadow Smooth", float) = .1
        _GlowSmoothDelta("Glow Smooth", float) = .1
        _MainColor("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (1,0,0,1)
        _GlowColor("Glow Color", Color) = (1,0,0,1)
        _ShadowColor("Shadow Color", Color) = (1,0,0,1)
        _ShadowOffsetX("Shadow Offset X", float) = 0
        _ShadowOffsetY("Shadow Offset Y", float) = 0


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
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _SDF;
            //float4 _MainColor;
            float4 _OutlineColor;
            float4 _ShadowColor;
            float4 _GlowColor;
            float _SmoothDelta;
            float _ShadowSmoothDelta;
            float _GlowSmoothDelta;
            float _DistanceMark;
            float _OutlineDistanceMark;
            float _GlowDistanceMark;
            float _ShadowOffsetX;
            float _ShadowOffsetY;

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

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                float distance = color.a;

                //fixed4 col = tex2D(_MainTex, IN.texcoord);

                fixed4 outlineCol;
                outlineCol.a = smoothstep(_OutlineDistanceMark - _SmoothDelta, _OutlineDistanceMark + _SmoothDelta,
                                          distance);
                outlineCol.rgb = _OutlineColor.rgb;

                fixed4 glowCol;
                glowCol.a = smoothstep(_GlowDistanceMark - _GlowSmoothDelta, _GlowDistanceMark + _GlowSmoothDelta,
                                       distance);
                glowCol.rgb = _GlowColor.rgb;

                float shadowDistance = tex2D(_MainTex, IN.texcoord + half2(_ShadowOffsetX, _ShadowOffsetY));
                float shadowAlpha = smoothstep(_DistanceMark - _ShadowSmoothDelta, _DistanceMark + _ShadowSmoothDelta,
                                               shadowDistance);
                fixed4 shadowCol = fixed4(_ShadowColor.rgb, _ShadowColor.a * shadowAlpha);

                color.rgb = _Color.rgb;
                color.a = smoothstep(_DistanceMark - _SmoothDelta, _DistanceMark + _SmoothDelta, distance);
                //if (distance < _DistanceMark)
                //	col.a = 0.0;
                //else
                //	col.a = 1.0;
                //col.rgb = _MainColor.rgb;

                //return col;
                return lerp(glowCol, color, color.a);
                //return lerp(outlineCol, col, col.a);
                //return lerp(shadowCol, color, color.a);

                //color.a = smoothstep(_SDF, _SDF + 0.25, color.a);
                return color;
            }
            ENDCG
        }
    }
}