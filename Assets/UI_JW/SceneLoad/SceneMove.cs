using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneMove : MonoBehaviour
{
    public string sceneNameToLoad;
    public TextMeshProUGUI text;

    void Start()
    {
        text.text = "Move To " + sceneNameToLoad;
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
