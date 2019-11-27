Shader "PostEffect/GameOverGlitch"
{
    Properties
    {
        [HideInInspector]
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold("Threshold",Range(0,1))=0.8
        _Speed("Speed",Range(1,32))=16
        _NumberMin("NumberMin",int)=5
        _NumberMax("NumberMax",int)=15
        _NoiseSize("NoiseSize",Range(0,1))=0.3
        _ShiftLevel("ShiftLevel",Range(0,2.5))=0
        _AberrationDir("_AberrationDirection(RB)",Color)=(0,0,0,1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 param :TEXCOORD1;
            };


            sampler2D _MainTex;
            sampler2D _WaveTex;
            fixed _Threshold;
            fixed _Speed;
            fixed _NumberMin;
            fixed _NumberMax;
            fixed _NoiseSize;
            fixed _ShiftLevel;
            fixed4 _AberrationDir;


            // 乱数生成期
            float random (fixed a,fixed b) { 
                return frac(sin(dot(fixed2(a,b), fixed2(12.9898,78.233))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                // 共通のパラメータは処理負荷軽減のため頂点シェーダで計算
                fixed a = floor(_Time.y*_Speed);
                fixed rand = random(1,a)*100;
                fixed hikaku = random(1+rand,a);
                fixed kazu=random(2+rand,a)*(_NumberMax-_NumberMin)+_NumberMin;
                o.param=fixed4(a,rand,hikaku,kazu);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed2 adduv=fixed2(0,0);
                fixed2 uv0=i.uv;
                
                fixed a = i.param.x;
                fixed rand = i.param.y;
                fixed hikaku = i.param.z;
                fixed kazu=i.param.w;

                for(fixed i=0;i<kazu;i+=1){
                    // ずれの発生する領域の決定
                    fixed2 pos=fixed2(random(5+i+rand,a),random(6+i+rand,a));
                    fixed2 size=fixed2(random(3+i+rand,a),random(4+i+rand,a))*_NoiseSize;

                    // ずれの距離を決定
                    fixed2 dif=size*fixed2(random(7+i+rand,a),random(8+i+rand,a));

                    // 矩形領域内ならUVをずらす
                    adduv += dif * step(pos.x,uv0.x)*step(pos.y,uv0.y)*
                                    step(uv0.x,pos.x+size.x)*step(uv0.y,pos.y+size.y);
                }

                // 閾値を超えた時のみずれを適用
                fixed2 uv=uv0+adduv*step(hikaku,_Threshold);

                // 走査線の輝度を計算
                fixed scanLine=1-step(0.5,frac(uv.y*120+_Time.y))*0.1;

                // 横方向にシフト
                uv.x-=saturate(_ShiftLevel-random(0,uv.y))*1.5;

                
                fixed2 dir=fixed2(1,0.4)*0.05*saturate(_ShiftLevel);
                // テクスチャからR色を取得
                fixed4 colr = tex2D(_MainTex, uv);
                // テクスチャからG色を取得
                fixed4 colg = tex2D(_MainTex, uv+dir);
                // テクスチャからB色を取得
                fixed4 colb = tex2D(_MainTex, uv-dir);

                // 画面のちらつき
                fixed brightness = random(_Time.y,a)*0.1+0.9;
        
                // 走査線を適用して描画
                return fixed4(colr.r,colg.g,colb.b,1)*scanLine*brightness;
            }
            ENDCG
        }
    }
}
