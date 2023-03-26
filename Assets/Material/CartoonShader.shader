
Shader "DC/CartoonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1)
        _FlashColorOrig ("Flash Color Origin", Color) = (1,1,1)
        _Border("Border", Vector) = (0,0,0,0)
        _HurtFlash ("Hurt Flash", float) = 0
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

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
            float2 _BumpOffset;
            float4 _MainTex_TexelSize;
            
            sampler2D _MainTex;
            fixed4 _Color;
            fixed3 _FlashColor;
            fixed3 _FlashColorOrig;
            float2 _Border;
            float _HurtFlash;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.xy += _Border.xy;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                fixed3 col = tex2D(_MainTex, i.uv);
                clip(col.g == 1 ? -1 : 1);
                if(col.r == _FlashColorOrig.r && col.g == _FlashColorOrig.g && col.b == _FlashColor.b)
                {
                    col = _FlashColor;
                }

                col.rgb += _HurtFlash;
                col = clamp(col, 0, 1);
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
