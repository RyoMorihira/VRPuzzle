﻿Shader "Custom/TransparentCullOff" {
	Properties {
	_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Pass {
			Tags { "RenderType"="Transparent" "Queue"="Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			float4 vert(float4 v:POSITION) : SV_POSITION {
				return mul (UNITY_MATRIX_MVP, v);
			}
			
			fixed4 frag() : SV_Target {
				return fixed4(1.0, 0.5, 0.5, 0.2);
			}
			ENDCG
		}
	}
}
