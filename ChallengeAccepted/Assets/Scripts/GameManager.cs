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
        CleanupScene();
    }

    private void CleanupScene()
    {
        var popups = GameObject.FindGameObjectsWithTag("popup");
        var masks = GameObject.FindGameObjectsWithTag("mask");
        foreach (GameObject popup in popups)
        {
            popup.SetActive(false);
        }
        foreach (GameObject mask in masks)
        {
            mask.SetActive(false);
        }
    }

    public void NextScene()
    {
        LoadSceneWithTransition(activeScene + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PreviousScene()
    {
        LoadSceneWithTransition(activeScene - 1);
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
