using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCameraSceneSegmentAction : SceneSegmentAction
{
	[SerializeField] private Transform CenterOfIntrestTransform = null;
	[SerializeField] private Camera cameraToMove = null;
	[SerializeField] private float actionDuration = 1;
	[SerializeField] private bool enableCameraFollowOnEnd = false;

	private CameraFollow cameraFollow = null;

	public override void Execute()
	{
		cameraFollow = cameraToMove.GetComponent<CameraFollow>();
		cameraFollow.enabled = false;
		IsCompleted = false;

		StartCoroutine(Wait());
		cameraToMove.transform.DOMoveX(CenterOfIntrestTransform.position.x, actionDuration);
		cameraToMove.transform.DOMoveY(CenterOfIntrestTransform.position.y, actionDuration);
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(actionDuration);
		if (enableCameraFollowOnEnd)
			cameraFollow.enabled = true;

		IsCompleted = true;
	}
}
