Shader "Custom/Grab/GrabTexShader"
{
	Properties
	{
		_DistortionTex("Distortion Texture(RG)", 2D) = "grey" {}
		_DistortionPower("Distortion Power", Range(-1, 1)) = 0
		_DisplayScale("DisplayScale",Float) = 1
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGB

		GrabPass { "_GrabPassTexture" }

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
				half4 grabPos               : TEXCOORD1;
			};

			fixed4 _MixColor;
			sampler2D _DistortionTex;
			half4 _DistortionTex_ST;
			sampler2D _GrabPassTexture;
			half _DistortionPower;
			half _DisplayScale;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.uv = TRANSFORM_TEX(v.texcoord, _DistortionTex);
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex = mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(o.uv.x-0.5, o.uv.y-0.5, 0, 0)*_DisplayScale);
				o.grabPos = ComputeGrabScreenPos(o.vertex);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// w除算
				half2 uv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);

				// Distortionの値に応じてサンプリングするUVをずらす
				half2 distortion = tex2D(_DistortionTex, i.uv).rg - 0.5;
				distortion *= _DistortionPower;

				uv = uv + distortion;
				return tex2D(_GrabPassTexture, uv);
			}
			ENDCG
		}
	}
}
