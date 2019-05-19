Shader "Custom/PatrolBlind"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_lightColor ("Light Color", Color) = (1,1,1,1)
		_bgRatio ("Background Ratio", Range(0., 1.)) = 0.5
	}
	SubShader
	{
        // Tags { "Queue" = "Transparent" }
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

		GrabPass
        {
            "_BackgroundTexture"
        }

		Pass
		{
			// CGPROGRAM
            // #pragma vertex vert_img
            // #pragma fragment frag
			
			// #include "UnityCG.cginc"

			// sampler2D _MainTex;
			// float4 _RGB;
			 
            // fixed4 frag (v2f_img i) : SV_Target
            // {
            //     fixed4 col = tex2D(_MainTex, i.uv);
			// 	col *= _RGB;

            //     return col;
            // }
            // ENDCG

			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
            };

			sampler2D _MainTex;
			float4 _lightColor;
			float _bgRatio;
        	float4 _MainTex_ST;

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                // use ComputeGrabScreenPos function from UnityCG.cginc
                // to get the correct texture coordinate
                o.grabPos = ComputeGrabScreenPos(o.pos);
            	o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
                return o;
            }

            sampler2D _BackgroundTexture;

            half4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);	// Area
                half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);	// background color
                return col * (bgcolor * (1 - _bgRatio) + _lightColor * _bgRatio);
            }
            ENDCG
		}
	}
}
