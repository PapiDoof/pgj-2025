Shader "Custom/FogOfWarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0,0,0,1) // Set fog color to black
        _FogDistance ("Fog Distance", Range(0, 1)) = 0.5 // Distance at which fog starts to appear
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" } // Set to Transparent
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Enable alpha blending

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
            float4 _FogColor; // This will be black
            float _FogDistance;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Calculate distance from each corner of the screen
                float distanceFromTopLeft = length(i.uv - float2(0.0, 0.0));
                float distanceFromTopRight = length(i.uv - float2(1.0, 0.0));
                float distanceFromBottomLeft = length(i.uv - float2(0.0, 1.0));
                float distanceFromBottomRight = length(i.uv - float2(1.0, 1.0));

                // Find the minimum distance from the corners
                float minDistance = min(min(distanceFromTopLeft, distanceFromTopRight), 
                                        min(distanceFromBottomLeft, distanceFromBottomRight));

                // Create a fog factor based on the minimum distance
                float fogFactor = smoothstep(_FogDistance, _FogDistance + 0.1, minDistance); 

                // Invert the fog factor to create the desired effect
                float invertedFogFactor = 1.0 - fogFactor;

                // Return the fog color (black) where fog is present and white (transparent) where there is no fog
                return fixed4(_FogColor.rgb, invertedFogFactor * _FogColor.a); 
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}