using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Archer : Unit
{
    [SerializeField] private UnitState _currentUnitState;

    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private Enemy _targetEnemy;

    [SerializeField] private float _distanceToFollow = 7f, _distanceToAttack = 5f;



    [SerializeField] private float _attackPeriod = 2f;
    private float _timer;

    public override void Start()
    {
        base.Start();

        SetState(UnitState.WalkToPoint);
    }

    void Update()
    {
        if (_currentUnitState == UnitState.Idle)
        {
            FindTarget();
        }
        else if (_currentUnitState == UnitState.WalkToPoint)
        {
            FindTarget();
        }
        else if (_currentUnitState == UnitState.WalkToEnemy)
        {
            if (_targetEnemy)
            {
                NavMeshAgent.SetDestination(_targetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                if (distance > _distanceToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }
                else if (distance < _distanceToAttack)
                {
                    SetState(UnitState.Attack);
                    NavMeshAgent.ResetPath();
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }

        }
        else if (_currentUnitState == UnitState.Attack)
        {
            if (_targetEnemy)
            {
                float distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                if (distance > _distanceToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }

                _timer += Time.deltaTime;
                if (_timer > _attackPeriod)
                {
                    _timer = 0f;
                    _targetEnemy.TakeDamage(1);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }

        }
    }

    public void SetState(UnitState unitState)
    {
        _currentUnitState = unitState;

        if (_currentUnitState == UnitState.Idle)
        {

        }
        else if (_currentUnitState == UnitState.WalkToPoint)
        {

        }
        else if (_currentUnitState == UnitState.WalkToEnemy)
        {

        }
        else if (_currentUnitState == UnitState.Attack)
        {

        }
    }

    void FindTarget()
    {
        if (FindClosesEnemy() < _distanceToFollow)
        {
            _targetEnemy = ClosesEnemy;
            SetState(UnitState.WalkToEnemy);
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
}
