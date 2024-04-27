using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectShipView : MonoBehaviour
{
    [SerializeField] private Button mShipOne;
    [SerializeField] private Button mShipTwo;
    [SerializeField] private Button mShipThree;
    [SerializeField] private Button mShipFour;
    [SerializeField] private Button mShipFive;

    public Action<int> OnSelectionChange;

	private void Awake()
	{
        mShipOne.onClick.AddListener(delegate { SetSelectedShip(0); }); ;
        mShipTwo.onClick.AddListener(delegate { SetSelectedShip(1); }); ;
        mShipThree.onClick.AddListener(delegate { SetSelectedShip(2); }); ;
        mShipFour.onClick.AddListener(delegate { SetSelectedShip(3); }); ;
        mShipFive.onClick.AddListener(delegate { SetSelectedShip(4); }); ;
	}


    private void SetSelectedShip(int index)
	{
        OnSelectionChange?.Invoke(index);
	}

    public void DisableButton(int index)
    {
        switch(index)
        {
            case 0:
                mShipOne.interactable = false;
                break;
            case 1:
                mShipTwo.interactable = false;
                break;
            case 2:
                mShipThree.interactable = false;
                break;
            case 3:
                mShipFour.interactable = false;
                break;
            case 4:
                mShipFive.interactable = false;
                break;
        }
    }
}
