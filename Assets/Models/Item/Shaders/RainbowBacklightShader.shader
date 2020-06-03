Shader "Custom/Item/RainbowBacklightShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_RingTex("RingTex", 2D) = "white" {}
		_GradationTex("GradationTex", 2D) = "white" {}
		_Color2("Color2",Color) = (1,1,1,1)
		_Speed("Speed",float) = 0.1
		_Size("Circle Size",float) = 0.1
		_RingSize("Ring Size",Range(0,1)) = 0.9
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			LOD 100
			ZWrite Off
			Blend One One

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
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _RingTex;
				float4 _RingTex_ST;
				sampler2D _GradationTex;
				float4 _GradationTex_ST;
				fixed4 _Color2;
				float _Speed;
				float _Size;
				float _RingSize;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					float2 uv2 = i.uv * 2 - 1;
					float r = length(uv2);
					float t = atan2(-uv2.y,uv2.x) + _Time.y * 0.1;

					fixed4 color = tex2D(_GradationTex, float2(t / 3.14159265 + _Time.x * 2,0));

					fixed4 col = tex2D(_MainTex, float2(t / 3.14159265,_Time.y * _Speed));

					fixed4 ringCol = tex2D(_RingTex, i.uv);

					return color * saturate((1 - r) * (col.r + 0.2)) * 2 + _Color2 * saturate((_Size - r + col.r * 0.5) * 4) + color * ringCol;
				}
				ENDCG
			}
		}
}
