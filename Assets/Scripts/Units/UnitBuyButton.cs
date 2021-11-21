using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuyButton : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;

    [SerializeField] private Transform _unitSpawnTransform;
    public void TryBuyUnit()
    {
        int price = _unitPrefab.GetComponent<Unit>().Price;
        Resources resources = FindObjectOfType<Resources>();

        if (resources.Money >= price)
        {
            resources.WithdrawMoney(price);
            resources.UpdateMoney();
            GameObject newUnit = Instantiate(_unitPrefab, _unitSpawnTransform.position, Quaternion.identity);
            Vector3 position = _unitSpawnTransform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            newUnit.GetComponent<Unit>().WhenClickOnGround(position);
        }
        else
        {
            Debug.Log("Мало денег!");
        }
    }
}
