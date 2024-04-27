using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSegmentAction : CustomYieldInstruction
{
	public override bool keepWaiting
	{
		get
		{
			return !action.IsCompleted;
		}
	}

	private SceneSegmentAction action;

	public WaitForSegmentAction(SceneSegmentAction _action)
	{
		action = _action;
	}
}
