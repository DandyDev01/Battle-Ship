using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private SelectShipController mSelectShipController;
    [SerializeField] private ShipSpawner mEnemyShipSpawner;

    [Header("States")]
    [SerializeField] private GameState mPlayerPlaceShipsState;
    [SerializeField] private GameState mCIPlaceShipsState;
    [SerializeField] private GameState mPlayerAttackState;
    [SerializeField] private GameState mCIAttackState;
    [SerializeField] private GameState mCurrentState;

    private PlayerInputHandler mPlayerInputHandler;
    private ShipSpawner mPlayerShipSpawner;

	private void Awake()
	{
        mCurrentState = mPlayerPlaceShipsState;
	}

	// Start is called before the first frame update
	private void Start()
    {
        mPlayerInputHandler = GetComponent<PlayerInputHandler>();
        mPlayerShipSpawner = GetComponent<ShipSpawner>();

        mSelectShipController.OnShipChange += mPlayerShipSpawner.SetShipPrefab;
        mSelectShipController.OnShipChange += mPlayerShipSpawner.CreateShipPreview;

        mPlayerInputHandler.OnMouseClick += mPlayerShipSpawner.SpawnShip;
        mPlayerInputHandler.OnRotateClick += mPlayerShipSpawner.SetRotation;

        mPlayerShipSpawner.OnShipPlacement += mSelectShipController.HandleShipPlacement;
        mPlayerShipSpawner.OnShipPlacement += CheckForAllShips;
    }

	private void CheckForAllShips(Ship ship, int totalShips)
	{
        if (totalShips < 5)
            return;

        // all ships are placed, move onto next stage
        mSelectShipController.TurnOffUI();

        var playerPlaceShipsState = mPlayerPlaceShipsState as PlayerPlaceShipsState;
		playerPlaceShipsState.AllShipsPlaced();
	}

	// Update is called once per frame
	private void Update()
    {
        mCurrentState = mCurrentState.RunState(mPlayerInputHandler);

        mPlayerShipSpawner.MoveShipPreview(mPlayerInputHandler.MousePosition);
    }

	private void OnDestroy()
	{
		mPlayerInputHandler.OnMouseClick -= mPlayerShipSpawner.SpawnShip;
        mSelectShipController.OnShipChange -= mPlayerShipSpawner.SetShipPrefab;
        mPlayerInputHandler.OnRotateClick -= mPlayerShipSpawner.SetRotation;
        mPlayerShipSpawner.OnShipPlacement -= mSelectShipController.HandleShipPlacement;
	}
}
