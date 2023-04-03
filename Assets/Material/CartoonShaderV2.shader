
Shader "DC/CartoonShaderV2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _GlowColor ("Flash Color", Color) = (1,1,1)
        _Border("Border", Vector) = (0,0,0,0)
        _HurtFlash ("Hurt Flash", float) = 0
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend One OneMinusSrcAlpha
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
            float4 _MainTex_TexelSize;
            
            sampler2D _MainTex;
            fixed4 _Color;
            float3 _GlowColor;
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

            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float light = col.a >= 0.5 ? (col.a - 0.5) : col.a;
                light *= 2;
                
                if(light > 0 && col.r == 0 && col.g == 0 && col.b == 0)
                {
                    col.rgb = _GlowColor.rgb * light * 0.5;
                }
                
                col.rgb += light * _GlowColor;
                
                
                col.a = col.a == 0 ? 0 : 1;

                col.rgb += _HurtFlash;
                
                clip(col.a == 0 ? -1 : 1);
                return col;
            }
            ENDCG
        }
    }
}
