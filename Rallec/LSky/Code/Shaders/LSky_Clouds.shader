// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/////////////////////////////////////////////
/// LSKy
/// ================
///
/// Description:
/// ================
/// Simple clouds.
/////////////////////////////////////////////


Shader "Rallec/LSky/Clouds"
{

	Properties
	{
	[MaterialToggle] _TestBool ("Test Bool", Float) = 0
	// Type
	[Header(Cloud Type)]
	[MaterialToggle]_NoiseBasedClouds("Noise Based Clouds", Float) = 0

	[Header(NOISE BASED CLOUDS)]
	// Color
	[Header(Color)]
	_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
	_EdgeColor("Edge Color", Color) = (0.5, 0.5, 0.5, 1)
	_ColorEdgeHeight("Color Edge Height", Range(0.0, 0.1)) = 0.25
	_Transparency("Transparency", Range(0, 1)) = 1

	// Shape
	[Header(Shape)]
	_NoiseScale("Noise Scale", Float) = 100
	_Power("Noise Power", Float) = 1
	_Clipping("Clipping", Range(0, 1)) = 0 //lsky_CloudsCoverage("Clouds Coverage", Range(0, 1)) = 0
	_Smoothness("Smoothness", Range(0, 1)) = 0.1
	_DistortionNoiseScale("Distortion Noise Scale", Float) = 100
	_Distortion("Distortion", Range(0, 1)) = 0.1

	// Texture based
	[Header(TEXTURE BASED CLOUDS)]
	_MainTex("Main Texture", 2D) = "white" {}
	lsky_CloudsDensity("Clouds Density", Range(0, 1)) = 0.5
	lsky_CloudsCoverage("Clouds Coverage", Range(0, 1)) = 0.5
	}
	CGINCLUDE

	uniform float _TestBool;
		// Includes
		//--------------------------
		#include "LSky_Common.cginc"
		//==========================

		// Type
		uniform float _NoiseBasedClouds;

		// Colors
		//--------------------------
		uniform half4 _Color;
		uniform half4 _EdgeColor;
		uniform float _ColorEdgeHeight;

		// Color
		//-------------------------
		uniform half  _Intensity;
		uniform half4 _Tint; // TODO Delete ?
		//=========================

		// Texture
		//---------------------------------
		uniform half lsky_CloudsDensity;
		uniform half  lsky_CloudsCoverage;
		sampler2D  _MainTex;
		float4     _MainTex_ST;
		//======================

		// Noise
		//---------------------------------
		uniform half _Clipping;
		half _NoiseScale;
		half _Smoothness;
		half _Power; // = Density
		half _Transparency;
		half _DistortionNoiseScale;
		half _Distortion;
		//=================================


		struct v2f
		{
			float2 texcoord : TEXCOORD0;
			half4  col      : TEXCOORD2;
			float4 vertex   : SV_POSITION;
			float3 worldPos : TEXCOORD3;
			float3 objectScale : TEXCOORD4;
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f vert(appdata_base v)
		{

			// Init
			//--------------------------------------
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			//======================================

			//
			//-------------------------------------------
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			//===========================================

			// Position - coords - horizon fade.
			//--------------------------------------------------------
			o.vertex   = UnityObjectToClipPos(v.vertex);
			#ifdef UNITY_REVERSED_Z
	  		o.vertex.z = 1e-5f;
  			#else
	  		o.vertex.z = o.vertex.w - 1e-5f;
  			#endif
			//--------------------------------------------------------
			if(_NoiseBasedClouds == 0)
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			else
				o.texcoord = v.texcoord;

			o.col.rgb = _Color.rgb;
			o.col.a    = normalize(v.vertex.xyz).y * 10;
			//========================================================
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			return o;
		}

		// ====================================================
		// --- Unity Gradient Noise ---

		float2 unity_gradientNoise_dir(float2 p)
		{
			p = p % 289;
			float x = (34 * p.x + 1) * p.x % 289 + p.y;
			x = (34 * x + 1) * x % 289;
			x = frac(x / 41) * 2 - 1;
			return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
		}
		void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
		{
			Out = noise(UV * Scale) + 0.5;
		}
		float noise(float2 p)
		{
			float2 ip = floor(p);
			float2 fp = frac(p);
			float d00 = dot(unity_gradientNoise_dir(ip), fp);
			float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
			float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
			float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
			fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
			return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
		}
		float normalizedNoise(float2 p)
		{
			return noise(p) * 0.5 + 0.5;
		}
		half Smoothstep(half stepValue, half clipStrength, half smoothness)
		{
			float stepEdge = clamp(clipStrength - smoothness, 0, 1); // Smoothness
			half result = smoothstep(stepEdge, clipStrength, stepValue); // Coverage
			return result;
		}
		// ====================================================

		float remap(float min, float max, float newMin, float newMax, float value)
		{
			return newMin + (value - min) * (newMax - newMin) / (max - min);
		}
		float2 createGradient(float2 p)
		{
			float t = (p.y + 1) / 2; // map y-coordinate to range of 0 to 1
			return normalize(lerp(float2(-1, 0), float2(1, 0), t));
		}

		half4 frag(v2f i) : SV_Target
		{
			if(_NoiseBasedClouds == 0)
			{
				half noise = tex2D(_MainTex, i.texcoord).r;
				half4 col = half4(i.col.rgb * (1.0 - (noise-lsky_CloudsCoverage) * 0.5), saturate(  smoothstep(noise,0,lsky_CloudsCoverage) * lsky_CloudsDensity) * i.col.a );
				return saturate(col);
			}

            // --- Create Procedural Noise Texture ---
			// Deform UV
			half distortNoise = normalizedNoise(i.texcoord * _DistortionNoiseScale);
			half2 distortedUV = i.texcoord + distortNoise;
			half2 deform = lerp(i.texcoord, distortedUV, _Distortion);

			// Scale the texture coordinates for noise generation
			half2 scaledTexCoords = deform * _NoiseScale;

			// Generate noise using the custom noise function
			half noiseValue = normalizedNoise(scaledTexCoords);

			// Adjust noise shape: density, coverage, smoothness
			half noiseValuePowered = pow(noiseValue, _Power); // Density
			noiseValue = Smoothstep(noiseValuePowered, _Clipping, _Smoothness); // Smoothness, Clipping
			noiseValue = noiseValue * _Transparency; // Visibility

			// --- Create Horizon Mask ---
			half4 horizonMask = saturate( (i.col.a *0.1) );

			// -- Apply Color and Alpha --
			// Create offseted noise for top color
			half alpha = noiseValue * horizonMask;
			half noiseValueTopMask = Smoothstep(noiseValuePowered, _Clipping + _ColorEdgeHeight, _Smoothness);
			half4 col = half4(lerp(_EdgeColor.rgb, _Color.rgb, noiseValueTopMask), alpha);

			return clamp(col, 0, 1);
		}
		


	ENDCG


		SubShader
		{

			Tags{"Queue"="Background+1100" "RenderType"="Background" "IgnoreProjector"="True"}
			//==============================================================================================

			Pass
			{

				Cull Front
				//ZWrite Off
				//ZTest LEqual
				//Blend One One
				Blend SrcAlpha OneMinusSrcAlpha
				Fog{ Mode Off }
				//==========================================================================================

				CGPROGRAM

					#pragma target   2.0
					#pragma vertex   vert
					#pragma fragment frag

				ENDCG

				//==========================================================================================
			}

			
		}

}
