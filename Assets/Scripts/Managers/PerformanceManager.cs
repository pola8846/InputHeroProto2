using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    //ΩÃ±€≈Ê
    private static PerformanceManager instance;
    public static PerformanceManager Instance => instance;

    [SerializeField]
    private SerializedDictionary<string, float> times=new();
    private static SerializedDictionary<string, float> Times => instance.times;
    [SerializeField]
    private SerializedDictionary<string, int> count=new();
    private static SerializedDictionary<string, int> Count => instance.count;
    private Dictionary<string, Stopwatch> timer=new();
    private static Dictionary<string, Stopwatch> Timer => instance.timer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //ΩÃ±€≈Ê
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        Times.Add("PerformanceManager", 0);
        Timer.Add("PerformanceManager", new());
    }

    public static void StartTimer(string str)
    {
        Timer["PerformanceManager"].Start();
        if (!Times.ContainsKey(str))
        {
            Times.Add(str, 0f);
            Count.Add(str, 0);
        }
        if (!Timer.ContainsKey(str))
        {
            Timer.Add(str, new Stopwatch());
        }
        Timer[str].Start();
        Count[str]++;
        Timer["PerformanceManager"].Stop();
    }

    public static void StopTimer(string str)
    {
        Timer["PerformanceManager"].Start();
        if (!Times.ContainsKey(str))
        {
            return;
        }
        Timer[str].Stop();
        Times[str] = Timer[str].ElapsedMilliseconds;
        Timer["PerformanceManager"].Stop();
    }

    private static void Reorder()
    {
        Timer["PerformanceManager"].Start();
        Timer["PerformanceManager"].Stop();
    }
}
