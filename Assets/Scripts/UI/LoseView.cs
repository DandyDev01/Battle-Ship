using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseView : MonoBehaviour
{
	[SerializeField] private Button mPlayAgainButton;
	[SerializeField] private Button mQuitButton;

	public Action OnPlayAgain;
	public Action OnQuit;

	private void Awake()
	{
		mPlayAgainButton.onClick.AddListener(PlayAgain);
		mQuitButton.onClick.AddListener(Quit);
	}

	private void Quit()
	{
		OnQuit?.Invoke();
	}

	private void PlayAgain()
	{
		OnPlayAgain?.Invoke();
	}
}
