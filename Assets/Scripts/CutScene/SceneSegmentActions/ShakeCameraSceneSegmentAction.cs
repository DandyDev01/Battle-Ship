using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraSceneSegmentAction : SceneSegmentAction
{
	[SerializeField] float shakeTime = 1;
	[SerializeField] float shakePower = 1;

	public override void Execute()
	{
		//IsCompleted = false;
		//CameraShake.instance.StartShake(shakeTime, shakePower);

		//IsCompleted = true;
	}
}
