using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeUnityEventSceneSegmentAction : SceneSegmentAction
{
	public UnityEvent action;

	public override void Execute()
	{
		action?.Invoke();
	}
}
