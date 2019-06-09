Shader "Custom/Grab/TranslateShader"
{
    Properties
    {
		_TranslateTex("TranslateTex", 2D) = "normal" {}
		_Level("Level", Range(0,1)) = 0
		_WaveSpeed("WaveSpeed", Float) = 0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent+100" "RenderType" = "Transparent" }

		Cull Back
		ZWrite Off
		ZTest LEqual
		ColorMask RGB

		GrabPass { "_GrabPassTexture" }

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
				float4 grabPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _TranslateTex;
            float4 _TranslateTex_ST;
			sampler2D _GrabPassTexture;
			float4 _GrabPassTexture_ST;
			half _Level;
			half _WaveSpeed;

            v2f vert (appdata v)
            {
				v2f o;
				o.uv = v.uv;
				float scaleX = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));

				o.vertex = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(o.uv - 0.5, 0, 0)*scaleX);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

				fixed4 translation = tex2D(_TranslateTex, i.uv);
				


				half2 uv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);
				uv += translation.xy*half2(1, _GrabPassTexture_ST.y / _GrabPassTexture_ST.x)*_Level*(sin(_Time.y*_WaveSpeed)+1)/2;

				

                // sample the texture
                fixed4 col = tex2D(_GrabPassTexture, uv);
                return col;
            }
            ENDCG
        }
    }
}
