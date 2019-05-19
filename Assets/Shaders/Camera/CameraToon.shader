Shader "Custom/CameraToon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Partition("Partition", Range(4, 12)) = 12
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Partition;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				// Start toon color												// Assume partition = 4
				float colorSplitter = _Partition == 12 ? 0 : 1 / _Partition;	// Checking color range e.g 0-0.25, 0.25-0.5, 0.5-0.75, 0.75-1
				float colorRatio = _Partition == 12 ? 0 : 1 / (_Partition - 1);	// Toon color e.g 0, 0.33, 0.66, 1
																				// Loop
																				// if (col.x <= 0.25) col.x = 0.00->0.00;
																				// else if (col.x <= 0.50) col.x = 0.25->0.33;
																				// else if (col.x <= 0.75) col.x = 0.50->0.66;
																				// else if (col.x <= 1.00) col.x = 0.75->1.00
																	
				// col.x
				for (int partitionIndex = 0; partitionIndex < _Partition; partitionIndex++)
				{
					// Color in range
					if (col.x <= colorSplitter * (partitionIndex + 1))
					{
						// Toon color
						col.x = colorRatio * partitionIndex;

						// Finish toon color
						break;
					}
				}

				// col.y
				for (int partitionIndex = 0; partitionIndex < _Partition; partitionIndex++)
				{
					// Color in range
					if (col.y <= colorSplitter * (partitionIndex + 1))
					{
						// Toon color
						col.y = colorRatio * partitionIndex;

						// Finish toon color
						break;
					}
				}

				// col.x
				for (int partitionIndex = 0; partitionIndex < _Partition; partitionIndex++)
				{
					// Color in range
					if (col.z <= colorSplitter * (partitionIndex + 1))
					{
						// Toon color
						col.z = colorRatio * partitionIndex;

						// Finish toon color
						break;
					}
				}

				return col;
			}
			ENDCG
		}
	}
}
