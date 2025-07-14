using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayer;
    [Header("Object Pool")]
    [SerializeField] private GameObject clickEffectPoolObject;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float lifetime = 0.1f;

    private Queue<GameObject> clickEffectPool = new Queue<GameObject>();
    private Camera cam;
    private WaitForSeconds lifetimeSeconds;

    private void Awake()
    {
        cam = Camera.main;

        InitializePool();

        lifetimeSeconds = new WaitForSeconds(lifetime);
    }
    private void OnEnable()
    {
        InputHandler.Instance.OnMouseLeftClickPressed += Click;
    }
    private void OnDisable()
    {
        if(InputHandler.HasInstance) InputHandler.Instance.OnMouseLeftClickPressed -= Click;
    }

    private void InitializePool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(clickEffectPoolObject, transform);
            go.SetActive(false);
            clickEffectPool.Enqueue(go);
        }
    }
    private GameObject GetPooledGameObject()
    {
        if(clickEffectPool.Count > 0)
        {
            GameObject go = clickEffectPool.Dequeue();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject go = Instantiate(clickEffectPoolObject, transform);
            return go;
        }
    }
    private void ReturnToPool(GameObject go)
    {
        StartCoroutine(ReturnAfterPlaying(go));
    }
    private IEnumerator ReturnAfterPlaying(GameObject go)
    {
        yield return lifetimeSeconds;
        go.SetActive(false);
        clickEffectPool.Enqueue(go);
    }
    private void GetClickEffect(Vector3 position, bool randomRotation = true)
    {
        GameObject go = GetPooledGameObject();
        go.transform.position = position;
        if(randomRotation) go.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(-360f, 360f));
        ReturnToPool(go);
    }

    private void Click()
    {
        Vector3 mousePosition = InputHandler.Instance.MousePosition;
        Ray ray = cam.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, floorLayer))
        {
            Floor floor = hit.collider.GetComponent<Floor>();

            Vector3 clickEffectPosition = mousePosition;
            clickEffectPosition.z = 16f;
            clickEffectPosition = cam.ScreenToWorldPoint(clickEffectPosition);

            GetClickEffect(clickEffectPosition);

            AudioManager.Instance.PlayFloorClick();
            floor.AddXPByClick();
        }
    }
}
