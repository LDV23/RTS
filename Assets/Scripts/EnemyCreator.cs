using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private float _creationPeriod;

    [SerializeField] private GameObject _enemyPrefab;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _creationPeriod)
        {
            _timer = 0f;
            Instantiate(_enemyPrefab, transform.position, transform.rotation);
        }
    }
}

