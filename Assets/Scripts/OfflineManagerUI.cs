using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class OfflineManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform offlineEarningsTransform;
    [SerializeField] private TMP_Text offlineEarningsText;
    [SerializeField] private Vector2 onScreenPosition;
    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private float stayDuration = 1f;
    [SerializeField] private Ease moveEase = Ease.Linear;

    public void ShowEarningsPanel(long totalOfflineEarnings, double totalOfflineHours)
    {
        StartCoroutine(MoveEarningsPanel(totalOfflineEarnings, totalOfflineHours));
    }

    private IEnumerator MoveEarningsPanel(long totalOfflineEarnings, double totalOfflineHours)
    {
        offlineEarningsText.text = $"You gained ${totalOfflineEarnings} while offline for {totalOfflineHours:F2} hours";
        offlineEarningsTransform.DOAnchorPos(onScreenPosition, moveDuration).SetEase(moveEase);
        yield return new WaitForSeconds(moveDuration + stayDuration);
        offlineEarningsTransform.DOAnchorPos(offScreenPosition, moveDuration).SetEase(moveEase);
    }
}
