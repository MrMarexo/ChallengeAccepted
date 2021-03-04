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

public class HelperListClass
{
    public List<string> strings = new List<string>();

    public HelperListClass(List<string> strings)
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
        var listClass = new HelperListClass(listOfJsons);
        return JsonUtility.ToJson(listClass);
    }

    public static List<Challenge> JsonToChallengeList(string json)
    {
        var listClass = JsonUtility.FromJson<HelperListClass>(json);
        var challengeList = new List<Challenge>();
        foreach (string line in listClass.strings)
        {
            challengeList.Add(JsonUtility.FromJson<Challenge>(line));
        }
        return challengeList;
    }

    public static void SaveCurrentChallengeList(string key)
    {
        PlayerPrefs.SetString(key, ChallengeListToJson(ChallengeList.challengeList));
    }

    public static List<Challenge> LoadChallengeList(string key)
    {
        return JsonToChallengeList(PlayerPrefs.GetString(key));
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

    public static void SavePlayerData(List<PlayerData> playerList, string key)
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
        var finalJson = JsonUtility.ToJson(new HelperListClass(finalListOfJsons));
        PlayerPrefs.SetString(key, finalJson);
    }

    public static List<PlayerData> LoadPlayerData(string key)
    {
        var list = new List<PlayerData>();
        var helperList = new List<HelperNameAndJsonClass>();
        var singleJson = PlayerPrefs.GetString(key, "error");
        if (singleJson == "error")
        {
            Debug.LogWarning("this key: " + key + " doesnt exist");
        }
        var jsonListClass = JsonUtility.FromJson<HelperListClass>(singleJson);
        foreach (string pd in jsonListClass.strings)
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

    public static void SaveNewSaveItemData(List<SaveItemData> list)
    {
        var listOfStrings = new List<string>();
        foreach (SaveItemData data in list)
        {
            listOfStrings.Add(JsonUtility.ToJson(data));
        }
        PlayerPrefs.SetString("saveData", JsonUtility.ToJson(new HelperListClass(listOfStrings)));
    }

    public static List<SaveItemData> LoadSaveItemData()
    {
        var finalList = new List<SaveItemData>();
        var json = PlayerPrefs.GetString("saveData");
        if (json == "")
        {
            Debug.Log("empty save file");
            return finalList;
        }
        var helperClass = JsonUtility.FromJson<HelperListClass>(json);
        foreach (string jsonString in helperClass.strings)
        {
            finalList.Add(JsonUtility.FromJson<SaveItemData>(jsonString));
        }
        return finalList;
    }

    public static void RemoveSavedString(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
}
