// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Extra/Saturation"
{
  Properties
  {
	_StencilMask("Stencil Mask", Int) = 0
    _MainTex("Texture", 2D) = "white" {}
	_Saturation("Saturation", Range(0, 1)) = 0
  }
  SubShader
  {
	Blend One Zero
    // No culling or depth
    Cull Off ZWrite Off ZTest Always
	Stencil {
		Ref[_StencilMask]
		Comp notequal
		Pass keep
		Fail keep
	}

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
        float4 vertex : SV_POSITION;
      };

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
      }

      sampler2D _MainTex;
      float _Saturation;

      fixed4 frag (v2f i) : SV_Target
      {
		float4 color = tex2D(_MainTex, i.uv);
		float lum = 0.2126*color.r + 0.7152*color.g + 0.0722*color.b;
		float4 gray = float4(lum, lum, lum, color.a);
        return _Saturation * color + (1 - _Saturation) * gray;
      }
      ENDCG
    }
  }
}