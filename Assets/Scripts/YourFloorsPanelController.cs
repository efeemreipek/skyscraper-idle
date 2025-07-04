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
    }
    private void OnDisable()
    {
        if(Skyscraper.HasInstance) Skyscraper.Instance.OnFloorAdded -= OnFloorAdded;  
    }
    private void OnFloorAdded(Floor floor)
    {
        GameObject floorInfoGO = Instantiate(floorInfoPrefab, container);
        FloorInfo floorInfo = floorInfoGO.GetComponent<FloorInfo>();
        floorInfoList.Add(floorInfo);
        floorInfo.InitializeFloorInfo(floor);
    }
}
