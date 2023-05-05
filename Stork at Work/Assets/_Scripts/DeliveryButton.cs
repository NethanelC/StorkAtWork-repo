using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeliveryButton : MonoBehaviour
{
    public static event Action<int, int, float, DeliveryManager.DeliveryType> OnDeliveryButtonStatusChanged;
    private static event Action OnClickedUpdateVisuals;
    [SerializeField] private Button _buttonAcceptDelivery;
    [SerializeField] private TextMeshProUGUI _deliveryNumberText, _deliveryDistanceText, _amountToDeliverText;
    [SerializeField] private Image _deliveryTypeImage, _isSelectedDeliveryImage;
    [SerializeField] private Sprite[] _deliveryTypeSprites = new Sprite[2]; //switch to scriptableobject
    [Header("Selected Colors")]
    [SerializeField] private ColorBlock _selectedColorBlock;
    private bool _isThisDeliveryInProgress;
    private int _deliveryNumber, _amountToDeliver;
    private float _deliveryDistance;
    private DeliveryManager.DeliveryType _deliveryType;
    private ColorBlock _normalColorBlock;
    public void Init(int deliveryNumber, int amountToDeliver ,float deliveryDistance, DeliveryManager.DeliveryType deliveryType)
    {
        _deliveryNumber = deliveryNumber;
        _amountToDeliver = amountToDeliver;
        _deliveryDistance = deliveryDistance;
        _deliveryType = deliveryType;
        _deliveryNumberText.text = $"#{deliveryNumber}";
        _deliveryDistanceText.text = $"{deliveryDistance:F2}M";
        _amountToDeliverText.text = $"x{amountToDeliver}";
        _deliveryTypeImage.sprite = _deliveryTypeSprites[(byte)deliveryType];
        _buttonAcceptDelivery.interactable = !DeliveryManager.Instance.MaxDeliveriesReached;
    }
    private void Awake()
    {
        _normalColorBlock = _buttonAcceptDelivery.colors;
        OnClickedUpdateVisuals += ToggleButtonUsable;
        _buttonAcceptDelivery.onClick.AddListener(() => 
        { 
            _isThisDeliveryInProgress = !_isThisDeliveryInProgress;
            EventSystem.current.SetSelectedGameObject(null);
            OnDeliveryButtonStatusChanged?.Invoke(_deliveryNumber, _amountToDeliver, _deliveryDistance, _deliveryType);
            OnClickedUpdateVisuals?.Invoke();
        });
    }
    private void OnDestroy()
    {
        OnClickedUpdateVisuals -= ToggleButtonUsable;    
    }
    private void ToggleButtonUsable()
    {
        if (_isThisDeliveryInProgress)
        {
            _buttonAcceptDelivery.interactable = true;
            _buttonAcceptDelivery.colors = _selectedColorBlock;
            return;
        }
        _buttonAcceptDelivery.colors = _normalColorBlock;
        _buttonAcceptDelivery.interactable = !DeliveryManager.Instance.MaxDeliveriesReached;
    }
}
