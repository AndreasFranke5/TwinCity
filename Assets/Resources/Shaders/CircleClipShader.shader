Shader "Custom/SimpleCircleClipLitWithAmbient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ClipCenter ("Clip Center", Vector) = (0,0,0,0)
        _ClipRadius ("Clip Radius", Float) = 5.0
        _Color ("Color Tint", Color) = (1,1,1,1)
        _LightDir ("Light Direction", Vector) = (0,1,0,0)
        _Ambient ("Ambient Light", Float) = 0.2
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

            sampler2D _MainTex;
            float4 _ClipCenter;
            float _ClipRadius;
            fixed4 _Color;
            float4 _LightDir;
            float _Ambient;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Clip outside circle
                float2 diff = i.worldPos.xz - _ClipCenter.xz;
                if (length(diff) > _ClipRadius)
                    discard;

                fixed4 texcol = tex2D(_MainTex, i.uv) * _Color;

                // Simple Lambert lighting with fixed light direction
                float NdotL = saturate(dot(normalize(i.normalDir), normalize(_LightDir.xyz)));

                // Add ambient lighting to avoid fully black shadows
                float lighting = max(NdotL, _Ambient);

                fixed3 lit = texcol.rgb * lighting;

                return fixed4(lit, texcol.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
