Shader "Gear/flame_Test" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("State 0 (RGB)", 2D) = "white" {}
		_SubTex("State 1 (RGB)", 2D) = "white" {}
		_EmissionColor("EmissionColor",Color) = (0,0,0,0)
		_Emission("Emission",Range(0,10)) = 0
		_Frequency("State Frequency",Range(1,100)) = 10
		_Speed("Speed Range",Range(0,1)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		LOD 200
		ZWrite On
		Cull Off
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf flame alpha:auto

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _SubTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Emission, _Frequency, _Speed;
		fixed4 _Color, _EmissionColor;

		half4 Lightingflame(SurfaceOutput s, half3 lightDir, half atten) {	
			half4 c;        
	        c.rgb = s.Albedo * s.Alpha * s.Emission;
	        c.a = s.Alpha;
       		return c;
    	}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			IN.uv_MainTex.x *= 1/(_Speed + 0.1);

			fixed4 c0 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 c1 = tex2D (_SubTex, IN.uv_MainTex) * _Color;
			fixed4 c = lerp(c0,c1,abs(sin(_Time.y * _Frequency)));
			o.Albedo = c.rgb;
			o.Alpha = IN.uv_MainTex.x > 0.99? 0 : c.a;
			o.Emission = _EmissionColor.rgb * _Emission * (1 + _Speed/10.0);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
