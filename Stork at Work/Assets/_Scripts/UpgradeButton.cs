using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [Header("Select Upgrade")]
    [SerializeField] private PlayerUpgrades.Upgrade _upgrade;
    [Header("References")]
    [SerializeField] private PlayerUpgrades _playerUpgrades;
    [SerializeField] private TextMeshProUGUI _upgradeCost, _upgradename;
    [SerializeField] private Image _upgradeImage;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Sprite[] _statusImagesSprites = new Sprite[2];
    [SerializeField] private Image[] _statusImages = new Image [5];
    public static event Action OnUpgradePurchased;
    private int _currentUpgradePrice;
    private string _upgradeName => _upgrade.ToString();
    private int _currentUpgradedTimes => PlayerPrefs.GetInt(_upgradeName);
    private int _currentPacifiers => PlayerPrefs.GetInt("Pacifiers", 0);
    private void Awake()
    {
        UpdateVisuals();
        _upgradename.text = _upgrade.ToString().Replace("_", " ");
        OnUpgradePurchased += UpdateVisuals;
        for (int i = 0; i < _statusImages.Length; ++i) 
        {
            _statusImages[i].sprite = _statusImagesSprites[i < _currentUpgradedTimes ? 1 : 0];
        }
        _upgradeButton.onClick.AddListener(() => 
        {
            PlayerPrefs.SetInt(_upgradeName, _currentUpgradedTimes + 1);
            PlayerPrefs.SetInt("Pacifiers", _currentPacifiers - _currentUpgradePrice);
            _statusImages[_currentUpgradedTimes - 1].sprite = _statusImagesSprites[1];
            OnUpgradePurchased?.Invoke();
        });
    }
    private void OnDisable()
    {
        OnUpgradePurchased -= UpdateVisuals;
    }
    private void UpdateVisuals()
    {
        _currentUpgradePrice = _currentUpgradedTimes * 2;
        _upgradeButton.interactable = _currentPacifiers >= _currentUpgradePrice;
        _upgradeCost.text = _currentUpgradePrice.ToString();
    }
}
