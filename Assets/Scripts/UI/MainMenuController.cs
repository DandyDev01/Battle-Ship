using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mSelectDifficultyPanel;
    [SerializeField] GameObject mPlayPanel;

    [SerializeField] MainMenuView mMainMenuView;

	private void Awake()
	{
		mSelectDifficultyPanel.SetActive(false);

		mMainMenuView.PlayButton.onClick.AddListener(Play);
		mMainMenuView.EasyModeButton.onClick.AddListener(delegate { SetDifficulty(0); });
		mMainMenuView.MediumModeButton.onClick.AddListener(delegate { SetDifficulty(1); });
		mMainMenuView.HardModeButton.onClick.AddListener(delegate { SetDifficulty(2); });
	}

	private void Play()
	{
		mPlayPanel.SetActive(false);
		mSelectDifficultyPanel.SetActive(true);
	}

	private void SetDifficulty(int index)
	{
		switch (index)
		{
			case 0:
				CIAttackState.difficulty = Difficulty.easy;
				break;
			case 1:
				CIAttackState.difficulty = Difficulty.medium;
				break;
			case 2:
				CIAttackState.difficulty = Difficulty.hard;
				break;
		}

		SceneManager.LoadScene(1);
	}
}
