using UnityEngine;
using UnityEngine.Events;

public class UnityEventTest : MonoBehaviour
{
    public UnityEvent<int> m_MyEvent;

    void Start()
    {
        m_MyEvent.AddListener(Ping);
    }

    void Update()
    {
        if (Input.anyKeyDown && m_MyEvent != null)
        {
            m_MyEvent.Invoke(5);
        }
    }

    void Ping(int i)
    {
        Debug.Log("Ping" + i);
    }
}