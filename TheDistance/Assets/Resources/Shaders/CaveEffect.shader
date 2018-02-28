Shader "CaveEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PlayerPos("Player Pos", Vector)=(0,0,0)
		_SpiritPos("Spirit Pos", Vector)=(0,0,-1)
		//_FragmentPos("Fragment pos", Vector)=(0,0,-1)
		//_ChekpointPos("Checkpoint pos", Vector)=(0,0,-1)
		_FragmentLightRadius("Fragment Light Radius", float) = 500
		_CheckpointLightRadius("Checkpoint Light Radius", float) = 500
		_SpiritLightRadius("Spirit Light Radius", float) = 800
		_LightRadius("Light Radius", float) = 300
	}
	SubShader
	{
		Tags {"Queue"="Overlay" "RenderType"="Transparent" }
		LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		CGINCLUDE	
			float alphaMaskHelper(float3 worldPos, float3 sourcePos, float r)
			{
				float d = distance(worldPos, sourcePos);
				float f = 0;
				if(d < r)
				{
					f = (r - d)/r;
					f = f * f;
				}
				return f;
			}
		ENDCG

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
				float3 worldPos: TEXCOORD7;
			};

			sampler2D _MainTex;

			float3 _PlayerPos;
			float3 _SpiritPos;
			float4 _FragmentPos[3];
			float4 _CheckpointPos[3];

			float _LightRadius;
			float _SpiritLightRadius;
			float _FragmentLightRadius;
			float _CheckpointLightRadius;

			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos  = mul(unity_ObjectToWorld, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				col.a = col.a * (1-alphaMaskHelper(i.worldPos, _PlayerPos, _LightRadius));

				if(_SpiritPos.z > -0.5)
				{
					col.a = col.a * (1-alphaMaskHelper(i.worldPos, _SpiritPos, _SpiritLightRadius));
				}

				for(int idx = 0; idx < 3; idx++)
				{
					float4 cpPos = _CheckpointPos[idx];
					if(cpPos.w > -0.5)
					{
						col.a = col.a * (1-alphaMaskHelper(i.worldPos, cpPos, _CheckpointLightRadius));
					}

					float4 fgPos = _FragmentPos[idx];
					if(fgPos.w > -0.5)
					{
						col.a = col.a * (1-alphaMaskHelper(i.worldPos, fgPos, _CheckpointLightRadius));
					}
				}

				return col;
			}
			ENDCG
		}
	}
}
