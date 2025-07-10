using System;
using System.Collections.Generic;
using UnityEngine;

public class YourFloorsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject floorInfoPrefab;
    [SerializeField] private RectTransform container;

    private List<FloorInfo> floorInfoList = new List<FloorInfo>();

    private void OnEnable()
    {
        Skyscraper.Instance.OnFloorAdded += OnFloorAdded;
        Skyscraper.Instance.OnSkyscraperCleared += OnSkyscraperCleared;
    }
    private void OnDisable()
    {
        if(Skyscraper.HasInstance) Skyscraper.Instance.OnFloorAdded -= OnFloorAdded;  
        if(Skyscraper.HasInstance) Skyscraper.Instance.OnSkyscraperCleared -= OnSkyscraperCleared;  
    }
    private void OnFloorAdded(Floor floor)
    {
        GameObject floorInfoGO = Instantiate(floorInfoPrefab, container);
        FloorInfo floorInfo = floorInfoGO.GetComponent<FloorInfo>();
        floorInfoList.Add(floorInfo);
        floorInfo.InitializeFloorInfo(floor);
    }
    private void OnSkyscraperCleared()
    {
        floorInfoList.Clear();
        for(int i = container.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
    }
}
