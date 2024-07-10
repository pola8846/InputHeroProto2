using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string sceneNameToLoad;
  

    void Start()
    {
        
    }

    private void Update()
    {
        if(Input.anyKey)
        {
            MoveScene();
        }
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);
    }

    public void MoveSceneAsync()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNameToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
