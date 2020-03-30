Shader "Custom/BossCharge"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,0)
        _Radius ("Radius", Float) = 0.5
        _DrawRadius ("DrawRadius", Float) = 0.1
        _Thresh ("Thresh", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Radius;
            float _DrawRadius;
            float _Thresh;
            float a;

            v2f vert (appdata_base v)
            {
                v2f o;
                float scale= length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));
                o.pos = mul(UNITY_MATRIX_P,
                float4(UnityObjectToViewPos(float4(0, 0, 0, 1)), 1) + float4(v.vertex.x, v.vertex.y, 0, 0)*scale);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float PI = 3.141592653589793;
                float2 d = (i.uv - 0.5) * 2;
                float r = length(d);
                return step(1 - modf((atan2(d.y,d.x)+1.5*PI)/(2*PI), a), _Thresh) * step(_Radius, r) * step(r, _Radius + _DrawRadius) * _Color;
            }

            ENDCG
        }
    }
}