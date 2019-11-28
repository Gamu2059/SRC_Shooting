Shader "Custom/WireframeGlowAdditive"
{
    Properties
    {
         _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,1,0,1)
        _RimStrength("RimStrength", Range(0,4)) = 0.2
    }
    SubShader
    {
        Tags { "Queue" = "Transparent+1" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off  ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half vdotn : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            half _RimStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                // カメラへのベクトルを計算
                half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.vdotn = abs(dot(viewDir, v.normal.xyz));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                return saturate(col+(1-i.vdotn)*_RimStrength)*_Color;
            }
            ENDCG
        }
    }
}
