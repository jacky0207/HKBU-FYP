Shader "Custom/Glow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[Toggle] _Outline ("_Outline", float) = 1
		_OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
		_Intensity ("_Intensity", Range(1, 3)) = 1
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		Cull Off
        Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;	           
			float4 _MainTex_TexelSize;
			float _Outline;
			fixed4 _OutlineColor;
			float _Intensity;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Original color
				fixed4 col = tex2D(_MainTex, i.uv);

				if (_Outline == 0)
				{
					return col;
				}
				else
				{
					// Outline color
					fixed4 outlineColor = _OutlineColor;
					outlineColor.a *= col.a;
					outlineColor.rgb *=	outlineColor.a;	
					
					// Set intensity
					outlineColor *= _Intensity;		
					
					// Inner outline
					fixed alpha_up = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a;
					fixed alpha_down = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
					fixed alpha_left = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a;
					fixed alpha_right = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a;

					float4 o = lerp(outlineColor, col, ceil(alpha_up * alpha_down * alpha_left * alpha_right));
					
					return o;
				}
			}
			ENDCG
		}
	}
}
