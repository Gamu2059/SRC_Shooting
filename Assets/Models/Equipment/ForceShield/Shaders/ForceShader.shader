Shader "Unlit/ForceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color",Color) = (1,1,1,1)
        _BaseAlpha("Base Alpha",Range(0,1))=0.2
        _ScrollSpeed("Scroll Speed",Float)=0
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        ZWrite Off
        Cull off
        Blend SrcAlpha One // スクリーン合成

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half vdotn : TEXCOORD1;
            };

            float _F0;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _BaseAlpha;
            float _ScrollSpeed;
            
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
                // テクスチャ色の取得
                fixed4 col=tex2D(_MainTex, i.uv+float2(0,_Time.y*_ScrollSpeed));

                // フレネル値に応じて色を変更
                return _Color*(_BaseAlpha+(1-i.vdotn)*col);
            }
            ENDCG
        }
    }
}
