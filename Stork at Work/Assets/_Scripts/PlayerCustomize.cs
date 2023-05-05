using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerCustomize : MonoBehaviour
{
    [SerializeField] private PlayerSkins _playerSkins;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteLibrary _spriteLibrary;
    private void Awake()
    {
        ChangeSkin();
        MainMenu.OnButtonChangedSkin += ChangeSkin;
    }
    private void OnDestroy() 
    {
        MainMenu.OnButtonChangedSkin -= ChangeSkin;
    }
    private void ChangeSkin()
    {
        _spriteLibrary.spriteLibraryAsset = _playerSkins.GetSkin(PlayerPrefs.GetInt("Skin"));
        print("adsa");
    }
}
