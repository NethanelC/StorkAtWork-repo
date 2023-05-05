using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDropper : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    public static event Action<int> OnDeliveryLocationReached;
    private int _deliveryAmount;
    public void Init(int deliveryAmount)
    {
        _deliveryAmount = deliveryAmount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnDeliveryLocationReached?.Invoke(_deliveryAmount);
        print (_deliveryAmount);
        _collider.enabled = false;
    }
    private void Awake()
    {
        Finish.OnFinishLineReached += Finish_OnFinishLineReached;
    }
        private void OnDestroy()
    {
        Finish.OnFinishLineReached -= Finish_OnFinishLineReached;
    }
    private void Finish_OnFinishLineReached()
    {
        Destroy(gameObject);
    }
}
