using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _healthBarPrefab;
    private HealthBar _healthBar;
    private int _maxHealth;

    public int Price;
    public int XSize = 3, ZSize = 3;

    [SerializeField] private GameObject _buildMenu;

    private Color _startColor;
    [SerializeField] private Renderer _renderer;

    void Awake()
    {
        _startColor = _renderer.material.color;
        _maxHealth = _health;
        GameObject healthBar = Instantiate(_healthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;

        for(int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0f, z) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
            }
        }
    }

    public void DisplayUnacceptablePosition()
    {
        _renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition()
    {
        _renderer.material.color = _startColor;
    }

    public override void Select()
    {
        base.Select();

        _buildMenu.SetActive(true);
    }

    public override void Unselect()
    {
        base.Unselect();

        _buildMenu.SetActive(false);
    }

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
        Management management = FindObjectOfType<Management>();
        if (management)
        {
            management.Unselect(this);
        }

        BuildingPlacer buildingPlacer = FindObjectOfType<BuildingPlacer>();
        if(buildingPlacer)
        {
            buildingPlacer.UninstallBuilding(this);
        }


        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);

        }
    }
}
