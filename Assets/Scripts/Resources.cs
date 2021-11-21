using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public int Money { get; private set; } = 50;

    [SerializeField] private Text _moneyText;

    private void Start()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        if (_moneyText != null)
        {
            _moneyText.text = Money.ToString();
        }
    }

    public void WithdrawMoney(int value)
    {
        Money -= value;
    }

    public void AddMoney(int value)
    {
        Money += value;
    }
}
