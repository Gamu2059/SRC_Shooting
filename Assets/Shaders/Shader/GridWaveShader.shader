Shader "Custom/Grid/GridWaveShader"
{
	Properties
	{
		_WaveLevel("WaveLevel", Float) = 0
		_WaveSpeed("WaveSpeed", Float) = 0
		_WaveFrequency("WaveFrequency ", Float) = 4
		_LineWeight("LineWeight", Range(1, 16)) = 0.1
		_EmitLevel("EmitLevel", Float) = 1.3
		_Color("Color",Color) = (0,1,0,1)
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{ 
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		ZWrite Off
		Cull OFF
		Blend ONE ONE
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
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			half _WaveLevel;
			half _WaveSpeed;
			half _WaveFrequency;
			half _LineWeight;
			half _EmitLevel;
			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				float time = _Time * _WaveSpeed;
				float offsetY = sin((o.vertex.x- o.vertex.y) * _WaveFrequency) + sin((o.vertex.x + o.vertex.y) * _WaveFrequency);
				offsetY *= sin(time)*_WaveLevel;
				o.vertex.y += offsetY;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 texCol = tex2D(_MainTex, i.uv);
				fixed4 col = _Color * pow(texCol.r, _LineWeight) * _EmitLevel;

				return col;
			}
			ENDCG
		}
	}
}
