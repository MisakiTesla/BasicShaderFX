// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Tears"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_Texture1("Texture 1", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
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
			#include "UnityShaderVariables.cginc"


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

			uniform float4 _Color;
			uniform sampler2D _Texture0;
			uniform sampler2D _Texture1;
			uniform float4 _Texture1_ST;

			
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
				float2 appendResult6 = (float2(i.ase_texcoord1.xy.x , ( ( sin( ( ( i.ase_texcoord1.xy.x * 16.7 ) + ( _Time.y * 4.9 ) ) ) * -0.1 ) + i.ase_texcoord1.xy.y )));
				float2 uv_Texture1 = i.ase_texcoord1.xy * _Texture1_ST.xy + _Texture1_ST.zw;
				
				
				finalColor = ( _Color * ( tex2D( _Texture0, appendResult6 ) * tex2D( _Texture1, uv_Texture1 ).r ) );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18703
193;84;1567;934;993.2233;654.263;1.630927;True;True
Node;AmplifyShaderEditor.TexCoordVertexDataNode;4;-1052.5,223.5;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;18;-776.1761,442.6851;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-781.454,517.0324;Inherit;False;Constant;_Speed;Speed;1;0;Create;True;0;0;False;0;False;4.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-901.8765,374.499;Inherit;False;Constant;_Freq;Freq;1;0;Create;True;0;0;False;0;False;16.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-729.8749,200.4994;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-608.1633,480.9345;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-491.6284,401.8937;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-418.5628,504.8059;Inherit;False;Constant;_Strength;Strength;0;0;Create;True;0;0;False;0;False;-0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;13;-393.2146,180.7919;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;10;-853.9648,598.1809;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-211.646,338.8294;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-86.96484,529.1809;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;19;-593.798,733.8345;Inherit;True;Property;_Texture1;Texture 1;1;0;Create;True;0;0;False;0;False;5f36f7cd71ceee047a3647668a74de5c;5f36f7cd71ceee047a3647668a74de5c;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode;6;-74.5,247.5;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-451.5,-146.5;Inherit;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;False;0;False;283ca1c93520c1448808188071585df9;283ca1c93520c1448808188071585df9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;20;-148.007,759.8535;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-27.5,-117.5;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;364.2902,264.4913;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;341.325,-10.80037;Inherit;False;Property;_Color;Color;2;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;670.3222,135.1057;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-409.8458,91.3786;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;914.6573,128.4968;Float;False;True;-1;2;ASEMaterialInspector;100;1;Tears;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;True;0;False;-1;True;0;False;-1;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;11;0;4;1
WireConnection;11;1;12;0
WireConnection;22;0;18;0
WireConnection;22;1;17;0
WireConnection;16;0;11;0
WireConnection;16;1;22;0
WireConnection;13;0;16;0
WireConnection;10;0;4;2
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;9;0;14;0
WireConnection;9;1;10;0
WireConnection;6;0;4;1
WireConnection;6;1;9;0
WireConnection;20;0;19;0
WireConnection;2;0;1;0
WireConnection;2;1;6;0
WireConnection;21;0;2;0
WireConnection;21;1;20;1
WireConnection;24;0;23;0
WireConnection;24;1;21;0
WireConnection;0;0;24;0
ASEEND*/
//CHKSM=9601B27948F2C2744F9F0C55468E05632D4015C5