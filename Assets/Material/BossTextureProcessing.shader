
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.wNormal = UnityObjectToWorldNormal(v.normal);
				o.wTangent = UnityObjectToWorldNormal(v.tangent);
				o.wBitangent = cross(-o.wTangent, o.wNormal) * v.tangent.w;
                
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

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed3 dcol = _NoUseBump ? col : diffuse(i, col, facing);
                if(col.a < 0.5) {
                    dcol.rb = 0;
                    dcol.g = 1;
                } 
                else
                {
                    dcol.g = clamp(dcol.g, 0, 0.99);
                }
                return fixed4(dcol, 1);
            }
            ENDCG
        }
    }
}
