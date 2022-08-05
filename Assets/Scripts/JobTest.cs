using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


/*
 *
 * JobTest scene runs very slow because of the repeated dummy math operation below. Implement the for loop below, using parallelized Unity jobs and Burst compiler to gain performance
 * If the 'count' is too large for your machine to handle, you can decrease it.
 * 
 */

public class JobTest : MonoBehaviour
{

	[SerializeField]
	private bool useJob = false;


	private int count = 1000000;

	private float[] values;


	void Start()
	{
		values = new float[count];
	}


	void Update()
	{

		if (useJob)
		{
			// Job here

			var tempValues = new NativeArray<float>(count, Allocator.Persistent);
			for (var i = 0; i < tempValues.Length; i++)
				tempValues[i] = values[i];

			// Initialize the job data
			var job = new UpdateParallelJob()
			{
				valueArray = tempValues
			};

			JobHandle jobHandle = job.Schedule(count, 64);
			jobHandle.Complete();

			tempValues.Dispose();
		}
		else
		{

			for (int i = 0; i < values.Length; i++)
			{
				values[i] = Mathf.Sqrt(Mathf.Pow(values[i] + 1.75f, 2.5f + i)) * 5 + 2f;
			}

		}

	}

	[BurstCompile]
	private struct UpdateParallelJob : IJobParallelFor
	{
		public NativeArray<float> valueArray;

		public void Execute(int i)
		{
			valueArray[i] = Mathf.Sqrt(Mathf.Pow(valueArray[i] + 1.75f, 2.5f + i)) * 5 + 2f;
		}
	}
}