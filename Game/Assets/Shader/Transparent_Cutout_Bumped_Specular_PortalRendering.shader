Shader "Custom/Transparent_Cutout_Bumped_Specular_PortalRendering" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_MainTex("Base (RGB) TransGloss (A)", 2D) = "white" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_OutOrInScalar("Outside or inside scalar", Float) = 1
	}

		SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 400

		CGPROGRAM
#pragma surface surf BlinnPhong alphatest:_Cutoff
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _BumpMap;
	fixed4 _Color;
	half _Shininess;
	float4 _SphereCenter;
	float _SphereRadius;
	float _OutOrInScalar;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 worldPos;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		// First decide if we need to draw the pixel or not
		clip(_OutOrInScalar*(distance(_SphereCenter.xyz, IN.worldPos) - _SphereRadius));

		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = tex.rgb * _Color.rgb;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG
	}
	FallBack "Diffuse"
}
