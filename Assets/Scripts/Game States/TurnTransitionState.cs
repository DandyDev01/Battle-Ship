using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTransitionState : GameState
{
	[SerializeField] private CutScene mCutScene;

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		if (mHasDoneFirstRun == false)
		{
			mCutScene.Play();
			mHasDoneFirstRun = true;
		}

		if (mCutScene.IsCompleted)
		{
			mHasDoneFirstRun = false;
			return mNextState;
		}

		return this;
	}
}
