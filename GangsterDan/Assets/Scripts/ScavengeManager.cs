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
    
    [SerializeField] private List<GameObject> SpawnableWheels;
    [SerializeField] private List<GameObject> SpawnableFrames;
    [SerializeField] private List<GameObject> SpawnableSeats;
    [SerializeField] private List<GameObject> SpawnableHandlebars;

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

    private List<int> wheelIndices = new List<int>();
    private List<int> frameIndices = new List<int>();
    private List<int> seatIndices = new List<int>();
    private List<int> handlebarsIndices = new List<int>();
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

                if (timeRemaining < 0.0f || HasAllNeededObjects())
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

    private bool HasAllNeededObjects()
    {
        return wheelIndices.Count() >= 2 &&
               frameIndices.Count() >= 1 &&
               seatIndices.Count() >= 1 &&
               handlebarsIndices.Count() >= 1;
    }

    public void CollectItem(ItemData item)
    {
        switch (item.Type)
        {
            case ItemType.Wheel:
                wheelIndices.Add(item.Index);
                if (!WheelsCheckmark.activeSelf && wheelIndices.Count() >= 2)
                {
                    WheelsCheckmark.SetActive(true);
                }
                break;
            case ItemType.Frame:
                frameIndices.Add(item.Index);
                if (!FrameCheckmark.activeSelf)
                {
                    FrameCheckmark.SetActive(true);
                }
                break;
            case ItemType.Seat:
                seatIndices.Add(item.Index);
                if (!SeatCheckmark.activeSelf)
                {
                    SeatCheckmark.SetActive(true);
                }
                break;
            case ItemType.Handlebars:
                handlebarsIndices.Add(item.Index);
                if (!HandlebarsCheckmark.activeSelf)
                {
                    HandlebarsCheckmark.SetActive(true);
                }
                break;
        }
    }

    private void SpawnItem()
    {
        var type = (ItemType)Random.Range(0, 4);

        List<GameObject> itemList = new List<GameObject>();

        switch (type)
        {
            case ItemType.Wheel:
                itemList = SpawnableWheels;
                break;
            case ItemType.Frame:
                itemList = SpawnableFrames;
                break;
            case ItemType.Seat:
                itemList = SpawnableSeats;
                break;
            case ItemType.Handlebars:
                itemList = SpawnableHandlebars;
                break;
            default:
                break;
        }

        int i = Random.Range(0, itemList.Count);

        var go = Instantiate(itemList[i], SpawnPoint.position, Quaternion.identity) as GameObject;

        var itemController = go.AddComponent<ConveyorItemController>();

        itemController.Data = new ItemData()
        {
            Type = type,
            Index = i
        };

        var joints = go.GetComponents<Joint2D>();

        foreach (var j in joints)
        {
            j.enabled = false;
        }

        go.transform.localScale *= type == ItemType.Frame ? 1 : 1.5f;
        go.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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