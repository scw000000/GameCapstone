Shader "Hidden/WorldSwitch"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TheOtherWorldTex("The other world texture", 2D) = "black"{}
		_TheOtherWorldDepthTex("The other World depth texture", 2D) = "black"{}
		_GradientTexture("Gradient texture", 2D) = "blue"{}
		_SphereRadius("Sphere radius", Float) = 1
		_SphereWidth("Sphere width", Float) = 1
		_BarColor("Bar color", Color) = (0, 1, 0, 1)
		_BarAlpha("Bar alpha", Float) = 0.5
		_GradientColorShift("Gradient color shift", Float) = 1
		_GradientColorUVShift("Gradient color uv shift", Float) = 1
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

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			sampler2D _TheOtherWorldTex;
			sampler2D _TheOtherWorldDepthTex;
			sampler2D _GradientTexture;
			float _SphereRadius;
			float _SphereWidth;
			float _GradientColorShift;
			float _GradientColorUVShift;
			float4x4 _InverseViewMat;
			float4 _BarColor;
			float _BarAlpha;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 GetWorldPos(float depth, float2 uv) {
				float linDepth = LinearEyeDepth(depth);
				// Get a vector that holds the FOV multipliers for our uv's
				float2 projectionMultipliers = float2(UNITY_MATRIX_P._11, UNITY_MATRIX_P._22);
				// convert from screenSpace to viewSpace by applying a reverse projection procedure
				float3 vpos = float3(
					// convert UV's so they represent a coordinate system with its origin in the middle
					(uv * 2 - 1) / projectionMultipliers, -1)
					// division translates uv's back from our screens aspect ratio to a quadratic space
					// -1 denotes a depth of -1, so in the next step we translate AWAY from the origin

					* linDepth; // slide the whole coordinates by the depth in a reverese projection

								// convert from viewSpace to worldSpace
				return mul(_InverseViewMat, float4(vpos, 1));
			}

			// Return 0, 1 integer value based on screen space
			float4 HorBar(float y) {
				// Frac: return decimal part, which is [0,1)
				return 1 - saturate(round(abs(frac(y * 100))));
			}

			fixed4 frag (v2f i) : SV_Target
			{
			float4 mainWorldPos = GetWorldPos(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv), i.uv);
			float4 otherWorldPos = GetWorldPos(SAMPLE_DEPTH_TEXTURE(_TheOtherWorldDepthTex, i.uv), i.uv);
			float mainWorldDist = distance(mainWorldPos.xyz, _WorldSpaceCameraPos);
			if (mainWorldDist < _SphereRadius) {
				if (mainWorldDist > _SphereRadius - _SphereWidth) {
					float alpha = 1 - (_SphereRadius - mainWorldDist) / _SphereWidth;
					float npAlpha = (alpha - 0.5) * 2;
					float4 gradientColor = lerp(tex2D(_GradientTexture, float2(pow(alpha, _GradientColorUVShift), 0)), HorBar(i.uv.y) * _BarColor, _BarAlpha);
					return lerp(tex2D(_MainTex, i.uv), gradientColor, pow(alpha, _GradientColorShift));
				}
				return tex2D(_MainTex, i.uv);
			}
			float otherWorldDist = distance(otherWorldPos.xyz, _WorldSpaceCameraPos);
			if (otherWorldDist < _SphereRadius) {
				if (otherWorldDist > _SphereRadius - _SphereWidth) {
					float alpha = 1 - (_SphereRadius - otherWorldDist) / _SphereWidth;
					float4 gradientColor = lerp(tex2D(_GradientTexture, float2(pow(alpha, _GradientColorUVShift), 0)), HorBar(i.uv.y) * _BarColor, _BarAlpha);
					return lerp(tex2D(_MainTex, i.uv), gradientColor, pow(alpha, _GradientColorShift));
				}
				return tex2D(_MainTex, i.uv);
			}
			return tex2D(_TheOtherWorldTex, i.uv);
			}
			ENDCG
		}
	}
}
