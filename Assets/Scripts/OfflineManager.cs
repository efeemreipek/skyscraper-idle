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
            yield break;
        }

        long binaryTime = SaveManager.Instance.SaveData.LastPlayTime;

        // Handle case where LastPlayTime hasn't been set yet
        if(binaryTime == 0)
        {
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
    }

    private void CalculateOfflineEarnings(TimeSpan timeAway)
    {
        if(Skyscraper.Instance?.FloorList == null)
        {
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
    }
    private void ApplyOfflineEarnings()
    {
        if(MoneyManager.HasInstance)
        {
            MoneyManager.Instance.AddMoney(totalOfflineEarnings);

            // You might want to show a popup or notification here
            ui.ShowEarningsPanel(totalOfflineEarnings, totalOfflineHours);
        }
    }
}
