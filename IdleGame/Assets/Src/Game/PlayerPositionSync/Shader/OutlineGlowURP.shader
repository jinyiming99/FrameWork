Shader "OutlineGlowURP"
{
    Properties
    {
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _Range("Range", Range(0,5)) = 1
    }

    // Shader实体
    SubShader
    {

        Tags{
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass
        {
            Tags
            {
                "LightMode"="UniversalForward"
            }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _Range;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 normal = normalize(TransformObjectToWorldNormal(v.normal));

                float3 worldPos = TransformObjectToWorld(v.vertex);

                o.pos = TransformObjectToHClip(v.vertex);
                o.color = float4(_OutlineColor.rgb, (1 - pow( abs(dot(normalize(GetCameraPositionWS() - worldPos), normal)),_Range)) * _OutlineColor.a);
                return o;
            }

            // 片段着色器
            half4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
        Pass{
            Tags
            {
                "RenderPipeline"="SRPDefaultUnlit"
            }
            ZWrite On
            ColorMask 0
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    
            float4 Vertex(float4 vertexPos : POSITION):SV_POSITION
            {    
                return TransformWorldToHClip(TransformObjectToWorld(vertexPos));   
            }
            half4 Fragment(void): COLOR
            {
                return half4(0,0,0,0);
            }
            ENDHLSL
        }
    }
}