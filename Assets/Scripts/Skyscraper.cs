using System;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : Singleton<Skyscraper>
{
    [SerializeField] private GameObject floorPrefab;

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

    public void AddNewFloor(FloorData data)
    {
        GameObject floorGO = Instantiate(floorPrefab, transform);
        Floor floor = floorGO.GetComponent<Floor>();
        floor.InitializeFloor(data);
        FloorList.Add(floor);
        OnFloorAdded?.Invoke(floor);
    }
}
