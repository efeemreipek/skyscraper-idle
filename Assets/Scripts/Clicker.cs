using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayer;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //TODO: change this to event based
        if(InputHandler.Instance.MouseLeftClickPressed)
        {
            Ray ray = cam.ScreenPointToRay(InputHandler.Instance.MousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100f, floorLayer))
            {
                Floor floor = hit.collider.GetComponent<Floor>();

                Debug.Log("Clicked on " + floor.Data.Name);
                floor.AddXP(floor.XPGainOnClick);
            }
        }
    }
}
