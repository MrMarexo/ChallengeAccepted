using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct Challenge
{
    public string name;
    public bool coronaFriendly;

    public Challenge(string name, bool coronaFriendly)
    {
        this.name = name;
        this.coronaFriendly = coronaFriendly;
    }
};


public class ActualList : MonoBehaviour
{
    List<Challenge> challengeList = new List<Challenge>();

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

    private void Awake()
    {
        challengeList = JsonToMyList(Load());
    }

    string Load()
    {
        return File.ReadAllText(Application.dataPath + "/save.txt");
    }

    void Save(string json)
    {
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    string MyListToJson(List<Challenge> challengeList)
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

    List<Challenge> JsonToMyList(string json)
    {
        var listClass = JsonUtility.FromJson<ListClass>(json);
        var challengeList = new List<Challenge>();
        foreach (string line in listClass.strings)
        {
            challengeList.Add(JsonUtility.FromJson<Challenge>(line));
        }
        return challengeList;
    }

    public List<Challenge> GetList()
    {
        return challengeList;
    }
}



