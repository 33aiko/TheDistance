// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32551,y:32730,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:f52e4223fb21b45ada4dcd2a7147bf0d,ntxv:0,isnm:False|UVIN-2567-OUT;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5983-RGB,C-5376-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32513,y:32915,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32479,y:33088,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1749,x:33025,y:32818,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A,C-5376-A;n:type:ShaderForge.SFN_Tex2d,id:7054,x:31911,y:32912,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7054,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-3134-UVOUT;n:type:ShaderForge.SFN_Slider,id:7014,x:31956,y:33136,ptovrint:False,ptlb:Distortion Percent,ptin:_DistortionPercent,varname:node_7014,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.02,max:0.1;n:type:ShaderForge.SFN_Lerp,id:2567,x:32306,y:32925,varname:node_2567,prsc:2|A-4214-UVOUT,B-842-OUT,T-7014-OUT;n:type:ShaderForge.SFN_TexCoord,id:4214,x:32031,y:32684,varname:node_4214,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:842,x:32113,y:32905,varname:node_842,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7054-RGB;n:type:ShaderForge.SFN_TexCoord,id:6875,x:30926,y:32467,varname:node_6875,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:5488,x:31458,y:32549,varname:node_5488,prsc:2,spu:1,spv:0|UVIN-6875-UVOUT,DIST-1998-OUT;n:type:ShaderForge.SFN_Time,id:2582,x:30512,y:32910,varname:node_2582,prsc:2;n:type:ShaderForge.SFN_Slider,id:6477,x:30355,y:33059,ptovrint:False,ptlb:Wave Speed,ptin:_WaveSpeed,varname:node_6477,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:2;n:type:ShaderForge.SFN_Multiply,id:555,x:30782,y:32811,varname:node_555,prsc:2|A-2582-TSL,B-6477-OUT;n:type:ShaderForge.SFN_Slider,id:3146,x:30995,y:32902,ptovrint:False,ptlb:X_Offset,ptin:_X_Offset,varname:node_3146,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:1998,x:31215,y:32656,varname:node_1998,prsc:2|A-555-OUT,B-3146-OUT;n:type:ShaderForge.SFN_Slider,id:9382,x:31351,y:33008,ptovrint:False,ptlb:Y_Offset,ptin:_Y_Offset,varname:node_9382,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-10,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:2476,x:31508,y:32785,varname:node_2476,prsc:2|A-555-OUT,B-9382-OUT;n:type:ShaderForge.SFN_Panner,id:3134,x:31684,y:32549,varname:node_3134,prsc:2,spu:0,spv:1|UVIN-5488-UVOUT,DIST-2476-OUT;proporder:4805-5983-7054-7014-6477-3146-9382;pass:END;sub:END;*/

Shader "Shader Forge/Distortion" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Noise ("Noise", 2D) = "white" {}
        _DistortionPercent ("Distortion Percent", Range(0, 0.1)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0, 2)) = 0.8
        _X_Offset ("X_Offset", Range(-10, 10)) = 1
        _Y_Offset ("Y_Offset", Range(-10, 10)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Stencil ("Stencil ID", Float) = 0
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilOpFail ("Stencil Fail Operation", Float) = 0
        _StencilOpZFail ("Stencil Z-Fail Operation", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            Stencil {
                Ref [_Stencil]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilOp]
                Fail [_StencilOpFail]
                ZFail [_StencilOpZFail]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 2.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DistortionPercent;
            uniform float _WaveSpeed;
            uniform float _X_Offset;
            uniform float _Y_Offset;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_2582 = _Time;
                float node_555 = (node_2582.r*_WaveSpeed);
                float2 node_3134 = ((i.uv0+(node_555*_X_Offset)*float2(1,0))+(node_555*_Y_Offset)*float2(0,1));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_3134, _Noise));
                float2 node_2567 = lerp(i.uv0,_Noise_var.rgb.rg,_DistortionPercent);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_2567, _MainTex));
                float node_603 = (_MainTex_var.a*_Color.a*i.vertexColor.a); // A
                float3 emissive = ((_MainTex_var.rgb*_Color.rgb*i.vertexColor.rgb)*node_603);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_603);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 2.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
