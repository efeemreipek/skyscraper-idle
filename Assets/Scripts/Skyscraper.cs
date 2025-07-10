using System;
using System.Collections.Generic;
using UnityEngine;

public class Skyscraper : Singleton<Skyscraper>
{
    [SerializeField] private FloorDataList floorDataList;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private float floorHeight = 4f;
    [SerializeField] private int floorMaxLimit = 50;

    public List<Floor> FloorList = new List<Floor>();

    public event Action<Floor> OnFloorAdded;
    public event Action OnSkyscraperCleared;
    
    public bool CanBuyNewFloor => FloorList.Count < floorMaxLimit;

    private void Start()
    {
        List<FloorSaveData> floorSaveDatas = SaveManager.Instance.LoadFloors();

        foreach(var floorSaveData in floorSaveDatas)
        {
            FloorData floorData = LoadFloorData(floorSaveData);
            if(floorData != null)
            {
                AddNewFloor(floorData, floorSaveData);
            }
            else
            {
                Debug.Log("Missing FloorData for ID:" + floorSaveData.FloorID);
            }
        }
    }

    private FloorData LoadFloorData(FloorSaveData floorSaveData)
    {
        foreach(FloorData floorData in floorDataList.List)
        {
            if(string.Equals(floorSaveData.FloorID, floorData.Name))
            {
                return floorData;
            }
        }
        return null;
    }

    public void AddNewFloor(FloorData data, FloorSaveData saveData = null)
    {
        GameObject floorGO = Instantiate(floorPrefab, transform);
        floorGO.transform.localPosition = new Vector3(0f, FloorList.Count * floorHeight, 0f);

        Floor floor = floorGO.GetComponent<Floor>();
        floor.InitializeFloor(data);

        if(saveData != null)
        {
            floor.LoadFromSave(saveData);
        }

        FloorList.Add(floor);
        OnFloorAdded?.Invoke(floor);
    }
    public void ClearSkyscraper()
    {
        FloorList.Clear();
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        OnSkyscraperCleared?.Invoke();
    }
}
