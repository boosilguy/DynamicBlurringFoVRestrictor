using System.Collections;
using System.Collections.Generic;

public class WalkingInPlace {
	
	public float sMin = 0.02f;				// 최소 Top peak - Bottom peak
	public float sMax = 0.10f;				// 최대 Top peak - Bottom peak
	public float velocityMin = 1.0f;		// 최저 속도
	public float velocityMax = 3.6f;		// 최대 속도
	public float iMin = 0.25f;				// 최소 Step interval (sec)
	public float iMax = 0.50f;				// 최소 Step interval (sec)

	public int listSize = 3;				// Eye level list에 들어갈 Frame 별 eye level
	public int queueSize = 5;				// Queue에 담길 Eye level list의 평균값 갯수
	public List<float> eyeLevelList = new List<float>();
	public Queue<float> queue = new Queue<float>();

	public WalkingInPlace ()
	{
		// 난 기본 생성자!
	}
	
	public WalkingInPlace (int listSize, int queueSize)
	{
		this.listSize = listSize;
		this.queueSize = queueSize;

		eyeLevelList = new List<float> (listSize);
		queue = new Queue<float> (queueSize);
	}

	///	<summary>
	/// Walking in place 인스턴스의 Queue에 있는 중앙값이 가장 작다면 반환하고,
	/// 그렇지 않으면 0을 반환한다.
	/// </summary>
	/// <param name="queue">WalkingInPlace의 인스턴스가 갖는 Queue</param>
	public float GetBottomPeak (Queue<float> queue)
	{
		Queue<float> q = new Queue<float>(queue);
		float size = (float)q.Count;
		int midIdx = (int)(size/2 + 0.5f);
		float smallest = 1000f;

		for (int idx = 0; idx < size; idx++)
		{
			float dequeue = q.Dequeue();
			// smallest를 찾기 위해, 교환을 진행할 때
			if (smallest > dequeue)
			{
				smallest = dequeue;

				// queue의 가운데 index가 넘어가면 Walking in place가 아님
				if (idx + 1 > midIdx)
					return 0;
			}
			else if (smallest <= dequeue)
			{
				if (idx + 1 == midIdx)
					return 0;
			}
		}
		return smallest;
	}

	///	<summary>
	/// Walking in place 인스턴스의 Queue에 있는 중앙값이 가장 크다면 반환하고,
	/// 그렇지 않으면 0을 반환한다.
	/// </summary>
	/// <param name="queue">WalkingInPlace의 인스턴스가 갖는 Queue</param>
	public float GetTopPeak (Queue<float> queue)
	{
		Queue<float> q = new Queue<float>(queue);
		float size = (float)q.Count;
		int midIdx = (int)(size/2 + 0.5f);
		float largest = 0;

		for (int idx = 0; idx < size; idx++)
		{
			float dequeue = q.Dequeue();
			// largest를 찾기 위해, 교환을 진행할 때
			if (largest < dequeue)
			{
				largest = dequeue;

				// queue의 가운데 index가 넘어가면 Walking in place가 아님
				if (idx + 1 > midIdx)
					return 0;
			}
			else if (largest >= dequeue)
			{
				if (idx + 1 == midIdx)
					return 0;
			}
		}
		
		return largest;
	}

	public float GetAverageEyeLevel ()
	{
		float result = 0;
		foreach (float f in eyeLevelList)
		{
			result += f;
		}

		return result / listSize;
	}

	public void ReleaseListComponent ()
	{
		eyeLevelList.RemoveAt(0);
	}

	public void ReleaseQueueComponent ()
	{
		queue.Dequeue();
	}

}
