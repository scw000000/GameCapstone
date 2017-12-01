Shader "Custom/Reflective_Bumped_Specular_PortalRendering" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess("Shininess", Range(0.01, 1)) = 0.078125
		_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
		_MainTex("Base (RGB) RefStrGloss (A)", 2D) = "white" {}
	_Cube("Reflection Cubemap", Cube) = "" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_OutOrInScalar("Outside or inside scalar", Float) = 1
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 400
		CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _BumpMap;
	samplerCUBE _Cube;

	fixed4 _Color;
	fixed4 _ReflectColor;
	half _Shininess;
	float4 _SphereCenter;
	float _SphereRadius;
	float _OutOrInScalar;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 worldRefl;
		float3 worldPos;
		INTERNAL_DATA
	};

	void surf(Input IN, inout SurfaceOutput o) {
		// First decide if we need to draw the pixel or not
		clip(_OutOrInScalar*(distance(_SphereCenter.xyz, IN.worldPos) - _SphereRadius));

		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 c = tex * _Color;
		o.Albedo = c.rgb;

		o.Gloss = tex.a;
		o.Specular = _Shininess;

		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

		float3 worldRefl = WorldReflectionVector(IN, o.Normal);
		fixed4 reflcol = texCUBE(_Cube, worldRefl);
		reflcol *= tex.a;
		o.Emission = reflcol.rgb * _ReflectColor.rgb;
		o.Alpha = reflcol.a * _ReflectColor.a;
	}
	ENDCG
	}

	FallBack "Diffuse"
}
