using UnityEngine;

public class BuyFloorsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private RectTransform container;
    [SerializeField] private FloorDataList floorDataList;

    private void Awake()
    {
        foreach(FloorData floorData in floorDataList.List)
        {
            GameObject buttonGO = Instantiate(buttonPrefab, container);
            BuyFloorButtonUI ui = buttonGO.GetComponent<BuyFloorButtonUI>();
            ui.InitializeButton(floorData.Name);
        }
    }
}
