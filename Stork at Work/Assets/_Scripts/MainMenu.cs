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
        SetFirstPlayerPrefs();
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
    private void SetFirstPlayerPrefs()
    {
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            PlayerPrefs.SetInt("Skin", 0);
            PlayerPrefs.SetInt("Pacifiers", 0);
            PlayerPrefs.SetInt("Amount_Of_Babies", 1);
            PlayerPrefs.SetInt("Distance", 1);
            PlayerPrefs.SetInt("Maximum_Deliveries", 1);
            PlayerPrefs.SetInt("Speed", 1);
            PlayerPrefs.SetInt("Lives", 1);
            PlayerPrefs.SetInt("FirstTime", 1);
        }
    }
}
