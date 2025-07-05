using DG.Tweening;
using UnityEngine;

public class BirdWingChirper : MonoBehaviour
{
    [SerializeField] private Transform[] wings = new Transform[2];
    [SerializeField] private float angle = 30f;
    [SerializeField] private float chirpTime = 2f;

    private float timer = 0f;
    private bool isReverseChirp;

    private void Update()
    {
        if(timer >= chirpTime)
        {
            timer = 0f;

            for(int i = 0; i < wings.Length; i++)
            {
                Transform wing = wings[i];
                float inverter = (i == 0) ? -1f : 1f;
                Vector3 rotationTarget = isReverseChirp ? new Vector3(-angle * inverter, 0f, 0f) : new Vector3(angle * inverter, 0f, 0f);

                wing.DOLocalRotate(rotationTarget, chirpTime * 0.5f);
            }

            isReverseChirp = !isReverseChirp;
        }
        timer += Time.deltaTime;
    }
}
