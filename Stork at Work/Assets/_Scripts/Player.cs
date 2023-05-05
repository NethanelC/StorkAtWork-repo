using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static event Action OnDeath;
    [SerializeField] private PlayerUpgrades _upgrades;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private Camera _camera;
    [SerializeField] private DeliveryProgress _deliveryProgress;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Life _lifeCounterPrefab;
    [SerializeField] private Baby _babyPrefab;
    [SerializeField] private RectTransform _rectTransformForLives;
    [SerializeField] private GameObject _loseCanvas, _winCanvas;
    private Dictionary<int, Life> _lives = new();
    private Transform _transform;
    private float _speed;
    void Awake()
    {
        _transform = transform;
        _speed = 7 * _upgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Speed];
        int maximumLives = 2 + _upgrades.Upgrades[(int)PlayerUpgrades.Upgrade.Lives];
        for (int i = 0; i < maximumLives; ++i) 
        {
            _lives[i] = Instantiate(_lifeCounterPrefab, _rectTransformForLives);
        }
    }
    private void Start()
    {
        _gameInput.OnJumpAction += _gameInput_OnJumpAction;
        DeliveryManager.Instance.OnStartedToDeliver += Instance_OnStartedToDeliver;
        Obstacle.OnObstacled += Obstacle_OnObstacled;
        Finish.OnFinishLineReached += Finish_OnFinishLineReached;
        DeliveryDropper.OnDeliveryLocationReached += DeliveryDropper_OnDeliveryLocationReached;
    }
    private void DeliveryDropper_OnDeliveryLocationReached(int amountOfBabies)
    {
        for (int i = 0; i < amountOfBabies; i++)
        {
            Instantiate(_babyPrefab, transform.position, Quaternion.Euler(0, 0, Range(-180, 180)));
        } 
    }
    private void Finish_OnFinishLineReached()
    {
        FreezeMovement();
        for (int i = 0; i < _lives.Count; ++i)
        {
            _lives[i].gameObject.SetActive(false);
        }
        _winCanvas.SetActive(true);
    }
    private void Obstacle_OnObstacled()
    {
        FreezeMovement();
        _camera.DOShakePosition(2, 3, 10, 90, true, ShakeRandomnessMode.Harmonic).OnComplete(() => 
        { 
            _transform.position = Vector3.zero;
            _spriteRenderer.gameObject.SetActive(true);
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        });
        Destroy(_lives[_lives.Count].gameObject);
        _lives.Remove(_lives.Count);
        if (_lives.Count == 0) 
        {
            _loseCanvas.gameObject.SetActive(true);
            OnDeath?.Invoke();
        }
    }
    private void Instance_OnStartedToDeliver()
    {
        _transform.position = Vector3.zero;
        _rb.AddForce(Vector2.right * _speed, ForceMode2D.Impulse);
        for (int i = 0; i < _lives.Count; ++i)
        {
            _lives[i].gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        _gameInput.OnJumpAction -= _gameInput_OnJumpAction;
    }
    private void OnDestroy()
    {
        DeliveryManager.Instance.OnStartedToDeliver -= Instance_OnStartedToDeliver;
        Obstacle.OnObstacled -= Obstacle_OnObstacled;
        Finish.OnFinishLineReached -= Finish_OnFinishLineReached;
    }
    private void FixedUpdate()
    {
        _transform.position += _speed * Time.fixedDeltaTime * (Vector3)_gameInput.GetMovementVectorNormalized();
    }
    private void _gameInput_OnJumpAction()
    {
        _transform.DOMoveY(_transform.position.y + 3, 0.5f);
    }
    private void FreezeMovement()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _spriteRenderer.gameObject.SetActive(false);
    }
}
