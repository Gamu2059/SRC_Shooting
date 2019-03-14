Shader "Standard(Optimize)"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		[NoScaleOffset]
		_MainTex("Albedo (RGBA)", 2D) = "white" {}
		[NoScaleOffset][Normal]
		_NormalTex("Normal", 2D) = "bump" {}
		[NoScaleOffset]
		_MaskTex("Mask(Metal/Smooth/Emit)", 2D) = "white" {}
		[HDR]
		_EmitColor("EmitColor", Color) = (1,1,1,1)

		_Metallic("Metallic", Range(0,1)) = 1.0
		_Smoothness("Smoothness", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		fixed4 _Color;
		fixed4 _EmitColor;

		fixed _Metallic;
		fixed _Smoothness;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 m = tex2D(_MaskTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
			o.Alpha = c.a;
            // Metallic and smoothness come from slider variables
            o.Metallic = m.r*_Metallic;
			o.Smoothness = m.g*_Smoothness;
			o.Emission = _EmitColor * m.b;

			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
