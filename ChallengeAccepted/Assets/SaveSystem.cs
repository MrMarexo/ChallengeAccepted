using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveCurrent()
    {
        var listOfData = new List<PlayerData>();
        foreach (Player p in PlayerList.playerList)
        {
            listOfData.Add(p.playerData);
        }
        Progress.SavePlayerData(listOfData);
        Progress.SaveCurrentChallengeList();
    }

    public void LoadGroup()
    {
        var loadedPlayerList = Progress.LoadPlayerData();
        var loadedChallengeList = Progress.LoadChallengeList();
        Debug.Log("loaded list count: " + loadedChallengeList.Count);
        ChallengeList.challengeList = loadedChallengeList;
        GetComponent<ChallengeList>().PopulateVisualList();
        GetComponent<PlayerList>().LoadPlayers(loadedPlayerList);
        //Debug.Log(loadedList[1].name + "'s second challenge was " + loadedList[1].generatedChallenges[1].key + "and the status is: " + loadedList[1].generatedChallenges[1].isFinished);
    }
}
