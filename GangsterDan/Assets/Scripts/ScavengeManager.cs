using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("UI Elements")]
    [SerializeField] private Text TimerText;
    [SerializeField] private Text CountdownText;
    [SerializeField] private GameObject FrameCheckmark;
    [SerializeField] private GameObject WheelsCheckmark;
    [SerializeField] private GameObject SeatCheckmark;
    [SerializeField] private GameObject HandlebarsCheckmark;

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
        Init();
    }

    public void Init()
    {
        spawnTime = SpawnDelay;
        State = ScavengeState.Countdown;
        timeRemaining = 3.99f;
        CountdownText.text = "";
        TimerText.text = "60:00";
        FrameCheckmark.SetActive(false);
        WheelsCheckmark.SetActive(false);
        SeatCheckmark.SetActive(false);
        HandlebarsCheckmark.SetActive(false);
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

        switch (item.Type)
        {
            case ItemType.Wheel:
                if (!WheelsCheckmark.activeSelf && scavengedItems.Where(x => x.Type == ItemType.Wheel).Count() >= 2)
                {
                    WheelsCheckmark.SetActive(true);
                }
                break;
            case ItemType.Frame:
                if (!FrameCheckmark.activeSelf)
                {
                    FrameCheckmark.SetActive(true);
                }
                break;
            case ItemType.Seat:
                if (!SeatCheckmark.activeSelf)
                {
                    SeatCheckmark.SetActive(true);
                }
                break;
            case ItemType.Handlebars:
                if (!HandlebarsCheckmark.activeSelf)
                {
                    HandlebarsCheckmark.SetActive(true);
                }
                break;
        }
    }

    private void SpawnItem()
    {
        int i = Random.Range(0, SpawnableItems.Count);

        Instantiate(SpawnableItems[i], SpawnPoint.position, Quaternion.identity);
    }

    private void UpdateGameTimer()
    {
        int seconds = Mathf.FloorToInt(timeRemaining);
        int milliseconds = Mathf.FloorToInt(timeRemaining % 1 * 100);
        TimerText.text = string.Format("{0}:{1}", seconds.ToString("D2"), milliseconds.ToString("D2"));
    }

    private void UpdateCountdownTimer()
    {
        CountdownText.text = Mathf.FloorToInt(timeRemaining).ToString("D1");
    }
}


public enum ScavengeState
{
    Countdown,
    InGame,
    TimeFinished
}