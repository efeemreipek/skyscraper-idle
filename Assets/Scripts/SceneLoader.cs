using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string SceneToLoad;

    private void Awake()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(SceneToLoad);
        while(!sceneLoad.isDone)
        {
            yield return null;
        }
    }
}
