Shader "Custom/Grab/WhiteNoiseShader"
{
    Properties
    {
		_MaskTex("Texture", 2D) = "black" {}
		_Strength("Strength", Range(0,1)) = 1
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
                float4 vertex				: SV_POSITION;
				half2 uv                    : TEXCOORD0;
				half4 grabPos               : TEXCOORD1;
            };

			sampler2D _GrabPassTexture;
            sampler2D _MaskTex;
			half _Strength;

			float random(fixed2 p) {
				return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
			}

            v2f vert (appdata v)
            {
				v2f o = (v2f)0;
				o.uv = v.uv;
				float scaleX = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));

				o.vertex = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(o.uv - 0.5, 0, 0)*scaleX);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half2 uv = half2(i.grabPos.x / i.grabPos.w, i.grabPos.y / i.grabPos.w);

				fixed4 mask = tex2D(_MaskTex, i.uv);

				return fixed4(tex2D(_GrabPassTexture, uv).xyz + (random(i.uv+fixed2(_Time.x,0))-0.5)*mask.x*_Strength, 1);
            }
            ENDCG
        }
    }
}
