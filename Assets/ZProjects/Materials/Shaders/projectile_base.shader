Shader "weap/projectile_base" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_CutAlpha("CutAlpha", Range(0,1)) = 0
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_EmissionColor("EmissionColor",Color) = (0,0,0,0)
		_Emission("Emission",Range(0,10)) = 0
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off
		ZWrite On
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf LV alpha:auto//alphatest:_CutAlpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half4 LightingLV (SurfaceOutput s, half3 lightDir, half atten) {	
			half4 c;        
	        c.rgb = s.Albedo * s.Alpha * s.Emission;
	        c.a = s.Alpha;
        return c;
    }

		half _Glossiness;
		half _Metallic, _Emission;
		fixed4 _Color, _EmissionColor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Emission = _EmissionColor.rgb * _Emission;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
