Shader "Custom/Invert"
{
    SubShader
    {
        Tags { "Queue"="Transparent" }
 
        Pass
        {
            ZWrite On
            ColorMask 0
        }
        Blend OneMinusDstColor OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct vertexInput
            {
                float4 vertex: POSITION;
                float4 color : COLOR;
            };

            struct fragmentInput
            {
                float4 pos : SV_POSITION;
            };

            fragmentInput vert(vertexInput i)
            {
                fragmentInput o;
                o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
                return o;
            }

            half4 frag(fragmentInput i) : COLOR
            {
                return (1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
