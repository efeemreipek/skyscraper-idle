using System;
using System.Collections;
using UnityEngine;

public class OfflineManager : Singleton<OfflineManager>
{
    [SerializeField] private double maxOfflineHours = 6;

    private long totalOfflineEarnings;
    private double totalOfflineHours;
    private OfflineManagerUI ui;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponent<OfflineManagerUI>();
    }

    private IEnumerator Start()
    {
        yield return null;

        yield return new WaitUntil(() =>
            SaveManager.HasInstance &&
            Skyscraper.HasInstance &&
            SaveManager.Instance.SaveData != null);

        if(SaveManager.Instance.IsFirstTime)
        {
            Debug.Log("First time player - no offline earnings to calculate");
            yield break;
        }

        long binaryTime = SaveManager.Instance.SaveData.LastPlayTime;

        // Handle case where LastPlayTime hasn't been set yet
        if(binaryTime == 0)
        {
            Debug.Log("No previous play time recorded");
            yield break;
        }

        DateTime lastPlayTime = DateTime.FromBinary(binaryTime);
        TimeSpan timeAway = DateTime.Now - lastPlayTime;

        // Only calculate if player was away for more than a second
        if(timeAway.TotalSeconds > 1)
        {
            CalculateOfflineEarnings(timeAway);

            // Apply the earnings if any were calculated
            if(totalOfflineEarnings > 0)
            {
                ApplyOfflineEarnings();
            }
        }
        else
        {
            Debug.Log("Player was away for less than a second - no offline earnings");
        }
    }

    private void CalculateOfflineEarnings(TimeSpan timeAway)
    {
        if(Skyscraper.Instance?.FloorList == null)
        {
            Debug.LogWarning("Skyscraper or FloorList is null");
            return;
        }

        totalOfflineEarnings = 0;
        int secondsAway = (int)Math.Min(timeAway.TotalSeconds, maxOfflineHours * 3600);

        foreach(Floor floor in Skyscraper.Instance.FloorList)
        {
            if(floor != null && floor.HasManager && floor.CurrentMoneyPerSecond > 0)
            {
                long moneyEarned = floor.CurrentMoneyPerSecond * secondsAway;
                totalOfflineEarnings += moneyEarned;
            }
        }

        totalOfflineHours = Math.Min(timeAway.TotalHours, maxOfflineHours);
        Debug.Log($"Total offline earnings: {totalOfflineEarnings} over {totalOfflineHours:F2} hours");
    }
    private void ApplyOfflineEarnings()
    {
        if(MoneyManager.HasInstance)
        {
            MoneyManager.Instance.AddMoney(totalOfflineEarnings);
            Debug.Log($"Applied {totalOfflineEarnings} offline earnings to player");

            // You might want to show a popup or notification here
            ui.ShowEarningsPanel(totalOfflineEarnings, totalOfflineHours);
        }
        else
        {
            Debug.LogWarning("MoneyManager not available to apply offline earnings");
        }
    }
}
