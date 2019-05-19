Shader "Custom/CameraToBlack"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Percentage ("Percentage ", Range(0., 1.)) = 0
		_StartTransition ("Start transition", int) = 1
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _Percentage;
			float _StartTransition;
			
			fixed4 frag (v2f_img i) : SV_Target
			{
                fixed4 col = tex2D(_MainTex, i.uv);

				if (_StartTransition == 0)	// To black
				{
					if (i.uv.x < _Percentage)
					{
						col.rgb = dot(abs(0.15 - col.rgb), float3(0,0,0));
					}
					else
					{
						col.rgb = abs(col.rgb);
					}
				}
				else
				{
					if (i.uv.x > _Percentage)
					{
						col.rgb = dot(abs(0.15 - col.rgb), float3(0,0,0));
					}
					else
					{
						col.rgb = abs(col.rgb);
					}
				}
	
                return col;
			}
			ENDCG
		}
	}
}
