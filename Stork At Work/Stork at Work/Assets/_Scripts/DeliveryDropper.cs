using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDropper : MonoBehaviour
{
    public static event Action<int> OnDeliveryLocationReached;
    private int _deliveryAmount;
    public void Init(int deliveryAmount)
    {
        _deliveryAmount = deliveryAmount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            OnDeliveryLocationReached?.Invoke(_deliveryAmount);
        }    
        Destroy(gameObject, 3);
    }
    private void Awake()
    {
        Finish.OnFinishLineReached += Finish_OnFinishLineReached;
    }
    private void Finish_OnFinishLineReached()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        Finish.OnFinishLineReached -= Finish_OnFinishLineReached;
    }
}
