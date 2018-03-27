// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32602,y:32524,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-6258-RGB,B-5983-RGB,C-5376-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32915,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:33079,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1749,x:33025,y:32818,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-6258-A,B-5983-A,C-5376-A;n:type:ShaderForge.SFN_ScreenPos,id:5463,x:31392,y:32636,varname:node_5463,prsc:2,sctp:0;n:type:ShaderForge.SFN_SceneColor,id:6258,x:32551,y:32752,varname:node_6258,prsc:2|UVIN-918-OUT;n:type:ShaderForge.SFN_RemapRange,id:6985,x:31556,y:32636,varname:node_6985,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-5463-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:16,x:31920,y:32927,ptovrint:False,ptlb:node_16,ptin:_node_16,varname:node_16,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b50e24d0f609a644ea95ef86500f7c26,ntxv:2,isnm:False|UVIN-4085-UVOUT;n:type:ShaderForge.SFN_Add,id:918,x:32302,y:32639,varname:node_918,prsc:2|A-708-OUT,B-8442-OUT;n:type:ShaderForge.SFN_Multiply,id:8442,x:32107,y:32810,varname:node_8442,prsc:2|A-1224-OUT,B-16-R;n:type:ShaderForge.SFN_ValueProperty,id:1224,x:31920,y:32810,ptovrint:False,ptlb:Distortion Rate,ptin:_DistortionRate,varname:node_1224,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:1045,x:31932,y:32490,varname:node_1045,prsc:2|A-8037-OUT,B-6375-G;n:type:ShaderForge.SFN_ComponentMask,id:6375,x:31751,y:32636,varname:node_6375,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-6985-OUT;n:type:ShaderForge.SFN_Append,id:708,x:32107,y:32639,varname:node_708,prsc:2|A-6375-R,B-1045-OUT;n:type:ShaderForge.SFN_Panner,id:4085,x:31755,y:32927,varname:node_4085,prsc:2,spu:1,spv:1|UVIN-6985-OUT,DIST-7480-TSL;n:type:ShaderForge.SFN_Time,id:7480,x:31568,y:32927,varname:node_7480,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:616,x:31556,y:32490,ptovrint:False,ptlb:Height,ptin:_Height,varname:node_616,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:8037,x:31751,y:32490,varname:node_8037,prsc:2|A-616-OUT,B-5935-OUT;n:type:ShaderForge.SFN_Vector1,id:5935,x:31556,y:32572,varname:node_5935,prsc:2,v1:2;proporder:4805-5983-16-1224-616;pass:END;sub:END;*/

Shader "Shader Forge/Reflection2D" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _node_16 ("node_16", 2D) = "black" {}
        _DistortionRate ("Distortion Rate", Float ) = 0.1
        _Height ("Height", Float ) = 0.5
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
        GrabPass{ }
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _Color;
            uniform sampler2D _node_16; uniform float4 _node_16_ST;
            uniform float _DistortionRate;
            uniform float _Height;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float2 node_6985 = ((sceneUVs * 2 - 1).rg*0.5+0.5);
                float2 node_6375 = node_6985.rg;
                float4 node_7480 = _Time;
                float2 node_4085 = (node_6985+node_7480.r*float2(1,1));
                float4 _node_16_var = tex2D(_node_16,TRANSFORM_TEX(node_4085, _node_16));
                float4 node_6258 = tex2D( _GrabTexture, (float2(node_6375.r,((_Height*2.0)-node_6375.g))+(_DistortionRate*_node_16_var.r)));
                float node_603 = (node_6258.a*_Color.a*i.vertexColor.a); // A
                float3 emissive = ((node_6258.rgb*_Color.rgb*i.vertexColor.rgb)*node_603);
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
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
