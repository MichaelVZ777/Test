Shader "Unlit/Grid"
{
	Properties
	{
		_Color("Color", color) = (1, 1, 1, 1)
		_LineColor("LineColor", color) = (0, 1, 0, 1)
		_LineThickness("LineThickness", float) = 1
		_CellSize("CellSize", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			half4 _Color;
			half4 _LineColor;
			float _LineThickness;
			float _CellSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 pos = i.worldPos;

				float dpx = length(float2(ddx(pos.x), ddy(pos.x)));
				float dpz = length(float2(ddx(pos.z), ddy(pos.z)));
				pos = abs(pos);
				pos.x += _LineThickness*dpx/2;
				pos.z += _LineThickness*dpz/2;
				pos = fmod(pos, _CellSize);

				fixed4 col;
				if (pos.x < _LineThickness*dpx || pos.z < _LineThickness*dpz)
					col = _LineColor;
				else
					col = _Color; 

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
