using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{
    [SerializeField] private int _mining;

    private Resources _resources;

    private float _timer;

    public override void Start()
    {
        _resources = FindObjectOfType<Resources>();
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= 5f)
        {
            _timer = 0;
            _resources.AddMoney(_mining);
            _resources.UpdateMoney();
        }
    }
}
