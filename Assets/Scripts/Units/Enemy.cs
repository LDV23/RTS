using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyState _currentEnemyState;

    [SerializeField] private int _health;
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;
    private int _maxHealth;

    private Building _targetBuilding, _oldBuilding;
    private Unit _targetUnit;

    [SerializeField] private float _distanceToFollow = 7f, _distanceToAttack = 1f;

    [SerializeField] NavMeshAgent _navMeshAgent;

    [SerializeField] private float _attackPeriod = 1f;
    private float _timer;

    private void Start()
    {
        SetState(EnemyState.WalkToBuilding);

        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    void Update()
    {
        if(_currentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();
            if(_targetBuilding)
            {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosesUnit();
        }
        else if(_currentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosesUnit();

            if(_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);

                float distance = Vector3.Distance(transform.position, _targetBuilding.transform.position);
                if (distance < _distanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }

            if (_targetBuilding == null)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if(_currentEnemyState == EnemyState.WalkToUnit)
        {
            if(_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToFollow)
                {
                    SetState(EnemyState.WalkToBuilding);
                }
                else if (distance < _distanceToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
            
        }
        else if(_currentEnemyState == EnemyState.Attack)
        {
            if(_targetUnit)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);

                float distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                if (distance > _distanceToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    _targetUnit.TakeDamage(1);
                }
            }
            else if(_targetBuilding)
            {
                float distance = Vector3.Distance(transform.position, _targetBuilding.transform.position);
                if(distance < _distanceToAttack)
                {
                    _timer += Time.deltaTime;
                    if (_timer > _attackPeriod)
                    {
                        _timer = 0f;
                        _targetBuilding.TakeDamage(1);
                    }
                }
                else
                {
                    SetState(EnemyState.WalkToBuilding);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBuilding);
            }
            
        }
    }

    public void SetState(EnemyState enemyState)
    {
        _currentEnemyState = enemyState;

        if (_currentEnemyState == EnemyState.Idle)
        {

        }
        else if (_currentEnemyState == EnemyState.WalkToBuilding)
        {
            FindClosestBuilding();
            if(_targetBuilding)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (_currentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (_currentEnemyState == EnemyState.Attack)
        {
            _timer = 0f;
        }
    }

    public void FindClosestBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for(int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);

            if(distance < minDistance)
            {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }

        _targetBuilding = closestBuilding;
    }    
    
    public void FindClosesUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for(int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);

            if(distance < minDistance)
            {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }

        if(minDistance < _distanceToFollow)
        {
            _targetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }  
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _healthBar.SetHealth(_health, _maxHealth);
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Resources resources = FindObjectOfType<Resources>();
        if(resources)
        {
            resources.AddMoney(1);
            resources.UpdateMoney();
        }     
        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);

        }     
    }

}

