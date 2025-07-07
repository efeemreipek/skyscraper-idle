using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Clips")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip floorClickClip;
    [SerializeField] private AudioClip moneyChangeClip;
    [Header("Data")]
    [SerializeField] private GameObject audioPoolObject;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float randomPitchLow = 0.8f;
    [SerializeField] private float randomPitchHigh = 1.2f;
    [Header("Volumes")]
    [SerializeField] private float buttonClickVolume = 1f;
    [SerializeField] private float floorClickVolume = 1f;
    [SerializeField] private float moneyChangeVolume = 1f;

    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        InitializePool();
    }

    private void InitializePool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(audioPoolObject, transform);
            AudioSource src = go.GetComponent<AudioSource>();
            go.SetActive(false);
            audioSourcePool.Enqueue(src);
        }
    }
    private AudioSource GetPooledAudioSource()
    {
        if(audioSourcePool.Count > 0)
        {
            AudioSource src = audioSourcePool.Dequeue();
            src.gameObject.SetActive(true);
            return src;
        }
        else
        {
            GameObject go = Instantiate(audioPoolObject, transform);
            AudioSource src = go.GetComponent<AudioSource>();
            return src;
        }
    }
    private void ReturnToPool(AudioSource src)
    {
        StartCoroutine(ReturnAfterPlaying(src));
    }
    private IEnumerator ReturnAfterPlaying(AudioSource src)
    {
        yield return new WaitForSeconds(src.clip.length);
        src.Stop();
        src.clip = null;
        src.gameObject.SetActive(false);
        audioSourcePool.Enqueue(src);
    }
    private void PlayClip(AudioClip clip, float volume, bool randomPitch)
    {
        if(clip == null) return;

        AudioSource src = GetPooledAudioSource();
        src.clip = clip;
        src.volume = volume;
        if(randomPitch) src.pitch = GetRandomPitch();
        src.Play();
        ReturnToPool(src);
    }
    private float GetRandomPitch()
    {
        return Random.Range(randomPitchLow, randomPitchHigh);
    }

    public void PlayButtonClick(bool randomPitch = true) => PlayClip(buttonClickClip, buttonClickVolume, randomPitch);
    public void PlayFloorClick(bool randomPitch = true) => PlayClip(floorClickClip, floorClickVolume, randomPitch);
    public void PlayMoneyChange(bool randomPitch = true) => PlayClip(moneyChangeClip, moneyChangeVolume, randomPitch);
}
