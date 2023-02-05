
Shader "DC/CartoonShader"
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
        _FlashColor ("Flash Color", Color) = (1,1,1)
        _FlashColorOrig ("Flash Color Origin", Color) = (1,1,1)
        _WorldSpaceLightPos ("World Space Light Pos", Vector) = (0,0,0,0)
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
                fixed s = col.r == _FlashColorOrig.r ? col.b == _FlashColorOrig.b ? col.g == _FlashColorOrig.g ? 0 : 1 : 1 : 1;
                fixed4 col2 = s == 0 ? fixed4(_FlashColor, 1) : col;
                col2.a = s == 0 ? 1 - col.g : col2.a;
                col2 = col2 * _Color;
                clip(col2.a - 0.01);
                clip(col.a - 0.01);
                fixed3 dcol = _NoUseBump ? col2 : diffuse(i, col2, facing);
                dcol.rgb += _HurtFlash;
                dcol = clamp(dcol, 0, 1);
                return fixed4(dcol, col2.a);
            }
            ENDCG
        }
    }
}
