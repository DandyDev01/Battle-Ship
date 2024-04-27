using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerPlaceShipsState : GameState
{
	private bool mAllShipsPlaced;

	private void Awake()
	{
		mAllShipsPlaced = false;
	}

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		playerInputHandler.HandleShipPlaceState();

		if (mAllShipsPlaced)
			return mNextState;

		return this;
	}

	public void AllShipsPlaced()
	{
		mAllShipsPlaced = true;
	}
}