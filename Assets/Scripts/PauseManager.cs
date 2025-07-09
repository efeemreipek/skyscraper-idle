using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private RectTransform pauseTransform;
    [SerializeField] private GameObject pauseBlackBackground;
    [SerializeField] private Vector2 pauseOnScreenPosition;
    [SerializeField] private Vector2 pauseOffScreenPosition;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private Ease moveEase = Ease.Linear;

    private bool isPaused;

    private void OnEnable()
    {
        InputHandler.Instance.OnPausePressed += OnPausePressed;
    }
    private void OnDisable()
    {
        if(InputHandler.HasInstance) InputHandler.Instance.OnPausePressed -= OnPausePressed;
    }

    private void OnPausePressed()
    {
        isPaused = !isPaused;
        pauseTransform.DOAnchorPos(isPaused ? pauseOnScreenPosition : pauseOffScreenPosition, moveDuration)
            .SetEase(moveEase)
            .SetUpdate(true)
            .OnComplete(() => 
            { 
                pauseBlackBackground.SetActive(isPaused); 
                Time.timeScale = isPaused ? 0f : 1f;
            });
    }

    public void ResumeButton()
    {
        OnPausePressed();
        AudioManager.Instance.PlayButtonClick();
    }
    public void BackToMenuButton()
    {
        StartCoroutine(BackToMenu());
        AudioManager.Instance.PlayButtonClick();
    }
    private IEnumerator BackToMenu()
    {
        int index = SceneManager.GetActiveScene().buildIndex - 1;
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(index);
        while(!sceneLoad.isDone)
        {
            yield return null;
        }
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
}
