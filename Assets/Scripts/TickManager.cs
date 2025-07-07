using System;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : Singleton<TickManager>
{
    [SerializeField] private float tickTime = 1f;
    [SerializeField] private int maxSoundsPerTick = 5;

    private float timer = 0f;

    public List<Floor> TickingObjects = new List<Floor>();

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= tickTime)
        {
            timer = 0f;

            int soundsPlayed = 0;

            foreach(var floor in TickingObjects)
            {
                bool generatedMoney = floor.OnTick();

                if(generatedMoney && soundsPlayed < maxSoundsPerTick)
                {
                    AudioManager.Instance.PlayMoneyChange(0.15f);
                    soundsPlayed++;
                }
            }
        }
    }

    public void AddToList(Floor floor)
    {
        if(!TickingObjects.Contains(floor)) TickingObjects.Add(floor);
    }
    public void RemoveFromList(Floor floor)
    {
        if(TickingObjects.Contains(floor)) TickingObjects.Remove(floor);
    }
}
