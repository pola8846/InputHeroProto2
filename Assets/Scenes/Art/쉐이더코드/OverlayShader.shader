Shader "Custom/OverlayShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OverlayTex ("Overlay (RGB)", 2D) = "white" {}
        _OverlayStrength ("Overlay Strength", Range(0, 1)) = 1.0
        _DarkenStrength ("Darken Strength", Range(0, 1)) = 1.0
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

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _OverlayTex;
            float4 _MainTex_ST;
            float4 _OverlayTex_ST;
            float _OverlayStrength;
            float _DarkenStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 OverlayBlend(float4 baseColor, float4 overlayColor)
            {
                float3 result;
                result = (baseColor.rgb < 0.5) ? (2.0 * baseColor.rgb * overlayColor.rgb) : (1.0 - 2.0 * (1.0 - baseColor.rgb) * (1.0 - overlayColor.rgb));
                return float4(result, baseColor.a);
            }

            float4 DarkenBlend(float4 baseColor, float4 overlayColor)
            {
                float3 result;
                result = min(baseColor.rgb, overlayColor.rgb);
                return float4(result, baseColor.a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 baseColor = tex2D(_MainTex, i.uv);
                float4 overlayColor = tex2D(_OverlayTex, i.uv);

                float4 overlayBlended = OverlayBlend(baseColor, overlayColor);
                float4 darkenBlended = DarkenBlend(baseColor, overlayColor);

                float4 result = lerp(baseColor, overlayBlended, _OverlayStrength);
                result = lerp(result, darkenBlended, _DarkenStrength);

                return result;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
