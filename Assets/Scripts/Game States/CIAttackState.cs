using Grid;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public enum Difficulty { easy = 0, medium, hard };

public class CIAttackState : GameState
{
	public static Difficulty difficulty;

	[SerializeField] private SampleGridXY mEnemySampleGrid;
	[SerializeField] private GameObject mMissMarker;
	[SerializeField] private GameObject mHitMarker;
	[SerializeField] private GameState mPlayerLosesState;

	private GridXY<cellType> mEnemyGrid;
	private Vector2 mLastCellHit;
	private Vector2 mLastCellAttacked;
	private Vector2 mNextAttackCell;
	private bool mFoundShip;
	private bool mHasMissed;
	private int mShipsSunk;
	private int mCardinalDirectionIndex = 0;
	private int mConcurentHits = 0;
	private Vector2[] mCardinalDirections = new Vector2[]
	{
		new Vector2(0, 1),
		new Vector2(1, 0),
		new Vector2(0, -1),
		new Vector2(-1, 0),
	};

	private void Awake()
	{
		mLastCellHit = Vector2.zero;
		mLastCellAttacked = Vector2.zero;
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
			mHasMissed = false;
		}

		switch (difficulty)
		{
			case Difficulty.easy:
				EasyModeAttack();
				break;
			case Difficulty.medium:
				MediumModeAttack();
				break;
			case Difficulty.hard:
				HardModeAttack();
				break;
		}

		if (mShipsSunk >= 5)
			return mPlayerLosesState;

		if (mHasMissed)
		{
			mHasDoneFirstRun = false;
			return mNextState;
		}

