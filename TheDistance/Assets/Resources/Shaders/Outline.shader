Shader "TheDistance/Outline"
{
	Properties
	{
		_RimColor ("RimColor", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_BloomIntensity ("BloomIntensity", Range(0,10)) = 2
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _BloomIntensity;
			fixed4 _RimColor;

			v2f vert (appdata v)
			{
				fixed4 rim_color = _RimColor;
				fixed hide_alpha = 0.5;
				fixed accuracy = 0.04;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 rim_color = _RimColor;
				fixed hide_alpha = 0.5;
				fixed accuracy = 0.04;
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				fixed2 top_left = i.uv+fixed2(-accuracy,accuracy);
				fixed2 top_right = i.uv+fixed2(accuracy,accuracy);
				fixed2 bottom_left = i.uv+fixed2(-accuracy,-accuracy);
				fixed2 bottom_right = i.uv+fixed2(accuracy,-accuracy);
				fixed4 col1 = tex2D(_MainTex, top_left);
				fixed4 col2 = tex2D(_MainTex, top_right);
				fixed4 col3 = tex2D(_MainTex, bottom_left);
				fixed4 col4 = tex2D(_MainTex, bottom_right);

				if(col1.a < hide_alpha || col2.a<hide_alpha || col3.a<hide_alpha || col4.a<hide_alpha){
					if(col.a>hide_alpha){
						col = rim_color*_BloomIntensity;
					}
					else{
						discard;
					}
				}

				return col;
			}
			ENDCG
		}
	}
}
