// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "stamp"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_SourceTex("SourceTex", 2D) = "white" {}
		_SourceUV("_SourceUV", Vector) = (0,0,0,0)
		_HeightOffset("HeightOffset", Float) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _HeightOffset;
			uniform sampler2D _SourceTex;
			uniform float4 _SourceUV;
			SamplerState sampler_MainTex;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_MainTex = i.ase_texcoord1.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode2 = tex2D( _MainTex, uv_MainTex );
				float4 temp_cast_0 = (_HeightOffset).xxxx;
				float4 temp_output_13_0 = ( tex2DNode2 - temp_cast_0 );
				float4 temp_cast_1 = (-1.0).xxxx;
				float4 temp_cast_2 = (0.0).xxxx;
				float4 clampResult16 = clamp( temp_output_13_0 , temp_cast_1 , temp_cast_2 );
				float4 temp_cast_3 = (_HeightOffset).xxxx;
				float4 temp_cast_4 = (0.1).xxxx;
				float4 clampResult15 = clamp( temp_output_13_0 , float4( 0,0,0,0 ) , temp_cast_4 );
				float2 appendResult8 = (float2(_SourceUV.x , _SourceUV.y));
				float2 appendResult7 = (float2(_SourceUV.z , _SourceUV.w));
				float4 tex2DNode3 = tex2D( _SourceTex, ( appendResult8 + ( appendResult7 * i.ase_texcoord1.xy ) ) );
				float4 break28 = ( clampResult16 + ( clampResult15 * tex2DNode3 ) + tex2DNode3 );
				float4 appendResult27 = (float4(break28.r , break28.g , break28.b , tex2DNode2.a));
				
				
				finalColor = appendResult27;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18703
418;289;1471;1273;470.5419;545.0472;1.3;True;True
Node;AmplifyShaderEditor.CommentaryNode;10;-946.369,181.8957;Inherit;False;662.9;395.1;offset & tilling;6;6;9;8;7;5;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;5;-942.5747,233.0637;Inherit;True;Property;_SourceUV;_SourceUV;2;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-393.3934,-153.3559;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;7;-710.3976,322.1037;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;4;-766.5071,435.4595;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;14;-123.9793,-347.7825;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-558.6566,319.9507;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;8;-704.8977,230.3036;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-59.49187,-350.178;Inherit;False;Property;_HeightOffset;HeightOffset;3;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-408.7973,230.9153;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;13;124.7949,-434.5502;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;19;348.9599,-218.6069;Inherit;False;225;206;凸起;1;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;20;383.0647,-541.8923;Inherit;False;351.0594;293.7266;凹陷;3;17;18;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;35;145.6581,-106.9472;Inherit;False;Constant;_MaxHeight;MaxHeight;4;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;404.8084,-359.8285;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;399.6962,-439.6148;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-224.9222,173.7284;Inherit;True;Property;_SourceTex;SourceTex;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;15;398.9601,-168.6068;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;16;593.605,-505.4642;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;615.5455,25.86895;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;808.0466,140.832;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;28;964.4846,208.3643;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;27;1193.971,294.2227;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1783.226,152.3408;Float;False;True;-1;2;ASEMaterialInspector;100;1;stamp;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;7;0;5;3
WireConnection;7;1;5;4
WireConnection;14;0;2;0
WireConnection;6;0;7;0
WireConnection;6;1;4;0
WireConnection;8;0;5;1
WireConnection;8;1;5;2
WireConnection;9;0;8;0
WireConnection;9;1;6;0
WireConnection;13;0;14;0
WireConnection;13;1;12;0
WireConnection;3;1;9;0
WireConnection;15;0;13;0
WireConnection;15;2;35;0
WireConnection;16;0;13;0
WireConnection;16;1;17;0
WireConnection;16;2;18;0
WireConnection;24;0;15;0
WireConnection;24;1;3;0
WireConnection;25;0;16;0
WireConnection;25;1;24;0
WireConnection;25;2;3;0
WireConnection;28;0;25;0
WireConnection;27;0;28;0
WireConnection;27;1;28;1
WireConnection;27;2;28;2
WireConnection;27;3;2;4
WireConnection;1;0;27;0
ASEEND*/
//CHKSM=D497C13F43EA710E23AC6133801A846EE237692A