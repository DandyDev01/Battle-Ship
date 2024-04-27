using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private Camera mCamera;

	public Vector3 Rotation {  get; private set; }
    public Vector2 MouseClickPosition { get; private set; }
	public Vector2 MousePosition { get; private set; }

	public Action<Vector3> OnRotateClick;
	public Action<Vector2> OnMouseClick;

	private void Awake()
	{
        mCamera = Camera.main;
		Rotation = Vector3.zero;
	}

	public void HanedlePlayerAttackLocationState()
	{
		if (Input.GetMouseButtonDown(0))
		{
			MouseClickPosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
			OnMouseClick?.Invoke(MouseClickPosition);
		}

		MousePosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
	}

	public void HandleShipPlaceState()
	{
		if (Input.GetMouseButtonDown(0))
		{
			MouseClickPosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
			OnMouseClick?.Invoke(MouseClickPosition);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			float z = Rotation.z;

			z = z == 0 ? 90 : 0;

			Rotation = new Vector3(0, 0, z);
			OnRotateClick?.Invoke(Rotation);
		}

		MousePosition = mCamera.ScreenToWorldPoint(Input.mousePosition);
	}
}
