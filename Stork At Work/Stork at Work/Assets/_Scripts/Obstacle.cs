using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    public static event Action OnObstacled;
    private void Awake()
    {
        Destroy(gameObject, Range(15, 20));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnObstacled?.Invoke();
    }
}
