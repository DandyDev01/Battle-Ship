using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	private Camera mCamera;

	private void Awake()
	{
		mCamera = Camera.main;
	}
}