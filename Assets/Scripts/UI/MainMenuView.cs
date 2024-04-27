using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button mPlayButton;
    [SerializeField] private Button mEasyModeButton;
    [SerializeField] private Button mMediumModeButton;
    [SerializeField] private Button mHardModeButton;

    public Button PlayButton => mPlayButton;
    public Button EasyModeButton => mEasyModeButton;
    public Button MediumModeButton => mMediumModeButton;
    public Button HardModeButton => mHardModeButton;
}
