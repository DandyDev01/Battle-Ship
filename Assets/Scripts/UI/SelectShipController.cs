using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectShipController : MonoBehaviour
{
    [SerializeField] private SelectShipView mSelectShipView;
    [SerializeField] private Ship[] mShips;
    
    public Ship mSelectedShip { get; private set; }

    public Action<Ship> OnShipChange;

	private void Awake()
	{
		SetShip(0);
	}

	// Start is called before the first frame update
	private void Start()
    {
        mSelectShipView.OnSelectionChange += SetShip;    
    }

	private void OnDestroy()
	{
		mSelectShipView.OnSelectionChange -= SetShip;
	}
	
	private void SetShip(int index)
	{
		if (index < 0 || index >= mShips.Length)
			mSelectedShip = null;
		else
			mSelectedShip = mShips[index];

        OnShipChange?.Invoke(mSelectedShip);
	}

	public void TurnOffUI()
	{
		mSelectShipView.gameObject.SetActive(false);
	}

	public void HandleShipPlacement(Ship ship, int totalShips)
	{
		mSelectShipView.DisableButton(ship.ShipSize - 1);
		SetShip(-1);
	}
}
