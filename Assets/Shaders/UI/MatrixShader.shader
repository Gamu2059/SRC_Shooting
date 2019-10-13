Shader "UI/MatrixShader"
{
    Properties
    {
        [NoScaleOffset]
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (0,0,0,1)
        _TextColor ("Text Color", Color) = (1,1,1,1)
        _Power("Brightness Power", Float) = 1
        _SplitY("Split Y", Range(1,100)) = 32
        _Repeat("Repeat", Range(0,10)) = 1
        _FallSpeed("Fall Speed", Range(0,1)) = 1
        _CharAspect("Character Aspect", Range(0.01,10)) = 1
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

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 pos : TEXCOORD0;
                float3 screenSize : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _TextColor;
            float4 _BaseColor;
            float _Repeat;
            float _FallSpeed;
            float _SplitY;
            float _CharAspect;
            float _Power;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                //スクリーン座標に変換
                o.pos = ComputeScreenPos(o.vertex);


                // スクリーン解像度の計算
                float4 projectionSpaceUpperRight = float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y);
                float4 viewSpaceUpperRight = mul(unity_CameraInvProjection, projectionSpaceUpperRight);

                // xyに解像度、zにアスペクト比
                o.screenSize.xy=viewSpaceUpperRight.xy;
                o.screenSize.z=viewSpaceUpperRight.x/viewSpaceUpperRight.y;
                return o;
            }

            // valをn階調に変換
            float dc(float val,float n){
                return floor(val*n)/n;
            }

            // 疑似乱数
            // フリッカーが発生しないように簡易的に実装
            float random (float p) { 
                return sin(p*23.234657)+cos(p*1324.325246);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.pos.xy*float2(i.screenSize.z,1)*_Repeat);

                // 縦の文字数を計算
                float ny=_SplitY*_Repeat;
                // アスペクト比から横の文字数を計算
                float nx=ny*i.screenSize.z/_CharAspect;

                // スクロール量の計算
                // float scroll=dc(_FallSpeed*_Time.y/_Repeat,ny)+dc(random(dc(i.pos.x,nx)),ny); // 文字内で色の段差ができないようにする
                float scroll=_FallSpeed*_Time.y/_Repeat+random(dc(i.pos.x,nx));

                // 正規化して反転
                float value=1-frac(i.pos.y+scroll);

                // 色の乗算
                col*=pow(value,_Power);
                return _BaseColor*(1-col.x)+_TextColor*col.x;
            }
            ENDCG
        }
    }
}
