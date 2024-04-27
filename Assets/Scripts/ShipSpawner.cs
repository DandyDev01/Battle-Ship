using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] private SampleGridXY mSampleGrid;

	private Vector3 mRotation;
	private Ship mShipPrefab;
	private GameObject mShipPreview;

	public GridXY<cellType> Grid { get; private set; }
	public Ship LastShipSpawned { get; private set; }
	public int ShipsPlaced { get; private set; }

	public Action<Ship, int> OnShipPlacement;

	private void Awake()
	{
		mRotation = Vector3.zero;
	}

	private void Start()
	{
		Grid = mSampleGrid.Grid;
	}

	public void SetShipPrefab(Ship ship)
	{
		mShipPrefab = ship;
	}

	public void CreateShipPreview(Ship ship)
	{
		if (mShipPreview != null)
		{
			Destroy(mShipPreview);
		}

		if (ship == null)
			return;

		mShipPreview = Instantiate(ship, new Vector3(-100, -100, 0),
			Quaternion.Euler(mRotation.x, mRotation.y, mRotation.z)).gameObject;

		SpriteRenderer spriteRenderer = mShipPreview.GetComponentInChildren<SpriteRenderer>();
		Color color = spriteRenderer.color;
		spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
	}

	private void RotateShipPreview()
	{
		if (mShipPreview == null)
			return;

		mShipPreview.transform.rotation = Quaternion.Euler(mRotation);
	}

	public void MoveShipPreview(Vector2 worldPosition)
	{
		if (mShipPreview == null)
			return;

		// start of ship is in range
		if (Grid.IsInRange(worldPosition) == false)
			return;

		Vector2 cell = Grid.GetCellPosition(worldPosition);

		if (cell.x < 0 || cell.y < 0)
			return;

		SpriteRenderer spriteRenderer = mShipPreview.GetComponentInChildren<SpriteRenderer>();

		Vector2 cellCenterWorldPosition = Grid.GetWorldPosition((int)cell.x, (int)cell.y);

		mShipPreview.transform.position = cellCenterWorldPosition;

		// validate cells
		if (IsValid(cell) == false)
		{
			spriteRenderer.color = new Color(255, 0, 0, 0.5f);
			return;
		}
		else
			spriteRenderer.color = new Color(0, 255, 0, 0.5f);


	}

	public void SetRotation(Vector3 rotation)
	{
		mRotation = rotation;
		RotateShipPreview();
	}

	public void SpawnShip(Vector2 position)
    {
		Vector2 cell = Grid.GetCellPosition(position);

		if (cell.x < 0 || cell.y < 0)
			return;

		Vector2 cellCenterWorldPosition = Grid.GetWorldPosition((int)cell.x, (int)cell.y);

		// there is a ship blocking, return
		if (IsValid(cell) == false || mShipPrefab == null)
			return;

		Ship ship = Instantiate(mShipPrefab, cellCenterWorldPosition,
			Quaternion.Euler(mRotation));
		
		// set the grid cells the ship occupies to true
		int col = (int)cell.x;
		int row = (int)cell.y;
		for (int i = 0; i < ship.ShipSize; i++)
		{
			Grid.SetElement(col, row, cellType.Ship);

			if (mRotation.z == 0)
			{
				col += 1;
			}
			else
			{
				row += 1;
			}
		}

		ShipsPlaced += 1;
		OnShipPlacement?.Invoke(mShipPrefab, ShipsPlaced);
		LastShipSpawned = ship;
    }
	
	private bool IsValid(Vector2 cell)
	{
		if (mShipPrefab == null)
			return false;

		int col = (int)cell.x;
		int row = (int)cell.y;
		for (int i = 0; i < mShipPrefab.ShipSize; i++)
		{

			if (Grid.GetElement(col, row) == cellType.Ship)
				return false;

			// check if whole ship is in range
			if (mRotation.z == 0 && cell.x + mShipPrefab.ShipSize-1 > Grid.Columns)
				return false;
			else if (mRotation.z == 90 && cell.y + mShipPrefab.ShipSize-1 > Grid.Rows)
				return false;

			if (mRotation.z == 0)
				col += 1;
			else
				row += 1;
		}

		return true;
	}
}
