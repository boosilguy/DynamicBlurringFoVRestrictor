Shader "Hidden/DynamicBlurring"
{
	Properties{
		[HideInInspector]_MainTex ("Texture", 2D) = "white" {}
		_KernelSize("Kernel Size", Int) = 11
		_StandardDeviation("Standard Deviation", Float) = 5.0

		_inRad("Inner Radius", Float) = 0
		_outRad("Outer Radius", Float) = 0
		_loop("Loop Times", Int) = 1
		_eyeXPos("Eye X Position", Float) = 0.5
		_eyeYPos("Eye Y Position", Float) = 0.5
		_softEdge("Soft Edge", Int) = 1
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"

	#define PI 3.14159265359
	#define E 2.71828182846

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float2 _MainTex_TexelSize;
	int _KernelSize;
	float _StandardDeviation;
	
	uniform float _inRad;
	uniform float _outRad;
	uniform float _eyeXPos;	
	uniform float _eyeYPos;
	uniform int _loop;
	uniform int _softEdge;

	float Gaussian(int x)
	{
		float sigmaSqu = _StandardDeviation * _StandardDeviation;
		return (1 / sqrt(2 * PI * sigmaSqu)) * pow(E, -(x * x)/(2 * sigmaSqu));
	}
	ENDCG

	SubShader
	{
		Cull Off
		ZWrite Off 
		ZTest Always

		Pass
		{
			Name "HorizontalPass"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_horizontal

			//vertex shader에 첨부될 데이터
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			//fragment shader 데이터 base
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//vertex shader 데이터 base
			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			//the fragment shader
			fixed4 frag_horizontal(v2f i) : SV_TARGET
			{
				// Eye-tracking variables & FoV variables
				float opacity = 1;
				float base_opacity = 1;
				float X = _eyeXPos;
				float Y = _eyeYPos;
				float L = sqrt((i.uv.x - X)*(i.uv.x - X) + (i.uv.y - (1-Y))*(i.uv.y - (1-Y)));
				float I = _inRad;
				float O = _outRad;

				float4 colTemp = float4(0.0, 0.0, 0.0, 1.0);
				float4 col = tex2D(_MainTex, i.uv);
				float KernelSum = 0.0;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int x = lower; x <= upper; ++x)
				{
					float gauss = Gaussian(x);
					KernelSum += gauss;
					colTemp += gauss * tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * x, 0.0));
				}

				colTemp /= KernelSum;
				
				//필요한 방법으로 화면을 조정
				//아래는 opacity를 조정하여 비율로 검은색(_Color)을 덧칠함.
				//FoV restrictor에 가우시안 필터 적용
				if (_softEdge == 1)
				{	
					if (L < I)
						opacity = 0;
					else if (I < L && L < O)
					{
						base_opacity = (L - I) / (O - I);
						for (int num = 0; num < _loop; num = num + 1)
						{
							opacity = opacity * base_opacity;
						}
					}
					else
						opacity = 1;
					col = col * (1 - opacity) + colTemp * (opacity);

					return col;
				}
				else
				{
					if (L < I)
						opacity = 0;
					else
						opacity = 1;
					
					col = col * (1 - opacity) + colTemp * (opacity);
					return col;
				}
			}
			ENDCG
		}

		Pass
		{
			Name "VerticalPass"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_vertical

			//vertex shader에 첨부될 데이터
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			//fragment shader 데이터 base
			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//vertex shader 데이터 base
			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			//the fragment shader
			fixed4 frag_vertical(v2f i) : SV_TARGET
			{
				// Eye-tracking variables & FoV variables
				float opacity = 1;
				float base_opacity = 1;
				float X = _eyeXPos;
				float Y = _eyeYPos;
				float L = sqrt((i.uv.x - X)*(i.uv.x - X) + (i.uv.y - (1-Y))*(i.uv.y - (1-Y)));
				float I = _inRad;
				float O = _outRad;

				float4 colTemp = float4(0.0, 0.0, 0.0, 1.0);
				float4 col = tex2D(_MainTex, i.uv);
				float KernelSum = 0.0;

				int upper = ((_KernelSize - 1) / 2);
				int lower = -upper;

				for (int y = lower; y <= upper; ++y)
				{
					float gauss = Gaussian(y);
					KernelSum += gauss;
					colTemp += gauss * tex2D(_MainTex, i.uv + fixed2(0.0, _MainTex_TexelSize.y * y));
				}

				colTemp /= KernelSum;
				
				//필요한 방법으로 화면을 조정
				//아래는 opacity를 조정하여 비율로 검은색(_Color)을 덧칠함.
				//FoV restrictor에 가우시안 필터 적용
				if (_softEdge == 1)
				{	
					if (L < I)
						opacity = 0;
					else if (I < L && L < O)
					{
						base_opacity = (L - I) / (O - I);
						for (int num = 0; num < _loop; num = num + 1)
						{
							opacity = opacity * base_opacity;
						}
					}
					else
						opacity = 1;
					col = col * (1 - opacity) + colTemp * (opacity);

					return col;
				}
				else
				{
					if (L < I)
						opacity = 0;
					else
						opacity = 1;
					
					col = col * (1 - opacity) + colTemp * (opacity);
					return col;
				}
			}
			ENDCG
		}
	}
}