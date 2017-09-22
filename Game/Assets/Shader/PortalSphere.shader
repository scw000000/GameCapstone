Shader "Unlit/PortalSphere"
{
	Properties
	{
		_Color("Sphere Color", Color) = (0, 0, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_ScanLineBandYCenter("ScanLine Y Center", Float) = 0.1
		_ScanLineBandWidth("ScanLine Y width", Float) = 0.1
		_BandColor("Band color", Color) = (0, 1, 0, 1)
		_BandAlpha("Band alpha", Float) = 0.5
		_BarColor("Bar color", Color) = (0, 1, 0, 1)
		_BarAlpha("Bar alpha", Float) = 0.5
		_FresnelPow("_FresnelPow ", Float) = 5
		_FresnelR("_FresnelR", Float) = 0.5
		_PulseFactor("_PulseFactor", Float) = 1
	}
	SubShader
	{
		//	Tags{ "RenderType" = "Opaque" }
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100
		 Blend One One
		ZWrite Off
		Cull Off
		//	Cull Back
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(1)
				float4 screenPos : TEXCOORD2;
				float4 objPos : TEXCOORD3;
				float fragDepth : DEPTH;
				float3 fresnel : TEXCOORD4;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _CameraDepthTexture;
			float4 _Color;
			float4 _BarColor;
			float4 _BandColor;
			float _ScanLineBandYCenter;
			float _ScanLineBandWidth;
			float _BandAlpha;
			float4x4 _InverseViewMat;
			float _FresnelPow;
			float _FresnelR;
			float _PulseFactor;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.fragDepth = -UnityObjectToViewPos(v.vertex).z *_ProjectionParams.w;
				o.objPos = v.vertex;

				float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
				o.fresnel = saturate(1.0 - dot(v.normal, viewDir));
				o.fresnel = pow(o.fresnel, _FresnelPow);
				o.fresnel = _FresnelR + (1.0 - _FresnelR) * o.fresnel;
				return o;
			}

			// Return 0, 1 integer value based on screen space
			float4 HorBar(float y) {
				// Frac: return decimal part, which is [0,1)
				return 1 - saturate(round(abs(frac(y * 100))));
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			float screenDepth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);
			float depthDiff = screenDepth - i.fragDepth;
			float depthWeight = 0;

			if (depthDiff > 0)
				depthWeight = 1 - smoothstep(0, _ProjectionParams.w, depthDiff);

			fixed4 col = _Color.a * _Color + depthWeight;
			float distanceToCenterY = abs(i.objPos.y - _ScanLineBandYCenter);

			if (abs(1 - distanceToCenterY ) < _ScanLineBandWidth) {
				distanceToCenterY = abs(1 - distanceToCenterY );
			}
			if (distanceToCenterY < _ScanLineBandWidth) {
				float4 bacCol = HorBar(i.objPos.y) * _BarColor;
				float4 bandCol = lerp(_BandColor, bacCol, bacCol.a) * _BandAlpha;
				col = lerp(bandCol, col, pow(distanceToCenterY/ _ScanLineBandWidth, 0.5 )) ;
			}
			return col * fixed4(i.fresnel, 1) * 2 * _PulseFactor;
			
			//	UNITY_APPLY_FOG(i.fogCoord, col);
			//	return _Color;
			}
			ENDCG
		}
	}
}