		return this;
	}
	
	private void EasyModeAttack()
	{
		int col = Random.Range(0, mEnemyGrid.Columns);
		int row = Random.Range(0, mEnemyGrid.Rows);

		Vector3 worldPosition = mEnemyGrid.GetWorldPosition(col, row);

		if (mEnemyGrid.GetElement(col, row) == cellType.Ship)
		{
			RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

			if (hit.collider == null)
				Debug.Log("No Collider On: " + hit.transform.name);

			Ship ship = hit.transform.gameObject.GetComponent<Ship>();

			ship.OnDie += HandleShipDie;

			if (ship == null)
			{
				Debug.Log("NULL Ship");
				return;
			}

			int index = GetShipHitIndex(worldPosition,
				ship.transform.rotation.eulerAngles, ship.ShipSize);

			if (index == -2)
			{
				Debug.Log("Outside of grid");
				return;
			}

			ship.DamageShip(index);
			Instantiate(mHitMarker, worldPosition, Quaternion.identity);

			ship.OnDie -= HandleShipDie;
		}
		else if(mEnemyGrid.GetElement(col,row) == cellType.Empty) 
		{
			Instantiate(mMissMarker, worldPosition, Quaternion.identity);
			mEnemyGrid.SetElement(col, row, cellType.Pin);
			Debug.Log("Miss");
			mHasMissed = true;
		}
	}
	
	private void MediumModeAttack()
	{
		bool hasHit = false;
		int col = -1;
		int row = -1;
		Vector3 worldPosition = Vector3.one * -1;

		if (mFoundShip)
		{
			if (mEnemyGrid.GetElement((int)mNextAttackCell.x, (int)mNextAttackCell.y) == cellType.Ship)
			{
				worldPosition = mEnemyGrid.GetWorldPosition((int)mNextAttackCell.x, (int)mNextAttackCell.y);

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

				if (hit.collider == null)
					Debug.Log("No Collider On: " + hit.transform.name);

				Ship ship = hit.transform.gameObject.GetComponent<Ship>();
				//Ship ship = Physics2D.OverlapCircle(worldPosition, 0.7f).GetComponent<Ship>();

				ship.OnDie += HandleShipDie;

				if (ship == null)
				{
					Debug.Log("NULL Ship");
					return;
				}

				int index = GetShipHitIndex(worldPosition,
					ship.transform.rotation.eulerAngles, ship.ShipSize);

				if (index == -2)
				{
					Debug.Log("Outside of grid");
					return;
				}

				ship.DamageShip(index);
				Instantiate(mHitMarker, worldPosition, Quaternion.identity);

				ship.OnDie -= HandleShipDie;

				mConcurentHits += 1;
				mLastCellHit = worldPosition;
				mLastCellAttacked = worldPosition;

				if (ship.ShipCurrentHealth <= 0)
				{
					mConcurentHits = 0;
					mFoundShip = false;
					mCardinalDirectionIndex = 0;
					return;
				}

				mFoundShip = true;
			}
			else
			{
				worldPosition = mEnemyGrid.GetWorldPosition((int)mNextAttackCell.x, (int)mNextAttackCell.y);

				Instantiate(mMissMarker, worldPosition, Quaternion.identity);
				mEnemyGrid.SetElement((int)mNextAttackCell.x, (int)mNextAttackCell.y, cellType.Pin);

				// reset to first hit cell
				mNextAttackCell = new Vector2((int)mNextAttackCell.x, (int)mNextAttackCell.y)
					- (mCardinalDirections[mCardinalDirectionIndex] * mConcurentHits);

				mCardinalDirectionIndex += 1;
				mConcurentHits = 1;
				mHasMissed = true;
			}

			// TODO: handle edge case where ship is along the boarder on any side

			mNextAttackCell = new Vector2((int)mNextAttackCell.x, (int)mNextAttackCell.y)
					+ mCardinalDirections[mCardinalDirectionIndex];

			return;
		}
		else
		{
			col = Random.Range(0, mEnemyGrid.Columns);
			row = Random.Range(0, mEnemyGrid.Rows);

			worldPosition = mEnemyGrid.GetWorldPosition(col, row);

			if (mEnemyGrid.GetElement(col, row) == cellType.Ship)
			{
				hasHit = true;

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

				if (hit.collider == null)
					Debug.Log("No Collider On: " + hit.transform.name);

				Ship ship = hit.transform.gameObject.GetComponent<Ship>();

				ship.OnDie += HandleShipDie;

				if (ship == null)
				{
					Debug.Log("NULL Ship");
					return;
				}

				int index = GetShipHitIndex(worldPosition,
					ship.transform.rotation.eulerAngles, ship.ShipSize);

				if (index == -2)
				{
					Debug.Log("Outside of grid");
					return;
				}

				ship.DamageShip(index);
				Instantiate(mHitMarker, worldPosition, Quaternion.identity);

				ship.OnDie -= HandleShipDie;

				mConcurentHits += 1;
				mLastCellHit = worldPosition;
				mLastCellAttacked = worldPosition;
				mFoundShip = true;

				mNextAttackCell = new Vector2(col, row) + mCardinalDirections[mCardinalDirectionIndex];

				return;
			}
		}

		// did not get a hit with the 5 chances
		if (hasHit == false)
		{
			Instantiate(mMissMarker, worldPosition, Quaternion.identity);
			mEnemyGrid.SetElement(col, row, cellType.Pin);
			Debug.Log("Miss");
			mHasMissed = true;
		}

		mLastCellAttacked = worldPosition;

	}

	private void HardModeAttack()
	{
		bool hasHit = false;
		int col = -1;
		int row = -1;
		Vector3 worldPosition = Vector3.one * -1;

		// has found a ship to attack
		if (mFoundShip)
		{
			if (mEnemyGrid.GetElement((int)mNextAttackCell.x, (int)mNextAttackCell.y) == cellType.Ship)
			{
				worldPosition = mEnemyGrid.GetWorldPosition((int)mNextAttackCell.x, (int)mNextAttackCell.y);

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

				if (hit.collider == null)
					Debug.Log("No Collider On: " + hit.transform.name);

				Ship ship = hit.transform.gameObject.GetComponent<Ship>();
				//Ship ship = Physics2D.OverlapCircle(worldPosition, 0.7f).GetComponent<Ship>();

				ship.OnDie += HandleShipDie;

				if (ship == null)
				{
					Debug.Log("NULL Ship");
					return;
				}

				int index = GetShipHitIndex(worldPosition,
					ship.transform.rotation.eulerAngles, ship.ShipSize);

				if (index == -2)
				{
					Debug.Log("Outside of grid");
					return;
				}

				ship.DamageShip(index);
				Instantiate(mHitMarker, worldPosition, Quaternion.identity);

				ship.OnDie -= HandleShipDie;

				mConcurentHits += 1;
				mLastCellHit = worldPosition;
				mLastCellAttacked = worldPosition;

				if (ship.ShipCurrentHealth <= 0)
				{
					mConcurentHits = 0;
					mFoundShip = false;
					mCardinalDirectionIndex = 0;
					return;
				}

				mFoundShip = true;
			}
			else
			{
				worldPosition = mEnemyGrid.GetWorldPosition((int)mNextAttackCell.x, (int)mNextAttackCell.y);
				
				Instantiate(mMissMarker, worldPosition, Quaternion.identity);
				mEnemyGrid.SetElement((int)mNextAttackCell.x, (int)mNextAttackCell.y, cellType.Pin);

				// reset to first hit cell
				mNextAttackCell = new Vector2((int)mNextAttackCell.x, (int)mNextAttackCell.y)
					- (mCardinalDirections[mCardinalDirectionIndex] * mConcurentHits);

				mCardinalDirectionIndex += 1;
				mConcurentHits = 1;
				mHasMissed = true;
			}

			// TODO: handle edge case where ship is along the boarder on any side

			mNextAttackCell = new Vector2((int)mNextAttackCell.x, (int)mNextAttackCell.y)
					+ mCardinalDirections[mCardinalDirectionIndex];

			return;
		}

		// five chances to get a hit
		for (int i = 0; i < 5; i++)
		{
			col = Random.Range(0, mEnemyGrid.Columns);
			row = Random.Range(0, mEnemyGrid.Rows);

			worldPosition = mEnemyGrid.GetWorldPosition(col, row);

			if (mEnemyGrid.GetElement(col, row) == cellType.Ship)
			{
				hasHit = true;

				RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

				if (hit.collider == null)
					Debug.Log("No Collider On: " + hit.transform.name);

				Ship ship = hit.transform.gameObject.GetComponent<Ship>();

				ship.OnDie += HandleShipDie;

				if (ship == null)
				{
					Debug.Log("NULL Ship");
					return;
				}

				int index = GetShipHitIndex(worldPosition,
					ship.transform.rotation.eulerAngles, ship.ShipSize);

				if (index == -2)
				{
					Debug.Log("Outside of grid");
					return;
				}

				ship.DamageShip(index);
				Instantiate(mHitMarker, worldPosition, Quaternion.identity);

				ship.OnDie -= HandleShipDie;

				mConcurentHits += 1;
				mLastCellHit = worldPosition;
				mLastCellAttacked = worldPosition;
				mFoundShip = true;

				mNextAttackCell = new Vector2(col, row) + mCardinalDirections[mCardinalDirectionIndex];

				return;
			}
		}

		// did not get a hit with the 5 chances
		if (hasHit == false)
		{
			Instantiate(mMissMarker, worldPosition, Quaternion.identity);
			mEnemyGrid.SetElement(col, row, cellType.Pin);
			Debug.Log("Miss");
			mHasMissed = true;
		}

		mLastCellAttacked = worldPosition;
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
			for (int i = 0; i < shipSize + 1; i++)
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
			for (int i = 0; i < shipSize + 1; i++)
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
