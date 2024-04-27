using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private int mShipSize;

    private bool[] mShipHits;

    public int ShipCurrentHealth { get; private set; }
    public int ShipSize => mShipSize;

    public Action<Ship> OnDie;

	private void Awake()
	{
        mShipHits = new bool[mShipSize];

        ShipCurrentHealth = mShipSize;
	}

    public void DamageShip(int index)
    {
        if (index >= mShipSize || index < 0)
            return;

        if (mShipHits[index])
            return;

        ShipCurrentHealth -= 1;
        mShipHits[index] = true;

        if (ShipCurrentHealth <= 0)
            OnDie?.Invoke(this);
    }
}
