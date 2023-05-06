using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryProgress : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _currrentDistanceText, _furthestDistanceText;
    [SerializeField] private DeliveryDropper _deliveryDropper;
    [SerializeField] private DeliveryWaypoint _waypointPrefab;
    [SerializeField] private Dictionary<int, DeliveryWaypoint> _waypointsAdded = new();
    private float _furthestDistance = 1;
    private void Start()
    {
        DeliveryManager.Instance.OnStartedToDeliver += Instance_OnStartedToDeliver;
        DeliveryButton.OnDeliveryButtonStatusChanged += DeliveryButton_OnDeliveryTaken;
        Finish.OnFinishLineReached += Finish_OnFinishLineReached;
    }
    private void Finish_OnFinishLineReached()
    {
        foreach(var waypoint in _waypointsAdded)
        {
            Destroy(waypoint.Value.gameObject);
        }
        _waypointsAdded.Clear();
    }
    private void OnDisable()
    {
        DeliveryManager.Instance.OnStartedToDeliver -= Instance_OnStartedToDeliver;
        DeliveryButton.OnDeliveryButtonStatusChanged -= DeliveryButton_OnDeliveryTaken;
    }
    private void Update()
    {
        _progressSlider.value = _playerTransform.position.x / _furthestDistance;
    }
    private void Instance_OnStartedToDeliver()
    {
        _furthestDistanceText.text = $"{_furthestDistance}M";
        _progressSlider.gameObject.SetActive(true);
        foreach (var waypoint in _waypointsAdded)
        {
            Instantiate(_deliveryDropper, new Vector2(waypoint.Value.DeliveryDistance, -6.5f), Quaternion.identity).Init(waypoint.Value.DeliveryAmount);
        }
    }
    private void DeliveryButton_OnDeliveryTaken(int deliveryNumber, int deliveryAmount, float deliveryDistance, DeliveryManager.DeliveryType deliveryType)
    {
        if (_waypointsAdded.ContainsKey(deliveryNumber))
        {
            Destroy(_waypointsAdded[deliveryNumber].gameObject);
            _waypointsAdded.Remove(deliveryNumber);
            float furthestDistanceHolder = 0;
            foreach (var waypoint in _waypointsAdded)
            {
                if (waypoint.Value.DeliveryDistance > furthestDistanceHolder)
                {
                    furthestDistanceHolder = waypoint.Value.DeliveryDistance;
                }
            }
            _furthestDistance = furthestDistanceHolder;
        }
        else
        {
            if (deliveryDistance > _furthestDistance)
            {
                _furthestDistance = deliveryDistance;
            }
            DeliveryWaypoint newWaypoint = Instantiate(_waypointPrefab, _progressSlider.fillRect);
            newWaypoint.Init(deliveryNumber, deliveryAmount, deliveryDistance);
            _waypointsAdded.Add(deliveryNumber, newWaypoint);
        }
        foreach (var waypoint in _waypointsAdded)
        {
            waypoint.Value.ChangePositionOnRoute(_furthestDistance);
        }
    }
}
