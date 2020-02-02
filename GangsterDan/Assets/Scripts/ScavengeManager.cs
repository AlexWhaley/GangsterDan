using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScavengeManager : MonoBehaviour
{
    public static ScavengeManager Instance;

    public ScavengeState State;
    public float ConveyorSpeed = 2;
    
    [SerializeField] private List<GameObject> SpawnableItems;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private float SpawnDelay = 5;
    [SerializeField] private float TotalTime = 60.0f;
    [SerializeField] private Text TimerText;
    [SerializeField] private Text CountdownText;

    private List<ItemData> scavengedItems = new List<ItemData>();
    private float spawnTime = 0;
    private float timeRemaining;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Init()
    {
        spawnTime = SpawnDelay;
        State = ScavengeState.Countdown;
        timeRemaining = 3.99f;
        CountdownText.text = "";
        UpdateGameTimer();
    }

    private void Update()
    {
        timeRemaining -= Time.deltaTime;

        switch (State)
        {
            case ScavengeState.Countdown:
                UpdateCountdownTimer();

                if (timeRemaining < 1.0f)
                {
                    State = ScavengeState.InGame;
                    timeRemaining = TotalTime;
                    CountdownText.text = "";
                }
                break;
            case ScavengeState.InGame:
                UpdateGameTimer();

                spawnTime += Time.deltaTime;

                if (spawnTime >= SpawnDelay)
                {
                    SpawnItem();
                    spawnTime = 0;
                }

                if (timeRemaining < 0.0f)
                {
                    State = ScavengeState.TimeFinished;
                    timeRemaining = 5.0f;
                    TimerText.text = "00:00";
                }
                break;
            case ScavengeState.TimeFinished:
                if (timeRemaining < 0.0f)
                {
                    // Go to next screen.
                }
                break;
        }
    }

    public void CollectItem(ItemData item)
    {
        scavengedItems.Add(item);
    }

    private void SpawnItem()
    {
        int i = Random.Range(0, SpawnableItems.Count);

        Instantiate(SpawnableItems[i], SpawnPoint.position, Quaternion.identity);
    }

    private void UpdateGameTimer()
    {
        TimerText.text = string.Format("{0}:{1}", timeRemaining.ToString("D2"), (timeRemaining % 1).ToString());
    }

    private void UpdateCountdownTimer()
    {
        CountdownText.text = timeRemaining.ToString("D1");
    }
}


public enum ScavengeState
{
    Countdown,
    InGame,
    TimeFinished
}