using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    public int Price = 2;

    [SerializeField] private int _health;

    public NavMeshAgent NavMeshAgent;

    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;

    private int _maxHealth;

    public Enemy ClosesEnemy { get; private set; }

    public override void Start()
    {
        base.Start();

        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }
    public override void WhenClickOnGround(Vector3 point)
    {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue)
    {
        _health -= damageValue;
        _healthBar.SetHealth(_health, _maxHealth);
        if(_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float FindClosesEnemy()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                ClosesEnemy = allEnemies[i];
            }
        }

        return minDistance;
    }

        private void OnDestroy()
    {
        Management management = FindObjectOfType<Management>();
        if(management)
        {
            management.Unselect(this);
        }

        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);

        }
    }
}
