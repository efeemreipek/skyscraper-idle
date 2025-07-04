using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip buttonClickClip;

    [SerializeField] private GameObject audioPoolObject;
    [SerializeField] private int poolSize = 10;

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
    private void PlayClip(AudioClip clip, float volume = 1f)
    {
        if(clip == null) return;

        AudioSource src = GetPooledAudioSource();
        src.clip = clip;
        src.volume = volume;
        src.Play();
        ReturnToPool(src);
    }

    public void PlayButtonClick() => PlayClip(buttonClickClip, 0.5f);
}
