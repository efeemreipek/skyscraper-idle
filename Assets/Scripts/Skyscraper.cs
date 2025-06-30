using System;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : Singleton<Skyscraper>
{
    public List<Floor> FloorList = new List<Floor>();

    public event Action<Floor> OnFloorAdded;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            Floor floor = child.GetComponent<Floor>();
            FloorList.Add(floor);
            OnFloorAdded?.Invoke(floor);
        }
    }
}
