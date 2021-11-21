using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingPlacer _buildingPlacer;

    [SerializeField] private GameObject _buildingPrefab;

    public void TryBuy()
    {
        int price = _buildingPrefab.GetComponent<Building>().Price;
        Resources resources = FindObjectOfType<Resources>();

        if (resources.Money >= price)
        {
            resources.WithdrawMoney(price);
            resources.UpdateMoney();
            _buildingPlacer.CreateBuilding(_buildingPrefab);
        }
        else
        {
            Debug.Log("Мало денег!");
        }
    }
}
