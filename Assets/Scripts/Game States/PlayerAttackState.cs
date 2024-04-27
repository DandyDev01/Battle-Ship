using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : GameState
{
	[SerializeField] private SampleGridXY mEnemySampleGrid;
	[SerializeField] private GameObject mMarker;
	[SerializeField] private GameObject mMissMarker;
	[SerializeField] private GameObject mHitMarker;
	[SerializeField] private GameState mPlayerWinState;

	private GridXY<cellType> mEnemyGrid;
	private bool mHasMissed;
	private int mShipsSunk;

	public Action OnAllShipsDestroyed;

	private void Awake()
	{
		mHasMissed = false;
	}

	private void Start()
	{
		mEnemyGrid = mEnemySampleGrid.Grid;
	}

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		if (mHasDoneFirstRun == false)
		{
			mHasDoneFirstRun = true;
			playerInputHandler.OnMouseClick += HandleClick;
			mMarker = Instantiate(mMarker);
			mMarker.tag = "Untagged";
		}

		playerInputHandler.HanedlePlayerAttackLocationState();
		Vector3 temp = mEnemyGrid.GetCellPosition(playerInputHandler.MousePosition);
		mMarker.transform.position = mEnemyGrid.GetWorldPosition((int)temp.x, (int)temp.y);

		if (mShipsSunk >= 5)
		{
			OnAllShipsDestroyed?.Invoke();
			return mPlayerWinState;
		}

		if (mHasMissed)
		{
			mHasDoneFirstRun = false;
			mHasMissed = false;
			return mNextState;
		}

		return this;
	}

	private void HandleClick(Vector2 worldPosition)
	{
		Vector2 cell = mEnemyGrid.GetCellPosition(worldPosition);
		
		// click was out of bounds
		if (cell.x == -1 || cell.y == -1)
			return;
		
		Vector3 cellCenterWorldPosition = mEnemyGrid.GetWorldPosition((int)cell.x, (int)cell.y);

		if (mEnemyGrid.GetElement((int)cell.x, (int)cell.y) == cellType.Ship)
		{
			RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

			if (hit.collider == null)
				Debug.Log("No Collider On: " + hit.transform.name);

			Ship ship = hit.transform.gameObject.GetComponent<Ship>();


			if (ship == null)
			{
				Debug.Log("NULL Ship");
				return;
			}

			ship.OnDie += HandleShipDie;

			int index = GetShipHitIndex(worldPosition, 
				ship.transform.rotation.eulerAngles, ship.ShipSize);

			if (index == -2)
			{
				Debug.Log("Outside of grid");
				return;
			}

			ship.DamageShip(index);
			Instantiate(mHitMarker, cellCenterWorldPosition, Quaternion.identity);

			ship.OnDie -= HandleShipDie;
		}
		else if (mEnemyGrid.GetElement((int)cell.x, (int)cell.y) == cellType.Empty)
		{
			Instantiate(mMissMarker, cellCenterWorldPosition, Quaternion.identity);
			mEnemyGrid.SetElement((int)cell.x, (int)cell.y, cellType.Pin);
			Debug.Log("Miss");
			mHasMissed = true;
		}
	}

	private void HandleShipDie(Ship ship)
	{
		SpriteRenderer spriteRenderer = ship.GetComponentInChildren<SpriteRenderer>();
		Color color = spriteRenderer.color;
		spriteRenderer.color = new Color(color.r, color.g, color.b, 1);

		mShipsSunk += 1;
	}

	private int GetShipHitIndex(Vector2 worldPosition, Vector3 shipRotation, int shipSize)
	{
		if (mEnemyGrid.IsInRange(worldPosition) == false)
			return -2;

		Vector2 cellHit = mEnemyGrid.GetCellPosition(worldPosition);

		int col = (int)cellHit.x;
		int row = (int)cellHit.y;
		if (shipRotation.z == 0)
		{
			// go left until grid is false
			for (int i = 0; i < shipSize+1; i++)
			{
				if (mEnemyGrid.GetElement(col, row) == cellType.Empty)
				{
					// one cell right is the start of the ship
					int startCellX = col + 1;
					return (int)cellHit.x - startCellX;
				}

				col -= 1;
			}
		}
		else if (shipRotation.z == 90)
		{
			// go down until grid is false
			for (int i = 0; i < shipSize+1; i++)
			{
				if (mEnemyGrid.GetElement(col, row) == cellType.Empty)
				{
					// one cell up is the start of the ship
					int startCellY = row + 1;
					return (int)cellHit.y - startCellY;
				}

				row -= 1;
			}
		}

		return -1;
	}
}
