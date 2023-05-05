using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   

public class DeliveryWaypoint : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deliveryNumberText, _deliveryDistanceText;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _waypointImage;
    [SerializeField] private Gradient _gradientByDistance;
    public float DeliveryDistance { get; private set; }
    public int DeliveryAmount { get; private set; }
    public void Init(int deliveryNumber, int deliveryAmount, float deliveryDistance)
    {
        _deliveryDistanceText.text = deliveryNumber.ToString();
        DeliveryDistance = deliveryDistance;
        DeliveryAmount = deliveryAmount;
        _rectTransform.anchoredPosition = new((deliveryDistance * 1100) - 25, -30);
        _waypointImage.color = _gradientByDistance.Evaluate(1);
    }
    public void ChangePositionOnRoute(float furthestDistance)
    {
        float percentOfDistance = DeliveryDistance / furthestDistance;
        _waypointImage.color = _gradientByDistance.Evaluate(percentOfDistance);
        _rectTransform.anchoredPosition = new Vector2((percentOfDistance * 1100) - 25, -30);
    }
}
