using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    [SerializeField]
    private int _pointsCount;
    [SerializeField]
    private float _maxRadius;
    [SerializeField]
    private float _blastSpeed;
    private LineRenderer _lineRenderer;
    [SerializeField]
    private float _startWidth;


  
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _pointsCount + 1;
    }

    private IEnumerator Blast()
    {
        float _currentRadius = 0f;
        while(_currentRadius < _maxRadius)
        {
            _currentRadius += Time.deltaTime * _blastSpeed;
            Draw(_currentRadius);
            Damage(_currentRadius);
            yield return null;
        }
    }
    private void Draw(float _currentRadius)
    {
        float _angleBetweenPoints = 400/_pointsCount;
        for(int i = 0; i <= _pointsCount; i++)
        {
            float _angle = i * _angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 _direction = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle));
            Vector3 _position = _direction * _currentRadius;
            _lineRenderer.SetPosition(i, _position);
        }
        _lineRenderer.widthMultiplier = Mathf.Lerp(0f, _startWidth, 1f - _currentRadius/_maxRadius);
       
    }

    private void Damage(float _currentRadius)
    {
        Collider2D[] _hittingObjects = Physics2D.OverlapCircleAll(transform.position, _currentRadius);      
        foreach (Collider2D _hitdObject in _hittingObjects)
        {
            GameObject collidedObject = _hitdObject.gameObject;
            Enemy _enemy = collidedObject.GetComponent<Enemy>();
            if (_enemy != null)
            {
                _enemy.BombDestroy();
            }           
        }
    }

    public void ExplodeBomb()
    {   
        StartCoroutine(Blast());
    }

}
