Shader "Custom/Grab/GrabScrewShader"
{
	Properties
	{
		//_DistortionTex("Distortion Texture(RG)", 2D) = "grey" {}
		_MixColor("MixColor",Color)=(1,1,1,1)
		_ScrewPower("Screw Power", Float) = 0
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGB

		GrabPass { "_RenderTexture" }

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				half4 vertex                : POSITION;
				half4 texcoord              : TEXCOORD0;
			};

			struct v2f {
				half4 vertex                : SV_POSITION;
				half2 uv                    : TEXCOORD0;
				half4 screenPos               : TEXCOORD1;
				half4 centerPos               : TEXCOORD2;
			};

			float4  _MixColor;
			sampler2D _RenderTexture;
			fixed4  _RenderTexture_TexelSize;
			half _ScrewPower;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.uv = v.texcoord.xy;
				float scaleX = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));
				o.vertex = mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(o.uv-0.5, 0, 0)*scaleX);
				o.screenPos = ComputeGrabScreenPos(o.vertex);
				o.centerPos = ComputeGrabScreenPos(mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1))));
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				const float TWO_PI = 3.14159;

				// レンダ―画像の解像度計算(単位px
				half2 pix = 1 / _RenderTexture_TexelSize.xy;
				// 中心位置と描画点の座標を計算(単位px)
				half2	center	= i.centerPos	*pix.xy / i.centerPos.w;
				half2	dest	= i.screenPos		*pix.xy / i.screenPos.w;


				
				half dist = max(1 - length(i.uv * 2 - half2(1, 1)), 0);
				half rot =dist*_ScrewPower;

				half2 d = dest - center;

				// 座標変換(単位px)
				half2 pos = center + half2(d.x*cos(rot)-d.y*sin(rot),d.x*sin(rot)+d.y*cos(rot));

				// 中心からの距離に応じで乗算色の強度を設定
				fixed3 mixCol = _MixColor.xyz*dist + fixed3(1, 1, 1)*(1-dist);

				return  tex2D(_RenderTexture, pos/ pix)*fixed4(mixCol,1);
			}
			ENDCG
		}
	}
}
