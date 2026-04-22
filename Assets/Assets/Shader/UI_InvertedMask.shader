Shader "UI/InvertedMask"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0,0,0,0.8)
        _HoleCenter ("Hole Center", Vector) = (0.5,0.5,0,0)
        _HoleWidth ("Hole Width", Float) = 0.15
        _HoleHeight ("Hole Height", Float) = 0.3
        _Softness ("Edge Softness", Float) = 0.05
        _AspectRatio ("Aspect Ratio (W/H)", Float) = 1.777
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _HoleCenter;
            float _HoleWidth;
            float _HoleHeight;
            float _Softness;
            float _AspectRatio;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Offset UV from hole center
                float2 delta = i.uv - _HoleCenter.xy;

                // Correct X axis for screen aspect ratio so the ellipse
                // isn't distorted by non-square UV space
                delta.x *= _AspectRatio;

                // Ellipse signed distance: normalize each axis by its radius
                float ellipseDist = length(float2(delta.x / _HoleWidth, delta.y / _HoleHeight));

                // 0 = inside hole (transparent), 1 = outside hole (opaque overlay)
                float alpha = smoothstep(1.0, 1.0 + _Softness, ellipseDist);

                fixed4 col = _Color;
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}