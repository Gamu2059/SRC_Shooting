// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Animation/LineWaveShader"
{
	Properties
	{
		_DotDensity("DotDensity", Float) = 32
		_LineDensity("LineDensity", Float) = 32
		_Axis("Axis", Color) = (0,1,0,1)
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

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};


			half _DotDensity;
			half _LineDensity;
			half _WaveSpeed;
			half4 _Axis;
			half _EmitLevel;
			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half x = dot(i.worldPos,normalize(_Axis.xyz));
				half val = (sin(x*_LineDensity+_Time.x*_WaveSpeed)+1)*0.5;
				half dotVal = abs(sin(i.uv.x*_DotDensity)*sin(i.uv.y*_DotDensity));


				return _Color * _EmitLevel*val*dotVal;
			}
			ENDCG
		}
	}
}
