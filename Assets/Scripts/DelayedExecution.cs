
using UnityEngine;

using System;
using System.Collections;

public class DelayedExecution : MonoBehaviour
{


	/*
	 * 
	 * 
	 * Implement the function 'Do' below so that it can be called from any context.
	 * You want to pass it a function and a float 'delay'. After 'delay' seconds, the function is to be executed.
	 * You can create as many additional functions as you need.
	 * Assume that this class needs to be a 'MonoBehaviour', so don't change that.
	 * 
	 * 
	 */

	public static DelayedExecution Instance;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void Start()
	{
		DelayedExecution.Do(5f);
	}

	public static void Do(float delay, Action onComplete = null)
	{
		Debug.Log("Do Start");
		Instance.StartCoroutine(Instance.Delay(delay, onComplete));
	}


	IEnumerator Delay(float delay, Action onComplete = null)
    {
		Debug.Log("Delay Start");
		onComplete = onComplete ?? delegate { };
		yield return new WaitForSeconds(delay);
		onComplete();

		Debug.Log("Delay Finished after, " + delay + " sec");
	}

}

