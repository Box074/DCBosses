
Shader "DC/BossTextureProcessing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [Range(0,1)] _MaxDark ("Max Dark", float) = 1
        [NoScaleOffset] _BumpMap ("Bump Tex", 2D) = "bump" {}
        [Toggle] _FlipBumpMap ("Flip Bump Map", float) = 0
        _LightColor ("Light Color", Color) = (1,1,1)
        [Toggle] _NoUseBump ("No Use Bump", float) = 0
        _WorldSpaceLightPos ("World Space Light Pos", Vector) = (0,0,0,0)


        [Toggle] _UseGlowColor ("Use Glow Color", float) = 1

        _GlowHSVStart ("Glow HSV Start", Float) = 290
        _GlowHSVEnd ("Glow HSV End", Float) = 300
        _GlowColor ("Glow Color", Color) = (0,0,0)
        _GlowOffset ("Glow Offset", Vector) = (0,0,0,0)
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
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 wNormal : TEXCOORD1;
				float3 wTangent : TEXCOORD2;
				float3 wBitangent : TEXCOORD3;

                float4 uv01 : TEXCOORD4;  
                float4 uv23 : TEXCOORD5;
                float4 uv45 : TEXCOORD6;
            };
            float2 _BumpOffset;
            float4 _MainTex_TexelSize;
            
            sampler2D _MainTex;
            sampler2D _BumpMap;
            float4 _BumpMap_TexelSize;
            fixed _NoUseBump;
            fixed _FlipBumpMap;
            fixed4 _Color;
            fixed3 _LightColor;
            float _MaxDark;
            float3 _WorldSpaceLightPos;

            fixed _UseGlowColor;

            float3 _GlowColor;
            float4 _GlowOffset;

            float _GlowHSVStart;
            float _GlowHSVEnd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.wNormal = UnityObjectToWorldNormal(v.normal);
				o.wTangent = UnityObjectToWorldNormal(v.tangent);
				o.wBitangent = cross(-o.wTangent, o.wNormal) * v.tangent.w;

                _GlowOffset *= _MainTex_TexelSize.xyxy;  

                o.uv01 = v.uv.xyxy + _GlowOffset.xyxy * float4(1, 1, -1, -1);  
                o.uv23 = v.uv.xyxy + _GlowOffset.xyxy * float4(1, 1, -1, -1) * 2.0;  
                o.uv45 = v.uv.xyxy + _GlowOffset.xyxy * float4(1, 1, -1, -1) * 3.0;  
                
                return o;
            }
            

            fixed3 diffuse(v2f i, fixed3 col, fixed facing)
            {
                i.uv.x = _FlipBumpMap ? 1 - i.uv.x : i.uv.x;
                float3 normalTex = normalize(tex2D(_BumpMap, i.uv) * 2 - 1);
                //normalTex.z *= facing;
				float3 N = normalize(i.wTangent) * normalTex.r + normalize(i.wBitangent) * normalTex.g + normalize(i.wNormal) * normalTex.b;
                
                half3 toonLight = saturate(dot(N, _WorldSpaceLightPos)) > 0.3 ? _LightColor : unity_AmbientSky;
                return col * max(toonLight, (1 - _MaxDark));
            }

            float3 rgb2hsv(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            fixed getcolor(float4 orig)
            {
                float3 hsv = rgb2hsv(orig.rgb);
                if(hsv.r >= _GlowHSVStart && hsv.r <= _GlowHSVEnd && orig.a > 0.5)
                {
                    return 1;
                }
                return 0;
            }

            float computeGlow(v2f i)
            {
                float result = 0;
                result += 0.4 * getcolor(tex2D(_MainTex, i.uv));  
                result += 0.15 * getcolor(tex2D(_MainTex, i.uv01.xy));  
                result += 0.15 * getcolor(tex2D(_MainTex, i.uv01.zw));  
                result += 0.10 * getcolor(tex2D(_MainTex, i.uv23.xy));  
                result += 0.10 * getcolor(tex2D(_MainTex, i.uv23.zw));  
                result += 0.05 * getcolor(tex2D(_MainTex, i.uv45.xy));  
                result += 0.05 * getcolor(tex2D(_MainTex, i.uv45.zw));
                return result;
            }

            float4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                float3 dcol = _NoUseBump ? col : diffuse(i, col, facing);
                float glow = computeGlow(i);
                if(_UseGlowColor < 0.5)
                {
                    glow = 0;
                }
                else
                {
                    if(getcolor(col) == 1)
                    {
                       dcol.rgb = 0;
                    }
                }
                
                col.a = col.a > 0.5 ? (0.5 + glow / 2) : clamp(glow / 2, 0, 0.49);
                clip(col.a - 0.001);
                return float4(dcol, col.a);
            }
            ENDCG
        }
    }
}
