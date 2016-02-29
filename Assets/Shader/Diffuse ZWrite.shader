Shader "Transparent/Diffuse ZWrite" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionColor ("Color", Color) = (0,0,0,1)
 		_EmissionMap ("Emission", 2D) = "white" { }
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProject"="True" "RenderType"="Transparent" }
		LOD 200

		// extra pass that renders to depth buffer only
		Pass {
			ZWrite On
			ColorMask 0
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		uniform vec4 _EmissionColor;
		uniform  sampler2D _EmissionMap;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = c.rgb * tex2D(_Illum, IN.u_Illum).a;
		}

		// paste in forward rendering passes from Transparent/Diffuse
		UsePass "Transparent/Diffuse/FORWARD"
	}

	Fallback "Transparent/VertexLit"
}
