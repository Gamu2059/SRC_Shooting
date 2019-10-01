Shader "Custom/Grid/GridShader"
{
    Properties
    {
		_GridSize("GridSize",Float) = 1
		_LineWeight("LineWeight", Range(0.001, 1)) = 0.1
		_EmitLevel("EmitLevel", Float) = 1.3
		_Color ("Color",Color) = (0,1,0,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
	{
		Tags {"RenderType" = "Opapue"}
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			half _GridSize;
			half _LineWeight;
			half _EmitLevel;
			float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_ObjectToWorld, v.vertex).xz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 texCol = tex2D(_MainTex, i.uv/ _GridSize);
				return _Color * floor(texCol.r / (1.0 - _LineWeight)) * _EmitLevel;
            }
            ENDCG
        }
    }
}
