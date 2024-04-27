using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSceneSegmentAction : SceneSegmentAction
{
	[SerializeField] private float waitTime = 1;

	public override void Execute()
	{
		IsCompleted = false;
		StartCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(waitTime);

		IsCompleted = true;
	}
}
