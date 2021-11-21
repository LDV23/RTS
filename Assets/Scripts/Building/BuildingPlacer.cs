using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;

    [SerializeField] private Camera _raycastCamera;

    private Plane _plane;

    [SerializeField] private Building _currentBuilding;

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();

    public int XPoint { get; private set; }
    public int ZPoint { get; private set; }

    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (var check in BuildingDictionary)
            {
                Debug.Log(check);
            }
        }

        if (!_currentBuilding)
        {
            return;
        }

        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int XPoint = Mathf.RoundToInt(point.x);
        int ZPoint = Mathf.RoundToInt(point.z);

        _currentBuilding.transform.position = new Vector3(XPoint, 0f, ZPoint) * CellSize;

        if(CheckAllow(XPoint, ZPoint, _currentBuilding))
        {
            _currentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0))
            {
                InstallBuilding(XPoint, ZPoint, _currentBuilding);
                _currentBuilding = null;
            }
        }
        else
        {
            _currentBuilding.DisplayUnacceptablePosition();
        }
    }

    bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if(BuildingDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }

    void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingDictionary.Add(coordinate, building);
            }
        }
    }


    public void UninstallBuilding(Building building)
    {
        for (int x = 0; x < building.XSize; x++)
        {
            for (int z = 0; z < building.ZSize; z++)
            {
                Vector2Int coordinate = new Vector2Int((int)building.transform.position.x + x, (int)building.transform.position.z + z);
                BuildingDictionary.Remove(coordinate);
            }
        }
    }

    public void CreateBuilding(GameObject buildingPrefab)
    {
        GameObject newBuilding = Instantiate(buildingPrefab);
        _currentBuilding = newBuilding.GetComponent<Building>();
    }
}
