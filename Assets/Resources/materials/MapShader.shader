Shader "Unlit/MapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DetailTex ("Detail Texture", 2D) = "white" {}
        _LightTex("Light Texture", 2D) = "white" {}
        _VisibilityTex("Visibility Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float2 uv4 : TEXCOORD3;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD2;
                float2 uv4 : TEXCOORD3;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DetailTex;
            sampler2D _LightTex;
            sampler2D _VisibilityTex;

            float4 _MainTex_ST;
            float4 _DetailTex_ST;
            float4 _LightTex_ST;
            float4 _VisibilityTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _DetailTex);
                o.uv3 = TRANSFORM_TEX(v.uv3, _LightTex);
                o.uv4 = TRANSFORM_TEX(v.uv4, _VisibilityTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col1 = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_DetailTex, i.uv2);
                fixed4 col3 = tex2D(_LightTex, i.uv3);
                fixed4 col4 = tex2D(_VisibilityTex, i.uv4);
                if (col2.a == 1)
                    return col2 * col3 * col4;
                else
                    return col1 * col3 * col4;
            }
            ENDCG
        }
    }
}
