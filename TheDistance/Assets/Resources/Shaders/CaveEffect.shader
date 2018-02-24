Shader "CaveEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RUA("RUA", Vector)=(0,0,0)
		_SpiritPos("Spirit Pos", Vector)=(0,0,-1)
		_SpiritLightRadius("Spirit Light Radius", float) = 800
		_LightRadius("Light Radius", float) = 300
	}
	SubShader
	{
		Tags {"Queue"="Overlay" "RenderType"="Transparent" }
		LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
				float3 rua : TEXCOORD7;
			};

			sampler2D _MainTex;
			float3 _RUA;
			float3 _SpiritPos;
			float _LightRadius;
			float _SpiritLightRadius;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.rua = worldPos;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float f = 0;
				fixed4 col = tex2D(_MainTex, i.uv);

				float radius = _LightRadius;
				float d = distance(i.rua, _RUA);
				if(d < radius)
				{
					f = (radius- d)/radius;
					f = f * f;
				}
				col.a = col.a * (1-f);

				if(_SpiritPos.z > -0.5)
				{
				f = 0;
				radius = _SpiritLightRadius;
				d = distance(i.rua, _SpiritPos);
				if(d < radius)
				{
					f = (radius- d)/radius;
					f = f * f;
				}
				col.a = col.a * (1-f);
				}

				return col;
			}
			ENDCG
		}
	}
}
