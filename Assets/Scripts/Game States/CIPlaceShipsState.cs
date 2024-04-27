using Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CIPlaceShipsState : GameState
{
	[SerializeField] private ShipSpawner mShipSpawner;
	[SerializeField] private Ship[] mShips;
	[SerializeField] private SampleGridXY mSampleGrid;

	private Grid<cellType> mGrid;
	private int mShipsPlaced;

	public List<Ship> Ships { get; private set; }

	private void Awake()
	{
		Ships = new List<Ship>();
	}

	private void Start()
	{
		mShipsPlaced = 0;

		mGrid = mSampleGrid.Grid;

		mShipSpawner.SetShipPrefab(mShips[mShipsPlaced]);
		mShipSpawner.OnShipPlacement += ShipPlaced;
	}

	private void OnDestroy()
	{
		mShipSpawner.OnShipPlacement -= ShipPlaced;
	}

	public override GameState RunState(PlayerInputHandler playerInputHandler)
	{
		while(mShipsPlaced < 5)
		{
			// choose random place on the grid
			int col = UnityEngine.Random.Range(0, mSampleGrid.Columns);
			int row = UnityEngine.Random.Range(0, mSampleGrid.Rows);

			Vector2 worldPosition = mGrid.GetWorldPosition(col, row);

			float z = UnityEngine.Random.Range(0, 10) > 5 ? 90 : 0;

			mShipSpawner.SetRotation(new Vector3(0, 0, z));

			mShipSpawner.SpawnShip(worldPosition);
			Ships.Add(mShipSpawner.LastShipSpawned);
		}

		foreach (Ship ship in Ships)
		{
			SpriteRenderer spriteRenderer = ship.GetComponentInChildren<SpriteRenderer>();
			Color color = spriteRenderer.color;
			spriteRenderer.color = new Color(color.r, color.g, color.b, 0);
		}

		return mNextState;
	}

	private void ShipPlaced(Ship ship, int arg2)
	{
		mShipsPlaced += 1;

		if (mShipsPlaced >= 5)
			return;

		mShipSpawner.SetShipPrefab(mShips[mShipsPlaced]);
	}
}
