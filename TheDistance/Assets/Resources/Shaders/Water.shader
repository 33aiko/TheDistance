// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BCW/Water"
{
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Normalmap", 2D) = "bump" {}
		_BaseTex ("Base Texture", 2d) ="rua" {}
		//_BackgroundTex("Back Ground Texture", 2d) ="rua" {}
		_Magnitude("Magnitude", Range(0,1)) = 0.05
	}
	SubShader {
		// Need to disable batching because of the vertex animation
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "DisableBatching"="True"}

		GrabPass{"_BackgroundTex"}
		
		Pass {
			
			Name "BASE"
			
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM  
			#pragma vertex vert 
			#pragma fragment frag
			
			#include "UnityCG.cginc" 
			
			
			struct vertexInput {
				float4 vertex : POSITION;
				float2 texcoord: TEXCOORD0;
			};

			struct vertexOutput {
				float4 vertex : SV_POSITION;
				float4 uvgrab : TEXCOORD0;
				float2 uvmain : TEXCOORD2;
				UNITY_FOG_COORDS(3)
			};

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _BaseTex;
			float4 _MainTex_ST;
			float _BumpAmt;
			float4 _BumpMap_ST;
			float  _Magnitude;
			float _height[256];

			float getHeight(float idx)
			{
				float f = idx * 255.0;
				int f0 = floor(f);
				int f1 = f0 + 1;
				return _height[f0] * (f1 - f) + _height[f1] * ( 1 - f1 + f);
			}

			vertexOutput vert (vertexInput v)
			{
				vertexOutput o;
				float4 offset;
				offset.xyzw = float4(0.0, 0.0, 0.0, 0.0);
				offset.z = getHeight(1 - v.texcoord[0]);
				offset.z = offset.z + sin(_Time.w * 2 + 50 * v.texcoord[0]) * 0.02;
				if(v.texcoord[1] < 0.2) 
				{
					offset.z = 0.0;
				}
				o.vertex = UnityObjectToClipPos(v.vertex + offset);
				
				 #if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
				UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
			}


			sampler2D _BackgroundTex;
			float4 _GrabTexture_TexelSize;
			sampler2D _BumpMap;

			half4 frag (vertexOutput i) : SV_Target
			{
				// Calculate perturbed coordinates
				half4 bump = tex2D(_MainTex, i.uvmain);

				half2 distortion = UnpackNormal(bump).rg;
				i.uvgrab.xy += distortion * _Magnitude;

				// Get pixel in GrabTexture, rendered in previous pass
				half4 col = tex2Dproj(_BackgroundTex, UNITY_PROJ_COORD(i.uvgrab));

				half4 c = tex2D(_BaseTex, i.uvmain);
				// Apply color
				col *= _Color;
				col *= c;
				col.a = 0.85;

				return col;
			}

			/*
			v2f vert(a2v v) {
				v2f o;
				
				//o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv = v.texcoord;


				o.pos = UnityObjectToClipPos(v.vertex + offset);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target {
				fixed4 c = tex2D(_MainTex, i.uv);
				c.rgb *= _Color.rgb;
				//float tmp = i.uv[1];
				c.a = 0.85;
				return c;
			} 
			*/
			
			ENDCG
		}
	}
	FallBack "Transparent/VertexLit"
}
