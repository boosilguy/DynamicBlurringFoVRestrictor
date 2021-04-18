using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicBlurring : MonoBehaviour
{
	public enum BlurringType
	{
		blurring,
		dynamic
	};
	public bool visualize = false;
	public BlurringType blurringType;
	private Material material;
	private Material visualizer;
	[Space(2)]

	[Header("FoV conditions")]
	[Range(0f, 1f)]
	public float FR_center_X = 0.5f;
	[Range(0f, 1f)]
	public float FR_center_Y = 0.5f;
	[Range(0f, 1f)]
	public float ratio110 = 0.45f;
	[Range(0f, 120f)]
	public float eyePositionBoundDegree = 40f;
	private float inRad = 0.1694f;
	public float outRadDeg = 51.4f;
	private float outRad = 0.2103f;
	[Space(2)]
	public int loop_times = 1;
	[Space(2)]
	public float sigma = 4.3f;
	public int dynamicKernelSize = 0;
	public int blurIntensity = 10;
	public float visualJNDPoint = 0.7326771f;
	public float blurUnit = 0.3f;
	public bool softEdge = true;
	[ReadOnly]
	public float targetKernelValue = 0;
	[ReadOnly]
	public float tempKernelValue = 0;

	private float InOutDiff;

	private Camera cam;
	private float VelocityMagnitude;

	void Start()
	{
		//설정한 OutRad로 초기화
		outRad = DegreeToUnityDegree(outRadDeg);
		material = new Material(Shader.Find("Hidden/DynamicBlurring"));
		visualizer = new Material(Shader.Find("Hidden/DynamicBlurringVisualizer"));
		
		
		//변수 설정
		InOutDiff = DegreeToUnityDegree(10f);
		inRad = outRad - InOutDiff;

		//이 script는 camera의 component임, cam을 가져옴
		cam = GetComponent<Camera>();
	}

	void FixedUpdate()
	{
		//메인이 되는 프로세스.
		//머리 움직임과 아바타 움직임, 눈 움직임을 가져와서
		//FOV Restrictor를 조절할 변수들을 조정함.
		//실제로 FOV Restrictor를 조절하는 것은 shader임

		//속도와 회전량
		VelocityMagnitude = cam.velocity.magnitude;

		targetKernelValue = visualJNDPoint * VelocityMagnitude * blurIntensity + 1;

		if (dynamicKernelSize < targetKernelValue)
			tempKernelValue = tempKernelValue + blurUnit;
		else if (dynamicKernelSize > targetKernelValue)
			tempKernelValue = tempKernelValue - blurUnit;

		dynamicKernelSize = Mathf.RoundToInt(tempKernelValue);

		if (dynamicKernelSize % 2 == 0)
			dynamicKernelSize--;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//fixed update에서 변경된 변수들을 shader로 넘겨줌
		//해당 변수들을 실제로 적용하는건 shader

		int softEdgeInteger = softEdge ? 1 : 0;
		
		if (blurringType == BlurringType.dynamic)
			material.SetInt("_KernelSize", dynamicKernelSize);
		else if (blurringType == BlurringType.blurring)
			material.SetInt("_KernelSize", (int)(visualJNDPoint * GameObject.FindObjectOfType<DBPlayerController>().maximumSpeed * blurIntensity + 1));
		
		material.SetInt("_softEdge", softEdgeInteger);
		material.SetFloat("_StandardDeviation", sigma);
		material.SetFloat("_inRad", inRad);
		material.SetFloat("_outRad", outRad);
		material.SetInt("_loop", loop_times);
		material.SetFloat("_eyeXPos", FR_center_X);
		material.SetFloat("_eyeYPos", FR_center_Y);

		var temporaryTexture = RenderTexture.GetTemporary(source.width, source.height);
		Graphics.Blit(source, temporaryTexture, material, 0);

		if (visualize)
		{
			var temporaryTexture2 = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(temporaryTexture, temporaryTexture2, material, 1);

			visualizer.SetInt("_KernelSize", dynamicKernelSize);
			visualizer.SetFloat("_inRad", inRad);
			visualizer.SetFloat("_outRad", outRad);
			Graphics.Blit(temporaryTexture2, destination, visualizer, 0);

			RenderTexture.ReleaseTemporary(temporaryTexture2);

		}
		else
		{
			Graphics.Blit(temporaryTexture, destination, material, 1);
		}
		
		RenderTexture.ReleaseTemporary(temporaryTexture);
		
	}

	private float DegreeToUnityDegree(float degree)
	{
		//degree를 유니티에서 사용할 값으로 변경함
		//해당 값은 oculus rift cv1에서 눈으로 측정되었음
		float UnityDegree = degree * ratio110 / 110f;
		return UnityDegree;
	}


}
