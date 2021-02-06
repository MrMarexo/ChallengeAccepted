using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject prefab2;

    [SerializeField] Transform listTransform;

    [SerializeField] GameObject plus;
    [SerializeField] GameObject minus;


    int playerCount = 1;

    private void Start()
    {
        CheckControls();
    }


    public void AddPlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var curPrefab = prefab;
        if (listChildrenCount % 2 == 0)
        {
            curPrefab = prefab2;
        }

        var newPlayer = Instantiate(curPrefab, listTransform);
        var nameChild = newPlayer.transform.GetChild(0);
        nameChild.localScale = Vector3.zero;
        var playerChildrenArr = newPlayer.transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in playerChildrenArr)
        {
            if (child.gameObject.name == "Placeholder")
            {
                child.text = "Player " + (playerCount + 1).ToString();
            }
        }
        newPlayer.transform.SetSiblingIndex(listChildrenCount - 1);
        LeanTween.scale(nameChild.gameObject, Vector3.one, 0.2f).setEaseOutElastic();
        ++playerCount;
        CheckControls();

    }

    public void RemovePlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var itemToRemove = listTransform.GetChild(listChildrenCount - 2);
        LeanTween.value(0, 1, 0.1f).setEaseInExpo().setOnUpdate((value) =>
        {
            foreach (Transform child in itemToRemove)
            {
                child.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, value);
            }
        }).setOnComplete(() =>
        {
            Destroy(itemToRemove.gameObject);
            --playerCount;
            CheckControls();
        });
    }

    void CheckControls()
    {
        if (playerCount <= 1)
        {
            minus.SetActive(false);
        }
        else
        {
            minus.SetActive(true);
        }

        if (playerCount >= 4)
        {
            plus.SetActive(false);
        }
        else
        {
            plus.SetActive(true);
        }
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

}
