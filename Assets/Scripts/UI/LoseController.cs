using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{
	[SerializeField] private WinView mWinView;

	// Start is called before the first frame update
	void Start()
	{
		mWinView.OnPlayAgain += HandlePlayAgain;
		mWinView.OnQuit += HandleQuit;
	}

	void OnDestroy()
	{
		mWinView.OnPlayAgain += HandlePlayAgain;
		mWinView.OnQuit += HandleQuit;
	}

	private void HandleQuit()
	{
		Application.Quit();
	}

	private void HandlePlayAgain()
	{
		SceneManager.LoadScene(0);
	}
}
