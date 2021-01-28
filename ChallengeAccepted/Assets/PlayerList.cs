using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform listTransform;

    [SerializeField] GameObject plus;
    [SerializeField] GameObject minus;


    int playerCount = 1;

    private void Start()
    {
        CheckSigns();
    }


    public void AddPlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var newPlayer = Instantiate(prefab, listTransform);
        newPlayer.transform.SetSiblingIndex(listChildrenCount - 1);
        ++playerCount;
        CheckSigns();

    }

    public void RemovePlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var itemToRemove = listTransform.GetChild(listChildrenCount - 2);
        Destroy(itemToRemove.gameObject);
        --playerCount;
        CheckSigns();

    }

    void CheckSigns()
    {
        if (playerCount <= 1) minus.SetActive(false);
        else minus.SetActive(true);

        if (playerCount >= 4) plus.SetActive(false);
        else plus.SetActive(true);
    }

    public int GetPlayerCount()
    {
        return playerCount;
    }

}
