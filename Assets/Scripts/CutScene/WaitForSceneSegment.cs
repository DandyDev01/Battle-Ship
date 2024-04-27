using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSceneSegment : CustomYieldInstruction
{
	public override bool keepWaiting 
	{
		get
		{
			return !segment.IsCompleted;
		}
	}

	private SceneSegment segment;

	public WaitForSceneSegment(SceneSegment _segment)
	{
		segment = _segment;
	}
}
