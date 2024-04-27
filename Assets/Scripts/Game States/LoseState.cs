using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : GameState
{
	[SerializeField] GameObject mLoseUI;

	private void Awake()
	{
		mLoseUI.SetActive(false);
	}

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		if (mHasDoneFirstRun == false)
		{
			mHasDoneFirstRun = true;
			mLoseUI.SetActive(true);
		}

		return this;
	}
}
