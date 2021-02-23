using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ReceiverType
{
    None = 0,
    Trash,
    Delete,
    Star,
    Edit,
    Number,
    DisableOptions,
    CancelDeletePrompt
};

public class ChallengeList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject deleteCheckPrefab;

    [SerializeField] Transform contextTransform;


    [SerializeField] Toggle socialToggle;
    [SerializeField] Toggle repeatToggle;

    public static List<Challenge> challengeList;

    GameObject listItemWithOptions;


    List<string> listOfGenerated = new List<string>();

    public static Challenge FindChallengeByKey(string key)
    {
        Challenge desiredChal = new Challenge("", true, "X");
        foreach (Challenge chal in challengeList)
        {
            if (chal.key == key)
            {
                desiredChal = chal;
            }
        }
        return desiredChal;
    }

    public static int GetNumberOfChallengeInList(Challenge chal)
    {
        return challengeList.IndexOf(chal) + 1;
    }

    void Start()
    {
        challengeList = GetComponent<ActualList>().GetList();
        PopulateVisualList();

        var stars = GameObject.FindGameObjectsWithTag("playerStar");
        foreach (GameObject star in stars)
        {
            SetAlphaTo(0, star.GetComponent<Image>());
        }
        listOfGenerated.Clear();
    }

    //Populate "physical" list on the popup with texts //////////////////////////////////////////////////////////////////////////////////////////
    public void PopulateVisualList()
    {
        foreach (Transform child in contextTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < challengeList.Count; ++i)
        {
            var newText = Instantiate(prefab, contextTransform);
            newText.GetComponent<ListItem>().Initialize(i, challengeList[i]);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void UpdateStarStatus(bool starStatus, int index)
    {
        challengeList[index].isStared = starStatus;
        Debug.Log("Challenge number " + index + " was changed status to: " + challengeList[index].isStared);
        //actual.SaveChanges(list);
    }

    //Generate  new challenge for each player /////////////////////////////////////////////////////////////////////////////////////////////////
    public void Generate()
    {
        GetComponent<Quote>().Leave();
        bool corona = socialToggle.isOn;
        bool repeat = repeatToggle.isOn;
        var playerList = PlayerList.playerList;
        var results = GameObject.FindGameObjectsWithTag("result");
        var resultNumbers = GameObject.FindGameObjectsWithTag("resultNumber");
        var stars = GameObject.FindGameObjectsWithTag("playerStar");

        foreach (GameObject star in stars)
        {
            SetAlphaTo(0, star.GetComponent<Image>());
        }

        for (int i = 0; i < playerList.Count; ++i)
        {
            var shouldShowStar = false;
            var curStarImage = stars[i].GetComponent<Image>();
            var curResultNumberGO = resultNumbers[i];
            var curResultGO = results[i];


            var counterOfAdded = 0;
            var originalCount = challengeList.Count;
            for (int ind = 0; ind < originalCount; ++ind)
            {
                if (challengeList[ind].isStared)
                {
                    challengeList.Add(challengeList[ind]);
                    ++counterOfAdded;
                }
            }

            var resIndex = Random.Range(0, challengeList.Count);
            if (corona)
            {
                while (!challengeList[resIndex].coronaFriendly)
                {
                    resIndex = Random.Range(0, challengeList.Count);
                }
            }
            if (!repeat)
            {
                if (challengeList.Count - listOfGenerated.Count <= playerList.Count)
                {
                    Debug.Log("all challenges used");
                    return;
                }
                while (listOfGenerated.Contains(challengeList[resIndex].key))
                {
                    resIndex = Random.Range(0, challengeList.Count);

                }
            }

            var curChallenge = challengeList[resIndex];
            playerList[i].playerData.generatedChallenges.Add(new PlayerChallenge(curChallenge.key));

            if (curChallenge.isStared)
            {
                shouldShowStar = true;
            }

            for (int ind = 0; ind < counterOfAdded; ++ind)
            {
                challengeList.RemoveAt(challengeList.Count - 1);
            }

            if (challengeList.Count != originalCount)
            {
                Debug.LogWarning("Something is wrong here dude!");
            }

            curResultGO.GetComponent<TextMeshProUGUI>().text = "";

            var realResIndex = challengeList.FindIndex((x) => x == curChallenge);
            listOfGenerated.Add(challengeList[realResIndex].key);
            var time = (realResIndex + 1) * 0.01f;
            LeanTween.value(resultNumbers[i], 1f, realResIndex + 1, time).setEaseInBounce().setOnUpdate((value) => ChangeValue(value, curResultNumberGO))
                .setOnComplete(() => PerformFinalChange(curResultGO, curChallenge.name, curStarImage, shouldShowStar));
        }

        void ChangeValue(float value, GameObject resultNumber)
        {
            resultNumber.GetComponent<TextMeshProUGUI>().text = value.ToString("F0");
        }

        void PerformFinalChange(GameObject result, string challenge, Image star, bool shouldShow)
        {
            result.transform.localScale = Vector3.zero;
            result.GetComponent<TextMeshProUGUI>().text = challenge;
            LeanTween.scale(result, Vector3.one, 0.2f).setEaseOutExpo();
            if (shouldShow)
            {
                LeanTween.value(0, 1, 0.8f).setEaseOutExpo().setOnUpdate((value) =>
                {
                    SetAlphaTo(value, star);
                });
            }
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AdministerOpenedOptions(GameObject newListItem)
    {
        if (listItemWithOptions && listItemWithOptions != newListItem)
        {
            listItemWithOptions.GetComponent<ListItem>().ReceiveInfo(ReceiverType.DisableOptions);
            listItemWithOptions = newListItem;
        }
        listItemWithOptions = newListItem;
    }

    public void DeleteChallengeFromList(int index, GameObject item)
    {
        StartCoroutine(Cleanup(index, item));
    }

    public IEnumerator Cleanup(int index, GameObject item)
    {
        yield return new WaitWhile(() => item);
        challengeList.Remove(challengeList[index]);
        PopulateVisualList();
    }


    ////////////////////////////////////////////////////////////////////

    public void AddNewChallenge(string name, bool social)
    {
        var newText = Instantiate(prefab, contextTransform);
        var listItem = newText.GetComponent<ListItem>();
        var chal = new Challenge(name, social, KeyGenerator(), false, true);
        listItem.Initialize(challengeList.Count, chal);
        listItem.ReceiveInfo(ReceiverType.Number);
        challengeList.Add(chal);
        //actual.SaveChanges(list);

        ScrollToBottom();
    }

    string KeyGenerator()
    {
        var date = System.DateTime.Now.ToString();
        date = date.Replace(" ", "-");
        var key = "new#kn" + date;
        return key;
    }

    void ScrollToBottom()
    {
        var scrollrect = contextTransform.GetComponentInParent<ScrollRect>();
        var curPos = scrollrect.verticalNormalizedPosition;
        LeanTween.value(curPos, 0, 0.3f).setEaseInExpo().setOnUpdate((value) =>
        {
            scrollrect.verticalNormalizedPosition = value;
        });
    }

    ////////////////////////////////////////////////////////////////////

    public void SaveEditedChallenge(int index, Challenge chal)
    {
        challengeList[index] = chal;
        //actual.SaveChanges(list);
    }


    ///////////////////////////////////////////////////////////////////////////

    void SetAlphaTo(float alpha, Image star)
    {
        var newColor = star.color;
        newColor.a = alpha;
        star.color = newColor;
    }


    ///////////////////////////////////////////////////////////////////////

    public void LastTwoInList()
    {
        ScrollToBottom();
    }

}
