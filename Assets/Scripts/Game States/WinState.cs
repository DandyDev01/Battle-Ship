using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : GameState
{
	[SerializeField] GameObject mWinUI;

	private void Awake()
	{
		mWinUI.SetActive(false);
	}

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		if (mHasDoneFirstRun == false)
		{
			mHasDoneFirstRun = true;
			mWinUI.SetActive(true);
		}

		return this;
	}
}
