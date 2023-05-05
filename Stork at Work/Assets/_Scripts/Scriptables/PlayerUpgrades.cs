using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOS", menuName = "ScriptableObjects/PlayerUpgrades", order = 1)]
public class PlayerUpgrades : ScriptableObject
{
    public int CurrentPacifiers { get; private set; }
    public int[] Upgrades = new int[] { 1, 1, 1, 1, 1 };
    public readonly Dictionary<Upgrade, int> MaximumOfUpgrade = new()
    {
        { Upgrade.Amount_Of_Babies, 5 },
        { Upgrade.Distance, 5 },
        { Upgrade.Maximum_Deliveries, 5 },
        { Upgrade.Speed, 5 },
        { Upgrade.Lives, 5 }
    };
    public enum Upgrade
    {
        Amount_Of_Babies,
        Distance,
        Maximum_Deliveries,
        Speed,
        Lives
    }
    [SerializeField] private Sprite[] UpgradeImages = new Sprite[4];
    public Sprite GetFittingSprite(Upgrade upgrade)
    {
        return UpgradeImages[(int)upgrade];
    }
    public void IncrementAnUpgrade(Upgrade upgrade)
    {
        Upgrades[(int)upgrade]++;
    }
    public void CollectPacifiers(int amountCollected)
    {
        CurrentPacifiers += amountCollected;
    }
    public void RemovePacifiers(int amountRemoved)
    {
        CurrentPacifiers -= amountRemoved;
    }
}
