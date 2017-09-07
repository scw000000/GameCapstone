Shader "Hidden/WorldSwitch"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TheOtherWorldTex("The other world texture", 2D) = "black"{}
		_TheOtherWorldDepthTex("The other World depth texture", 2D) = "black"{}
		_SphereRadius("Sphere radius", Float) = 1
		_SphereWidth("Sphere width", Float) = 1
		_EdgeSharpness("Edge sharpness", Float) = 2
		_MidColor("Mid color", Color) = (0, 0, 1, 1)
		_BarColor("Bar color", Color) = (0, 1, 0, 1)
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
			sampler2D _CameraGBufferTexture2;
			sampler2D _CameraGBufferTexture0;
			float _SphereRadius;
			float _SphereWidth;
			float _EdgeSharpness;
			float4x4 _InverseViewMat;
			float4 _MidColor;

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

			float4 HorBar(float y) {
				// Frac: return decimal part, which is [0,1)
				return 1 - saturate(round(abs(frac(y * 100))));
			}

			fixed4 frag (v2f i) : SV_Target
			{
			float mainSceneDepth = tex2D(_CameraDepthTexture, i.uv).r;
			
			float mainWorldDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
			float4 mainWorldPos = GetWorldPos(mainWorldDepth, i.uv);
			float otherWorldDepth = SAMPLE_DEPTH_TEXTURE(_TheOtherWorldDepthTex, i.uv);
			float4 otherWorldPos = GetWorldPos(otherWorldDepth, i.uv);
			// return tex2D(_MainTex, i.uv);
			// return tex2D(_TheOtherWorldTex, i.uv);
			// return float4(1.0, 0.0, 0.0, 1);
			// return tex2D(_TheOtherWorldTex, i.uv);
			float otherWorldDist = distance(otherWorldPos.xyz, _WorldSpaceCameraPos);
			float mainWorldDist = distance(mainWorldPos.xyz, _WorldSpaceCameraPos);
			if (mainWorldDist < _SphereRadius) {
				if (mainWorldDist > _SphereRadius - _SphereWidth) {
					float alpha = 1 - (_SphereRadius - mainWorldDist) / _SphereWidth;
					//return lerp(tex2D(_TheOtherWorldTex, i.uv), tex2D(_MainTex, i.uv), alpha);
					half4 gradientColor = lerp(_MidColor, tex2D(_TheOtherWorldTex, i.uv), pow(alpha, _EdgeSharpness));
					return lerp(tex2D(_MainTex, i.uv), gradientColor, alpha);// +HorBar(i.uv.y) * float4(0, 1, 0, 1);
																			 //return lerp(tex2D(_MainTex, i.uv), tex2D(_TheOtherWorldTex, i.uv), alpha);
				}
				return tex2D(_MainTex, i.uv);
				// return float4(1.0, 0.0 ,0.0, 1);
			}
			if (otherWorldDist < _SphereRadius) {
				if (otherWorldDist > _SphereRadius - _SphereWidth) {
					float alpha = 1 - (_SphereRadius - otherWorldDist) / _SphereWidth;
					//return lerp(tex2D(_TheOtherWorldTex, i.uv), tex2D(_MainTex, i.uv), alpha);
					half4 gradientColor = lerp(_MidColor, tex2D(_TheOtherWorldTex, i.uv), pow(alpha, _EdgeSharpness));
					return lerp(tex2D(_MainTex, i.uv), gradientColor, alpha);// +HorBar(i.uv.y) * float4(0, 1, 0, 1);
					//return lerp(tex2D(_MainTex, i.uv), tex2D(_TheOtherWorldTex, i.uv), alpha);
				}
				// return tex2D(_CameraGBufferTexture0, i.uv);
				return tex2D(_MainTex, i.uv);
				// return float4(1.0, 0.0 ,0.0, 1);
			}
			
			// return float4(1.0, 0.0, 0.0, 1);
			return tex2D(_TheOtherWorldTex, i.uv);
			//fixed4 mainCol = tex2D(_MainTex, i.uv);
			//	return tex2D(_CameraDepthTexture, i.uv);
			}
			ENDCG
		}
	}
}
