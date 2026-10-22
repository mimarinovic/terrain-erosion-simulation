Shader "Custom/TerrainSurface"
{
    Properties
    {
        _GrassColor ("Grass Color", Color) = (0,1,0,1)
        _DirtColor ("Dirt Color", Color) = (0.5,0.25,0,1)
        _RockColor ("Rock Color", Color) = (0.5,0.5,0.5,1)
        _SnowColor ("Snow Color", Color) = (1,1,1,1)
        _GrassSlope ("Grass Slope Threshold", Range(0,1)) = 0.3
        _RockSlope ("Rock Slope Threshold", Range(0,1)) = 0.6
        _SnowHeight ("Snow Height Threshold", Float) = 20
        _SlopeBlend ("Slope Blend", Range(0,0.5)) = 0.1
        _HeightBlend ("Height Blend", Range(0,10)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        fixed4 _GrassColor, _DirtColor, _RockColor, _SnowColor;
        float _GrassSlope, _RockSlope, _SnowHeight, _SlopeBlend, _HeightBlend;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float height = IN.worldPos.y;
            float slope = 1.0 - IN.worldNormal.y;

            float snowMask = smoothstep(_SnowHeight - _HeightBlend, _SnowHeight + _HeightBlend, height);
            float rockMask = smoothstep(_RockSlope - _SlopeBlend, _RockSlope + _SlopeBlend, slope);
            float dirtMask = smoothstep(_GrassSlope - _SlopeBlend, _GrassSlope + _SlopeBlend, slope) * (1.0 - rockMask);
            float grassMask = 1.0 - dirtMask - rockMask - snowMask;

            fixed4 c = _GrassColor * grassMask + _DirtColor * dirtMask + _RockColor * rockMask + _SnowColor * snowMask;
            o.Albedo = c.rgb;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
