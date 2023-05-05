using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "SOS", menuName = "ScriptableObjects/PlayerSkins", order = 1)]
public class PlayerSkins : ScriptableObject
{
    [SerializeField] private List<SpriteLibraryAsset> Skins = new();
    public SpriteLibraryAsset GetSkin(int skinIndex)
    {
        return Skins[skinIndex];
    }
    public int AmountOfSkins => Skins.Count;
}
