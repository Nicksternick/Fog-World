Shader "Custom/DistanceFadeWithReference"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base Texture", 2D) = "white" { }
        _FadeDistance ("Fade Distance", Float) = 10.0
        _MinOpacity ("Min Opacity", Float) = 0.0
        _MaxOpacity ("Max Opacity", Float) = 1.0
        _ReferencePosition ("Reference Position", Vector) = (0, 0, 0, 0) // Custom reference point (e.g., the player's position)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
                float3 worldPos : TEXCOORD0;
            };

            float _FadeDistance;
            float _MinOpacity;
            float _MaxOpacity;
            float4 _Color;
            float3 _ReferencePosition; // Custom reference position
            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                o.worldPos = v.worldPos;
                return o;
            }

            float CalculateOpacity(float distance)
            {
                float opacity = 1.0 - (distance / _FadeDistance);
                opacity = clamp(opacity, _MinOpacity, _MaxOpacity);
                return opacity;
            }

            half4 frag(v2f i) : SV_Target
            {
                float distance = length(i.worldPos - _ReferencePosition); // Use custom reference position
                float opacity = CalculateOpacity(distance);
                half4 texColor = tex2D(_MainTex, i.worldPos.xy);
                texColor.a *= opacity; // Adjust alpha based on distance from reference position
                return texColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}
