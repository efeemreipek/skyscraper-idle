using System;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : Singleton<Skyscraper>
{
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private float floorHeight = 4f;
    [SerializeField] private int floorMaxLimit = 50;

    public List<Floor> FloorList = new List<Floor>();

    public event Action<Floor> OnFloorAdded;
    
    public bool CanBuyNewFloor => FloorList.Count < floorMaxLimit;

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
        floorGO.transform.localPosition = new Vector3(0f, FloorList.Count * floorHeight, 0f);
        Floor floor = floorGO.GetComponent<Floor>();
        floor.InitializeFloor(data);
        FloorList.Add(floor);
        OnFloorAdded?.Invoke(floor);
    }
}
