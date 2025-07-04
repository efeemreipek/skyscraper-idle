using System.Collections;
using UnityEngine;

public class BackgroundMusicPitch : MonoBehaviour
{
    [SerializeField] private float lowPitchValue = 0.9f;
    [SerializeField] private float normalPitchValue = 1f;
    [SerializeField] private float highPitchValue = 1.1f;
    [SerializeField] private float normalTime = 120f;
    [SerializeField] private float reachTime = 30f;
    [SerializeField] private float stayTime = 30f;

    private AudioSource src;
    private WaitForSeconds waitNormal;
    private WaitForSeconds waitStay;

    private void Awake()
    {
        src = GetComponent<AudioSource>();

        waitNormal = new WaitForSeconds(normalTime);
        waitStay = new WaitForSeconds(stayTime);
    }

    private void Start()
    {
        StartCoroutine(PitchRoutine());
    }

    private IEnumerator PitchRoutine()
    {
        while(true)
        {
            src.pitch = normalPitchValue;
            yield return waitNormal;

            float targetPitch = Random.value > 0.5f ? lowPitchValue : highPitchValue;
            yield return StartCoroutine(InterpolatePitch(normalPitchValue, targetPitch, reachTime));

            yield return waitStay;

            yield return StartCoroutine(InterpolatePitch(targetPitch, normalPitchValue, reachTime));
        }
    }

    private IEnumerator InterpolatePitch(float from, float to, float duration)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            src.pitch = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        src.pitch = to;
    }
}
