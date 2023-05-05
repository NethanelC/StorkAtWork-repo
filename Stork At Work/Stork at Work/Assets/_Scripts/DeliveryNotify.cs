using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class DeliveryNotify : MonoBehaviour
{
    [SerializeField] private Image _image;
    //[SerializeField] private AudioClip _clip;
    [SerializeField] private TextMeshProUGUI _text;
    private void Awake()
    {
        //AudioSource.PlayClipAtPoint(_clip, Vector3.zero);
        AnimateFadeIn();
    }
    private async void AnimateFadeIn()
    {
        var task = _image.rectTransform.DOAnchorPosY(0, 1.5f).AsyncWaitForCompletion();
        await task;
        await UniTask.Delay(5000);
        _image.DOFade(0, 3);
        _text.DOFade(0, 3);
        await UniTask.Delay(3000);
        DOTween.KillAll();
        Destroy(gameObject);
    }
}
