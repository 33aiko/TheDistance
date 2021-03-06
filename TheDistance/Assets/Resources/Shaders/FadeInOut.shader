// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:34162,y:33333,varname:node_1873,prsc:2|emission-7219-OUT,alpha-603-OUT,clip-6642-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32551,y:32729,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,tex:d3852954314a347c1bccc6c4077b6b24,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32886,y:32825,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5983-RGB,C-5376-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32915,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:33079,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1749,x:33230,y:32855,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32833,y:33012,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A,C-5376-A;n:type:ShaderForge.SFN_Tex2dAsset,id:9716,x:33285,y:33629,ptovrint:False,ptlb:Ramp,ptin:_Ramp,varname:node_9716,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:181ec605fb3b4314bbee61db18ab41c0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:1202,x:33445,y:33548,varname:node_1202,prsc:2,tex:181ec605fb3b4314bbee61db18ab41c0,ntxv:0,isnm:False|UVIN-1324-OUT,TEX-9716-TEX;n:type:ShaderForge.SFN_Vector1,id:956,x:33130,y:33543,varname:node_956,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:1324,x:33306,y:33428,varname:node_1324,prsc:2|A-5263-OUT,B-956-OUT;n:type:ShaderForge.SFN_Tex2d,id:7049,x:32578,y:34001,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_7049,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:189,x:31994,y:33862,ptovrint:False,ptlb:Dissolve Amount,ptin:_DissolveAmount,varname:node_189,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:6642,x:32811,y:33960,varname:node_6642,prsc:2|A-4819-OUT,B-7049-R;n:type:ShaderForge.SFN_RemapRange,id:4819,x:32563,y:33737,varname:node_4819,prsc:2,frmn:0,frmx:1,tomn:-0.6,tomx:0.6|IN-5586-OUT;n:type:ShaderForge.SFN_OneMinus,id:5586,x:32352,y:33843,varname:node_5586,prsc:2|IN-189-OUT;n:type:ShaderForge.SFN_RemapRange,id:7774,x:32928,y:33742,varname:node_7774,prsc:2,frmn:0,frmx:1,tomn:-3.5,tomx:3.5|IN-6642-OUT;n:type:ShaderForge.SFN_Clamp01,id:9137,x:33123,y:33742,varname:node_9137,prsc:2|IN-7774-OUT;n:type:ShaderForge.SFN_OneMinus,id:5263,x:32953,y:33495,varname:node_5263,prsc:2|IN-9137-OUT;n:type:ShaderForge.SFN_Add,id:5862,x:33708,y:33331,varname:node_5862,prsc:2|A-1749-OUT,B-1202-RGB;n:type:ShaderForge.SFN_Clamp01,id:7219,x:33933,y:33239,varname:node_7219,prsc:2|IN-5862-OUT;n:type:ShaderForge.SFN_TexCoord,id:5204,x:32169,y:34260,varname:node_5204,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_RemapRange,id:636,x:32444,y:34281,varname:node_636,prsc:2,frmn:0,frmx:1,tomn:-0.5,tomx:0.5|IN-5204-UVOUT;n:type:ShaderForge.SFN_Length,id:1751,x:32639,y:34281,varname:node_1751,prsc:2|IN-636-OUT;n:type:ShaderForge.SFN_Add,id:5516,x:32982,y:34291,varname:node_5516,prsc:2|A-1988-OUT,B-766-OUT;n:type:ShaderForge.SFN_Vector1,id:766,x:32711,y:34516,varname:node_766,prsc:2,v1:0.1;n:type:ShaderForge.SFN_OneMinus,id:1988,x:32815,y:34281,varname:node_1988,prsc:2|IN-1751-OUT;n:type:ShaderForge.SFN_Multiply,id:5893,x:33371,y:34149,varname:node_5893,prsc:2|A-5586-OUT,B-5516-OUT,C-1982-OUT;n:type:ShaderForge.SFN_Vector1,id:1982,x:33082,y:34468,varname:node_1982,prsc:2,v1:3;n:type:ShaderForge.SFN_Multiply,id:3733,x:33823,y:33836,varname:node_3733,prsc:2|B-3970-OUT;n:type:ShaderForge.SFN_Clamp01,id:3970,x:33604,y:34035,varname:node_3970,prsc:2|IN-5893-OUT;proporder:4805-5983-9716-7049-189;pass:END;sub:END;*/

Shader "Shader Forge/FadeInOut" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Ramp ("Ramp", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
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
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _Ramp; uniform float4 _Ramp_ST;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DissolveAmount;
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
                float node_5586 = (1.0 - _DissolveAmount);
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_6642 = ((node_5586*1.2+-0.6)+_Noise_var.r);
                clip(node_6642 - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_603 = (_MainTex_var.a*_Color.a*i.vertexColor.a); // A
                float2 node_1324 = float2((1.0 - saturate((node_6642*7.0+-3.5))),0.0);
                float4 node_1202 = tex2D(_Ramp,TRANSFORM_TEX(node_1324, _Ramp));
                float3 emissive = saturate((((_MainTex_var.rgb*_Color.rgb*i.vertexColor.rgb)*node_603)+node_1202.rgb));
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
            #pragma target 3.0
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _DissolveAmount;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
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
                float node_5586 = (1.0 - _DissolveAmount);
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(i.uv0, _Noise));
                float node_6642 = ((node_5586*1.2+-0.6)+_Noise_var.r);
                clip(node_6642 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
