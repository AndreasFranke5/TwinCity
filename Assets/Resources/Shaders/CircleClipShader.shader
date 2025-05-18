Shader "Custom/CircleClipShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ClipCenter ("Clip Center", Vector) = (0,0,0,0)
        _ClipRadius ("Clip Radius", Float) = 5.0
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

            sampler2D _MainTex;
            float4 _ClipCenter;
            float _ClipRadius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 clipXZ = i.worldPos.xz - _ClipCenter.xz;
                if (length(clipXZ) > _ClipRadius)
                    discard;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
