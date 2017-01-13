// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Hidden/DebugDraw"
{
	Properties
	{
		_Color ("Color", Color) = (0,0,0,0)
		_Alpha("Alpha", Range(0,1)) = 0.5
		_RimStrength("Rim Strength", Range(0,1))=0.5
		_VertexOffset("Vertex Offset",float) = -0.001
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One OneMinusSrcAlpha
			//Blend DstColor Zero
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normal:TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _RimStrength;
			float _Alpha;
			float _VertexOffset;
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				//offset the vertex inside a bit to avoid ZFighting
				float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				float2 offset = TransformViewToProjection(norm.xy); 
				o.vertex.xy += offset * o.vertex.z * _VertexOffset;


				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				// compute world space position of the vertex
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                // compute world space view direction
                o.viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color;
				col.a *= _Alpha;
				half rim = 1.0 - saturate(dot (normalize(i.viewDir), i.normal));
				col+=rim*_RimStrength;
				return col;
			}
			ENDCG
		}
	}
}
