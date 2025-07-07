using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartButton()
    {
        StartCoroutine(LoadGame());
    }
    private IEnumerator LoadGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(index);
        while(!sceneLoad.isDone)
        {
            yield return null;
        }
    }
    public void SettingsButton()
    {

    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
