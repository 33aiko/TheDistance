Shader "TheDistance/OutlineSurf" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;

			fixed4 rim_color = _Color;
				fixed hide_alpha = 0.5;
				fixed accuracy = 0.02;

				fixed2 top_left = IN.uv_MainTex+fixed2(-accuracy,accuracy);
				fixed2 top_right = IN.uv_MainTex+fixed2(accuracy,accuracy);
				fixed2 bottom_left = IN.uv_MainTex+fixed2(-accuracy,-accuracy);
				fixed2 bottom_right = IN.uv_MainTex+fixed2(accuracy,-accuracy);
				fixed4 col1 = tex2D(_MainTex, top_left);
				fixed4 col2 = tex2D(_MainTex, top_right);
				fixed4 col3 = tex2D(_MainTex, bottom_left);
				fixed4 col4 = tex2D(_MainTex, bottom_right);

				if(col1.a < hide_alpha || col2.a<hide_alpha || col3.a<hide_alpha || col4.a<hide_alpha){
					if(c.a>hide_alpha){
						c = rim_color;
						o.Emission = 1;
					}
					else{
						discard;
					}
				}

			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
