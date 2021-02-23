using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge
{
    public string name;
    public bool coronaFriendly;
    public string key;
    public bool isStared;
    public bool wasCreated;

    public Challenge(string name, bool coronaFriendly, string key, bool isStared = false, bool wasCreated = false)
    {
        this.name = name;
        this.coronaFriendly = coronaFriendly;
        this.key = key;
        this.isStared = isStared;
        this.wasCreated = wasCreated;
    }
};

public class SavedList
{
    public List<Challenge> challenges = new List<Challenge>();

    public SavedList(List<Challenge> challenges)
    {
        this.challenges = challenges;
    }
}

public class ListClass
{
    public List<string> strings = new List<string>();

    public ListClass(List<string> strings)
    {
        this.strings = strings;
    }
}



public static class Progress
{
    public static string ChallengeListToJson(List<Challenge> challengeList)
    {
        var saved = new SavedList(challengeList);
        var listOfJsons = new List<string>();
        foreach (Challenge chal in saved.challenges)
        {
            listOfJsons.Add(JsonUtility.ToJson(chal));
        }
        var listClass = new ListClass(listOfJsons);
        return JsonUtility.ToJson(listClass);
    }

    public static List<Challenge> JsonToChallengeList(string json)
    {
        var listClass = JsonUtility.FromJson<ListClass>(json);
        var challengeList = new List<Challenge>();
        foreach (string line in listClass.strings)
        {
            challengeList.Add(JsonUtility.FromJson<Challenge>(line));
        }
        return challengeList;
    }

    public static void SaveCurrentChallengeList()
    {
        Debug.Log("loaded list count: " + ChallengeList.challengeList.Count);
        PlayerPrefs.SetString("savedList", ChallengeListToJson(ChallengeList.challengeList));
    }

    public static List<Challenge> LoadChallengeList()
    {
        return JsonToChallengeList(PlayerPrefs.GetString("savedList"));
    }


    public class HelperNameAndJsonClass
    {
        public string name;
        public List<string> list;
        public HelperNameAndJsonClass(string name, List<string> list)
        {
            this.name = name;
            this.list = list;
        }
    }

    public static void SavePlayerData(List<PlayerData> playerList)
    {
        var finalListOfJsons = new List<string>();

        foreach (PlayerData pd in playerList)
        {
            var listOfPlayerChallnegeJsons = new List<string>();
            foreach (PlayerChallenge challenge in pd.generatedChallenges)
            {
                listOfPlayerChallnegeJsons.Add(JsonUtility.ToJson(challenge));
            }

            var newClass = new HelperNameAndJsonClass(pd.name, listOfPlayerChallnegeJsons);

            finalListOfJsons.Add(JsonUtility.ToJson(newClass));
        }
        PlayerPrefsX.SetStringArray("playerData", finalListOfJsons.ToArray());
    }

    public static List<PlayerData> LoadPlayerData()
    {
        var list = new List<PlayerData>();
        var helperList = new List<HelperNameAndJsonClass>();
        var arr = PlayerPrefsX.GetStringArray("playerData");
        foreach (string pd in arr)
        {
            helperList.Add(JsonUtility.FromJson<HelperNameAndJsonClass>(pd));
        }
        foreach (HelperNameAndJsonClass helperClass in helperList)
        {
            var listOfPlayerChallenges = new List<PlayerChallenge>();
            foreach (string playerChallegeInJson in helperClass.list)
            {
                listOfPlayerChallenges.Add(JsonUtility.FromJson<PlayerChallenge>(playerChallegeInJson));
            }
            list.Add(new PlayerData(listOfPlayerChallenges, helperClass.name));
        }

        return list;
    }
}
