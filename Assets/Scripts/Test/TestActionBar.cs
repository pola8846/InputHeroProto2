using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestActionBar : MonoBehaviour
{
    [SerializeField]
    private GameObject action;

    private List<pair> pairs = new();

    private void Start()
    {
        
    }

    public void add(InputType type)
    {
        pairs.Add(new pair(type, action, transform));
    }

    public void remove(int index)
    {
        if (pairs.Count<=index)
        {
            Debug.LogError("ÀÎµ¦½º ÃÊ°ú");
            return;
        }

        Destroy(pairs[index].GO);
        pairs.RemoveAt(index);
    }

    public void removeAll()
    {
        for (int i = pairs.Count-1; i >= 0; i--)
        {
            remove(pairs.Count - 1);
        }
    }

    private struct pair
    {
        public InputType type;
        public GameObject GO;

        public pair(InputType type, GameObject prefeb, Transform parant)
        {
            this.type = type;
            GO = Instantiate(prefeb, parant);
            GO.GetComponentInChildren<TextMeshProUGUI>().text = type.ToString();
        }
    }
}
