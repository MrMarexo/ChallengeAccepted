using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ActualList : MonoBehaviour
{
    List<Challenge> challengeList = new List<Challenge>();


    //**************************For uploading a new list*******************************
    //List<Challenge> newList = new List<Challenge>()
    //{

    //    //paste the challenges here
    //};

    //private void Start()
    //{
    //    SaveChanges(newList);
    //}
    //**********************************************************************************



    public void SaveChanges(List<Challenge> list)
    {
        Save(Progress.ChallengeListToJson(list));
        Debug.Log("SAVED");
    }

    private void Awake()
    {
        challengeList = Progress.JsonToChallengeList(Load());
    }



    string Load()
    {
        return File.ReadAllText(Application.dataPath + "/save.txt");
    }

    void Save(string json)
    {
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }



    public List<Challenge> GetList()
    {
        return challengeList;
    }
}



