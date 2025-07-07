using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform menuTransform;
    [SerializeField] private Vector2 mainMenuPosition;
    [SerializeField] private Vector2 settingsMenuPosition;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private Ease moveEase = Ease.Linear;

    // MAIN MENU
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
        menuTransform.DOAnchorPos(settingsMenuPosition, moveDuration).SetEase(moveEase);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    // SETTINGS MENU
    public void ApplyButton()
    {

    }
    public void BackButton()
    {
        menuTransform.DOAnchorPos(mainMenuPosition, moveDuration).SetEase(moveEase);
    }
}
