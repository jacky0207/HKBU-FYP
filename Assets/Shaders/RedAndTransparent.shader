Shader "Custom/RedAndTransparent"
{
	Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Process("Process", Range(0., 1.)) = 0
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
            float _Process;
 
            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

				// Turn red
				if (_Process <= 0.25)
				{
					col.rgb = float3(col.r + (1 - col.r) * _Process / 0.5,
										col.g - col.g * _Process / 0.5,
										col.b - col.b * _Process / 0.5);
				}
				// Turn transparent
				else
				{
					col.rgba = float4(1, 0, 0, col.a * (1 - _Process) * 2);
				}

                return col;
            }
            ENDCG
        }
    }
}
