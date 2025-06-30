using System;
using System.Collections.Generic;
using UnityEngine;

public class YourFloorsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject floorInfoPrefab;
    [SerializeField] private RectTransform container;

    private List<FloorInfoUI> floorInfoList = new List<FloorInfoUI>();

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
        FloorInfoUI floorInfoUI = floorInfoGO.GetComponent<FloorInfoUI>();
        floorInfoList.Add(floorInfoUI);
        floorInfoUI.InitializePanel(floor);
    }

    private void Update()
    {
        foreach(FloorInfoUI floorInfoUI in floorInfoList)
        {
            floorInfoUI.UpdatePanel();
        } 
    }
}
