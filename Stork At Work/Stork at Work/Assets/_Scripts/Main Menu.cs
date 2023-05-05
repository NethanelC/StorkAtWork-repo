using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    private void Awake()
    {
        _playButton.onClick.AddListener(() => { LevelsManager.LoadALevel("Level"); }) ;
    }
}
