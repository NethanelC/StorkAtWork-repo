using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static event Action OnButtonChangedSkin;
    [SerializeField] private PlayerSkins _playerSkins;
    [SerializeField] private Button _previousButton, _nextButton;

    private void Awake()
    {
        PlayerPrefs.SetInt("Skin", 0);
        _previousButton.onClick.AddListener(() => 
        {
            int currentSkin = PlayerPrefs.GetInt("Skin");
            currentSkin = --currentSkin % _playerSkins.AmountOfSkins;
            if (currentSkin < 0)
            {
                currentSkin = _playerSkins.AmountOfSkins - 1;
            }
            PlayerPrefs.SetInt("Skin", currentSkin);
            OnButtonChangedSkin?.Invoke();
        });
        _nextButton.onClick.AddListener(() => 
        {
            int currentSkin = PlayerPrefs.GetInt("Skin");
            PlayerPrefs.SetInt("Skin", ++currentSkin % _playerSkins.AmountOfSkins);
            OnButtonChangedSkin?.Invoke();
        });
    }
}
