using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이벤트매니저
public class EventManager_Test : MonoBehaviour
{
    private static EventManager_Test instance = null;

    public List<CustomEvent_Test> events = new List<CustomEvent_Test>();

    public static EventManager_Test Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        foreach (CustomEvent_Test e in events)
        {
            e.Initialize();
        }
    }

    void Update()
    {
        foreach (CustomEvent_Test e in events)
        {
            if (e.Stage == CustomEvent_Test.EventStage.INACTIVE)
            {
                return;
            }
            else
            {
                e.Process();
            }
        }
    }

    public bool TriggerEventID(int id)
    {
        if (0 < id || id >= events.Count) return false;
        if (events[id].Stage != CustomEvent_Test.EventStage.INACTIVE) return false;

        events[id].SetStageEnter();
        return true;
    }
}