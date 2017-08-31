Shader "Hidden/FadeIn"
{
	Properties
	{
		_MainTex ("ScreenTexture", 2D) = "white" {}
		_FadePattern("FadePattern", 2D) = "white" {}
		_FadeColor("FadeColor", Color) = (0,0,0,1)
		_Threshold("Threshold", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"


			uniform sampler2D _MainTex;
			uniform sampler2D _FadePattern;
			uniform float _Threshold;
			uniform float4 _FadeColor;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//float patternIntensity = dot(tex2D(_FadePattern, i.uv).rgb, float3(0.3, 0.59, 0.11));
				float patternIntensity = 1 - tex2D(_FadePattern, i.uv).a;
				if (patternIntensity < _Threshold ) {
					//col = float4(patternIntensity, patternIntensity, patternIntensity, 1);
				    
				}
				else {
					col = _FadeColor;
				}
				// just invert the colors
				// col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
