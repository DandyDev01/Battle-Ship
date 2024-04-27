using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadUISceneSegmentAction : SceneSegmentAction
{
	//[SerializeField] private UI[] uiToUnload = null;
	//[SerializeField] private GameController gameController = null;

	//private UIController uiController;

	private void Start()
	{
		//gameController = FindObjectOfType<GameController>();
		//uiController = gameController.UIController;
	}

	public override void Execute()
	{
		//IsCompleted = false;

		//foreach (var item in uiToUnload)
		//{
		//	uiController.SetUIActive(item, false);
		//}

		//IsCompleted = true;
	}
}
