using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class temptemp : MonoBehaviour
{
    public float t;
    private void Start()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(e());

    }

    private IEnumerator e()
    {
        yield return new WaitForSecondsRealtime(t);
        SceneManager.LoadScene("Start", LoadSceneMode.Single);

    }
}
