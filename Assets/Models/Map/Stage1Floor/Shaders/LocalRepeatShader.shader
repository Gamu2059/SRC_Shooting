Shader "Custom/LocalRepeatShader"
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
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NormalTex;
		sampler2D _MaskTex;

        struct Input
        {
            float2 uv_MainTex;
            float scaleZ;
			float4 screenPos;
            
            
            #ifdef ENABLE_DITHER
			float dist;
            #endif
        };
        
        sampler3D	_DitherMaskLOD;
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

        void vert(inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input, o);
        
            // オブジェクトのスケールを取得
            float scaleX = length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x));
            o.scaleZ =  length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))/scaleX;

            // タイリングが整数倍になるよう補正
            o.scaleZ=ceil(o.scaleZ);

            #ifdef ENABLE_DITHER
            o.dist = UnityObjectToViewPos(v.vertex).z;
            #endif
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            #ifdef ENABLE_DITHER
            // 画面距離によってディザリング
            float clampDist=min(max(abs(IN.dist)*2-0.95,0),1)*0.99;
			half alphaRef = tex3D(_DitherMaskLOD, float3(IN.screenPos.xy / IN.screenPos.w * _ScreenParams.xy * 0.25,clampDist)).a;
			clip(alphaRef - 0.01);
            #endif

            // 補正済みUV座標
            float2 uv=IN.uv_MainTex*float2(1,IN.scaleZ);

			fixed4 c = tex2D(_MainTex, uv) * _Color;
			fixed4 m = tex2D(_MaskTex, uv);
            o.Albedo = c.rgb;
			o.Alpha = c.a;
            
            o.Metallic = m.r*_Metallic;
			o.Smoothness = m.g*_Smoothness;
			o.Emission = _EmitColor * m.b;

			o.Normal = UnpackNormal(tex2D(_NormalTex, uv));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
