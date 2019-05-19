Shader "Custom/CameraPixelation"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelNumber ("Pixel number of axis x an y", float) = 2000
		_Brightness ("Brightness", Range(0., 1.)) = 1
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
			// sampler2D _DissolveTex;
			float _PixelNumber;
			float _Brightness;
			
			fixed4 frag (v2f_img i) : SV_Target
			{
				half2 uv = half2((int)(i.uv.x * _PixelNumber) / _PixelNumber, (int)(i.uv.y * _PixelNumber) / _PixelNumber);	// n pixel use 1 uv position			
                fixed4 col = tex2D(_MainTex, uv);
				col.rgb = abs(col.rgb);
				col.rgb *= _Brightness;	// brightness

                return col;
			}
			ENDCG
		}
	}
}
