// Disolve URL (2020/02/29) : https://www.patreon.com/posts/quick-game-art-11304399
Shader "Unlit/ForceDissolveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color",Color) = (1,1,1,1)
        _BaseAlpha("Base Alpha",Range(0,1))=0.2
        _ScrollSpeed("Scroll Speed",Float)=0

        _NoiseTex("Dissolve Noise", 2D) = "white"{} // Texture the dissolve is based on
        _NScale ("Noise Scale", Range(0, 10)) = 1 // Color of the dissolve Line
        _DisAmount("Dissolve Amount", Range(0, 1)) = 0 // amount of dissolving going on
        _DisAmountOffset0("Dissolve Offset 0", Float) = 0 // amount が 0の時に適用されるオフセット値
        _DisAmountOffset1("Dissolve Offset 1", Float) = 0 // amount が 1の時に適用されるオフセット値
        
        _DisLineWidth("Dissolve Width", Range(0, 2)) = 0 // width of the line around the dissolve
        _DisLineColor("Dissolve Color", Color) = (1,1,1,1) // Color of the dissolve Line
        _DisLineColorEx("Dissolve Color Extra", Color) = (1,1,1,1) // Color of the dissolve Line
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
                float3 w : POSITION1;
                float amount : COLOR1;
            };

            float _F0;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _BaseAlpha;
            float _ScrollSpeed;

            sampler2D _NoiseTex;
            float _DisAmount, _NScale;
            float _DisAmountOffset0, _DisAmountOffset1;
            float _DisLineWidth;
            float4 _DisLineColor, _DisLineColorEx;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // カメラへのベクトルを計算
                half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                o.vdotn = abs(dot(viewDir, v.normal.xyz));
                o.w = v.vertex;
                // o.w = mul(unity_ObjectToWorld, v.vertex);
                o.amount = lerp(_DisAmountOffset0, _DisAmountOffset1, _DisAmount) + _DisAmount;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // テクスチャ色の取得
                fixed4 col = tex2D(_MainTex, i.uv+float2(0,_Time.y*_ScrollSpeed));
                // フレネル値に応じて色を変更                
                fixed4 c = _Color*(_BaseAlpha+(1-i.vdotn)*col);
                fixed4 n = tex2D(_NoiseTex, i.w.xz * _NScale);

                // ディゾルブ
                // 普通のディゾルブとは違い、アルファだけディゾルブのように加工する
                float DissolveLineIn = step(n.r - _DisLineWidth, i.amount);
                float DissolveLineInExtra = step(n.r - (_DisLineWidth + 0.2), i.amount) - DissolveLineIn;
                float NoDissolve = 1.0 - DissolveLineIn - DissolveLineInExtra;
                c.a = (DissolveLineIn * _DisLineColor.a * c.a) + (DissolveLineInExtra * _DisLineColorEx.a * c.a) + (NoDissolve * c.a);
                return c;
            }
            ENDCG
        }
    }
}
