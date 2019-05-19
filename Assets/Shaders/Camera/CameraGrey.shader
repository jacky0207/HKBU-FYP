Shader "Custom/CameraGrey"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius("Radius", Range(0., 1.)) = 0
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
            float _Radius;
 
            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if (sqrt((i.uv.x - 0.5) * (i.uv.x - 0.5) + (i.uv.y - 0.5) * (i.uv.y - 0.5)) < _Radius)
                {
                    col.rgb = dot(abs(0.15 - col.rgb), float3(0.5,0.5,0.5));
                }
                else
                {
                    col.rgb = abs(col.rgb);
                }

                return col;
            }
            ENDCG
        }
    }
}
