using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentPacifiersText;
    private void Awake()
    {
        UpdatePacifiersText();
        UpgradeButton.OnUpgradePurchased += UpdatePacifiersText;
    }
    private void OnDisable()
    {
        UpgradeButton.OnUpgradePurchased -= UpdatePacifiersText;
    }
    private void UpdatePacifiersText()
    {
        _currentPacifiersText.text = PlayerPrefs.GetInt("Pacifiers", 0).ToString();
    }
}
