using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
	[SerializeField] protected GameState mNextState;

	protected bool mHasDoneFirstRun;

	public abstract GameState RunState(PlayerInputHandler playerInputHandler);

	public void SetNextState(GameState nextState)
	{
		mNextState = nextState;
	}
}