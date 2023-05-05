using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.Random;
using Cysharp.Threading.Tasks;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    public event Action OnStartedToDeliver;
    [SerializeField] private PlayerUpgrades _playerUpgrades;
    [SerializeField] private RectTransform _deliveryMenu, _routeTransform;
    [SerializeField] private DeliveryButton _deliveryButtonPrefab;
    [SerializeField] private DeliveryNotify _deliveryNotifyPrefab;
    [SerializeField] private DeliveryWaypoint _waypointPrefab;
    [SerializeField] private Finish _finishPrefab;
    [SerializeField] private TextMeshProUGUI _deliveriesTakenText;
    [SerializeField] private Dictionary<int, DeliveryWaypoint> _waypointsAdded = new();
    [SerializeField] private Button _deliverButton;
    private int _deliveriesOrderSoFar;
    private float _furthestDistance;
    private const float _distanceMultiplier = 100;
    public bool MaxDeliveriesReached => _playerUpgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Maximum_Deliveries] == _waypointsAdded.Count;
    private int _randomDistanceByLevel => _deliveriesOrderSoFar * (int)Range(_distanceMultiplier - (_playerUpgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Distance] * 20),  _distanceMultiplier + 20);
    private DeliveryType _randomTypeByLevel => (DeliveryType)Range(0, 2);
    public enum DeliveryType
    {
        City,
        Village
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        _deliverButton.onClick.AddListener(() => 
        {
            OnStartedToDeliver?.Invoke();
            Instantiate(_finishPrefab, Vector2.right * _furthestDistance, Quaternion.identity);
            gameObject.SetActive(false); 
        });
        _deliverButton.gameObject.SetActive(false);
        _deliveriesTakenText.text = $"0 / {_playerUpgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Maximum_Deliveries]}";
        
        DeliveryButton.OnDeliveryButtonStatusChanged += DeliveryButton_OnDeliveryButtonStatusChanged;
        DeliveryDropper.OnDeliveryLocationReached += DeliveryDropper_OnDeliveryLocationReached;
    }
    private void Start() 
    {
       SpawnDelivery(); 
    }
    private void OnDestroy()
    {
        DeliveryButton.OnDeliveryButtonStatusChanged -= DeliveryButton_OnDeliveryButtonStatusChanged;
        DeliveryDropper.OnDeliveryLocationReached -= DeliveryDropper_OnDeliveryLocationReached;
    }
    private void DeliveryDropper_OnDeliveryLocationReached(int pacifiers)
    {
        _playerUpgrades.CollectPacifiers(pacifiers);
    }
    private void DeliveryButton_OnDeliveryButtonStatusChanged(int deliveryNumber, int deliveryAmount, float deliveryDistance, DeliveryType deliveryType)
    {
        //If clicked on a selected delivery, remove it
        if (_waypointsAdded.ContainsKey(deliveryNumber))
        {
            Destroy(_waypointsAdded[deliveryNumber].gameObject);
            _waypointsAdded.Remove(deliveryNumber);
            float furthestDistanceHolder = 0;
            foreach(var waypoint in _waypointsAdded)
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
            DeliveryWaypoint newWaypoint = Instantiate(_waypointPrefab, _routeTransform);
            newWaypoint.Init(deliveryNumber, deliveryAmount, deliveryDistance);
            _waypointsAdded.Add(deliveryNumber, newWaypoint);
        }
        foreach (var waypoint in _waypointsAdded)
        {
            waypoint.Value.ChangePositionOnRoute(_furthestDistance);
        }
        _deliverButton.gameObject.SetActive(_waypointsAdded.Count > 0);
        _deliveriesTakenText.text = $"{_waypointsAdded.Count} / {_playerUpgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Maximum_Deliveries]}";
    }
    private async void SpawnDelivery()
    {
        _deliveriesOrderSoFar++;
        Instantiate(_deliveryButtonPrefab, _deliveryMenu).Init(_deliveriesOrderSoFar, Range(1, _playerUpgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Amount_Of_Babies] + 1), _randomDistanceByLevel, _randomTypeByLevel);
        Instantiate(_deliveryNotifyPrefab);
        await UniTask.Delay(_deliveriesOrderSoFar * 5000).ContinueWith(SpawnDelivery);
    }
}
