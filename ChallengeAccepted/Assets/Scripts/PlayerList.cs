using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChallenge
{
    public string key;
    public bool isFinished;

    public PlayerChallenge(string key, bool isFinished = false)
    {
        this.key = key;
        this.isFinished = isFinished;
    }
}

public class PlayerData
{
    public List<PlayerChallenge> generatedChallenges;
    public string name;

    public PlayerData(List<PlayerChallenge> generatedChallenges, string name)
    {
        this.generatedChallenges = generatedChallenges;
        this.name = name;
    }
}

public class PlayerList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject prefab2;

    [SerializeField] Transform listTransform;

    [SerializeField] GameObject plus;

    public static List<Player> playerList = new List<Player>();


    private void Start()
    {
        playerList.Clear();
        AddPlayer();
        ChangePlusSign();

    }

    public void AddFromButton()
    {
        AddPlayer();
    }

    void AddPlayer(PlayerData data = null, bool wasUsed = false)
    {
        int listChildrenCount = listTransform.childCount;
        var curPrefab = prefab;
        if (listChildrenCount % 2 == 0)
        {
            curPrefab = prefab2;
        }
        var newPlayer = Instantiate(curPrefab, listTransform);
        var newPlayerScript = newPlayer.GetComponent<Player>();
        if (data != null)
        {
            if (data.generatedChallenges.Count == 0)
            {
                newPlayerScript.WasAdded(data);
            }
            newPlayerScript.WasAdded(data, wasUsed);
        }
        else
        {
            newPlayerScript.WasAdded(new PlayerData(new List<PlayerChallenge>(), CalculatePlaceholderName()));
        }
        newPlayer.transform.SetSiblingIndex(listChildrenCount - 1);
        ChangePlusSign();

    }

    string CalculatePlaceholderName()
    {
        return "Player " + (playerList.Count).ToString();
    }


    public void RemovePlayer()
    {
        int listChildrenCount = listTransform.childCount;
        var itemToRemove = listTransform.GetChild(listChildrenCount - 2);
        itemToRemove.GetComponent<Player>().WasRemoved();
    }

    void ChangePlusSign()
    {
        if (playerList.Count % 2 == 0)
        {
            plus.GetComponentInChildren<Image>().color = Color.black;
        }
        else
        {
            plus.GetComponentInChildren<Image>().color = Color.white;
        }
    }

    public int GetPlayerCount()
    {
        return playerList.Count;
    }

    public void LoadPlayers(List<PlayerData> list)
    {
        foreach (Player p in playerList)
        {
            Destroy(p.gameObject);
        }
        playerList.Clear();
        var quote = FindObjectOfType<Quote>();
        var challengesWereGenerated = CheckIfUsed();
        if (quote.quoteActive && challengesWereGenerated)
        {
            quote.Leave();
        }
        if (!quote.quoteActive && !challengesWereGenerated)
        {
            quote.Return();
        }
        StartCoroutine(WaitABit());


        IEnumerator WaitABit()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            foreach (PlayerData pd in list)
            {
                AddPlayer(pd, challengesWereGenerated);
            }
        }

        bool CheckIfUsed()
        {
            var used = false;
            foreach (PlayerData data in list)
            {
                if (data.generatedChallenges.Count > 0)
                {
                    used = true;
                }
            }
            return used;
        }
    }

}
