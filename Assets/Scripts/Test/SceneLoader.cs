using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public GameManager GameManager;
    public UnitManager UnitManager;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene(SceneName);
        //StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return null;
    }

}
