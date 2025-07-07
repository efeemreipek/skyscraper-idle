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
        AudioManager.Instance.PlayButtonClick();
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
        AudioManager.Instance.PlayButtonClick();
    }
    public void QuitButton()
    {
        StartCoroutine(QuitGame());
        AudioManager.Instance.PlayButtonClick();
    }
    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(0.1f);
        Application.Quit();
    }

    // SETTINGS MENU
    public void ApplyButton()
    {
        AudioManager.Instance.PlayButtonClick();
    }
    public void BackButton()
    {
        menuTransform.DOAnchorPos(mainMenuPosition, moveDuration).SetEase(moveEase);
        AudioManager.Instance.PlayButtonClick();
    }
}
