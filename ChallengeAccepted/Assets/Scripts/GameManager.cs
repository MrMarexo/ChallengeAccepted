using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject transitionCanvas;

    int activeScene;

    private void Awake()
    {
        activeScene = SceneManager.GetActiveScene().buildIndex;
        transitionCanvas.SetActive(true);
    }

    private void Start()
    {
        var image = transitionCanvas.transform.GetChild(0).gameObject;
        if (activeScene != 0)
        {
            LeanTween.scale(image, Vector3.zero, 0.3f);
        }
    }

    public void NextScene()
    {
        LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PreviousScene()
    {
        LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void LoadSceneWithTransition(int index)
    {
        var image = transitionCanvas.transform.GetChild(0).gameObject;
        LeanTween.scale(image, Vector3.one, 0.3f).setOnComplete(() =>
        {
            SceneManager.LoadScene(index);
        });
    }
}
