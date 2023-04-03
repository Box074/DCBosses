Shader "DCQ/CutSpaceEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutX ("Cut X(Y1)", float) = 0.0
        _CutXTo ("Cut X To(Y0)", float) = 0.0
        _CutY (" Cut Y", float) = 0.0
        _CutYTo ("Cut Y To", float) = 0.0
        _LOffset ("Left Offset", float) = 0.0
        _ROffset ("Right Offset", float) = 0.0
        _ClearColor ("Clear Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        GrabPass {}
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 ouv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _GrabTexture;
            //sampler2D _MainTex;
            float _CutX;
            float _CutXTo;

            float _CutY;
            float _CutYTo;


            float _LOffset;
            float _ROffset;

            half4 _ClearColor;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.ouv = v.uv;
                o.uv = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            float getK()
            {
                float s= _CutX - _CutXTo;
                return (_CutY - _CutYTo) / (s == 0 ? 1 : s);
            }
            float getB(float k)
            {
                return _CutY - (k * _CutX);
            }
            float getX(float y,float2 kb)
            {
                float k = kb[0];
                float b = kb[1];
                return (k == 0)? 0 : ((y - b) / k);
            }
            float getY(float x,float2 kb)
            {
                float k = kb[0];
                float b = kb[1];
                return k * x + b;
            }
            float2 getKB()
            {
                float k = getK();
                float b = getB(k);
                return float2(k,b);
            }
            float2 newKB(float2 kb, float2 xy)
            {
                float cx = getX(xy.y, kb);
                float o = cx - xy.x;
                kb[1] += kb[0] * o;
                return kb;
            }

            float2 moveP(float k, float b, float2 xy, float d)
            {
                float r = (d*d) / (k*k + 1);

                float x = sqrt(r) + ((d < 0? -1 : 1) * xy.x);
                float y = k * x + b;
                return d == 0? xy : float2(x, y);
            }
            half4 frag (v2f i) : SV_Target
            {
                float2 kb = getKB();
                float xpos = getX(i.ouv.y, kb);
                kb = newKB(kb, i.ouv);
                float2 uv = i.ouv - moveP(kb[0], kb[1], i.ouv, i.ouv.x > xpos ? _ROffset : _LOffset);
                i.uv -= uv;
                half4 col = tex2D(_GrabTexture, i.uv);
                col = (i.uv.x < 0 || i.uv.y < 0 || i.uv.x > 1 || i.uv.y > 1 || col.a < 0.01) ? _ClearColor : col;
                /*
                clip(i.uv.x);
                clip(i.uv.y);
                clip(i.uv.x > 1 ? -1 : 1);
                clip(i.uv.y > 1 ? -1 : 1);
                clip(col.a - 0.05);*/
                return col;
            }
            ENDCG
        }
    }
}
