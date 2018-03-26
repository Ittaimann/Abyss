Shader "A, putting a means its the top/ghost"
{
	Properties
	{
		 [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
		[NoScaleOffset]_fadeTex("gradient for fade",2D)="white"{}
		_fade("fade amount", Range(0.0,1.01))=0.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
			ZTest off
		Cull Off

		Blend SrcAlpha OneMinusSrcAlpha
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

			sampler2D _MainTex,_fadeTex;
			float4 _MainTex_ST;
			float _fade;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float val = tex2D(_fadeTex, i.uv).r;

				col.a*=step(_fade,val);
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
