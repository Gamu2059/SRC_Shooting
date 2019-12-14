Shader "Custom/NewImageEffectShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _PosX ("_PosX", Float) = 0
        _PosY ("_PosY", Float) = 0
        _Radius ("_Radius", Float) = 0
        
        // グリッド数
        _GridNumX ("_GridNumX", Float) = 10
        _GridNumY ("_GridNumY", Float) = 10

        // アスペクト比
        _Aspect ("Aspect", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float _PosX;
            float _PosY;
            float _Radius;

            float _GridNumX;
            float _GridNumY;
            float _Aspect;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv) * i.color;

                #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                    clip (color.a - 0.001);
                #endif

                float x = floor(i.uv.x * _GridNumX);
                // 分割数よりもインデックスが大きければ、1引く
                x -= step(x, _GridNumX);

                float y = floor(i.uv.y * _GridNumY);
                // 分割数よりもインデックスが大きければ、1引く
                y -= step(y, _GridNumY);

                // グリッドの中心座標を求める
                float factX = x / _GridNumX + 1 / (_GridNumX * 2);
                float factY = y / _GridNumY + 1 / (_GridNumY * 2);

                float dx = _PosX - factX;
                float dy = (_PosY - factY) * max(_Aspect, 0.0001);
                float len = length(float2(dx, dy));

                // 半径より内側にあるグリッドの中心座標がある場合は、何も描画しない
                int isSmallerLength = step(len, _Radius);
                color.a -= isSmallerLength;
                return color;
            }
            ENDCG
        }
    }
}
