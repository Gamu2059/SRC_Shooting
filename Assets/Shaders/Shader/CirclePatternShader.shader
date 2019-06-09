Shader "Custom/Animation/CirclePatternShader"
{
	Properties
	{
		_DotDensity("DotDensity", Float) = 32
		_CircleDensity("CircleDensity", Float) = 20
		_WaveSpeed("WaveSpeed", Float) = 4
		_EmitLevel("EmitLevel", Float) = 1.3
		_Color("Color",Color) = (0,1,0,1)
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		ZWrite Off
		Cull BACK
		Blend ONE ONE
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};


			half _DotDensity;
			half _CircleDensity;
			half _WaveSpeed;
			half _EmitLevel;
			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				half waveVal = sin((length(frac(i.uv)-half2(0.5,0.5))-_Time.x*_WaveSpeed)*_CircleDensity)+1;
				half dotVal = abs(sin(i.uv.x*_DotDensity)*sin(i.uv.y*_DotDensity));


				fixed4 texCol = tex2D(_MainTex, i.uv);
				fixed4 col = _Color * _EmitLevel*(dotVal*waveVal);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
