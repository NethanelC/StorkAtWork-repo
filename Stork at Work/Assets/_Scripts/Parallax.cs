using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    private Vector2 _startPosition;
    private float _startZ;
    private float _distanceFromPlayer => transform.position.z - _player.position.z;
    private float _clippingPlane => (_camera.transform.position.z + (_distanceFromPlayer > 0 ? _camera.farClipPlane : _camera.nearClipPlane));
    private float _parallaxFacter => Mathf.Abs(_distanceFromPlayer) / _clippingPlane;
    private Vector2 _travel => (Vector2)_camera.transform.position - _startPosition;
    private void Start() 
    {
        _startPosition = transform.position;
        _startZ = transform.position.z;
    }
    private void Update() 
    {
        Vector2 newPosition = _startPosition + _travel * _parallaxFacter;
        transform.position = new Vector3(newPosition.x, newPosition.y, _startZ);
    }
}
