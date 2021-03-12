using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EListReceiverType
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
    [SerializeField] MessageScript message;


    SearchSettings settings;


    public static List<Challenge> challengeList;

    GameObject listItemWithOptions;



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
        settings = GetComponent<SearchSettings>();
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
        bool freeGenerating = settings.FreeGeneratingToggle;

        foreach (Player player in PlayerList.playerList)
        {
            if (player.ShouldGeneratorStop() && !freeGenerating)
            {
                message.Appear("All players have to finish their challenges!");
                return;
            }
        }
        List<string> GetCleanListOfAllPlayersChallenges(List<Player> listOfPlayers)
        {
            var allPlayersChallengesTogether = new List<string>();
            foreach (Player player in PlayerList.playerList)

            {
                var list = player.playerData.generatedChallenges;
                foreach (PlayerChallenge chal in list)
                {
                    allPlayersChallengesTogether.Add(chal.key);
                }
            }
            var withoutRepeats = new List<string>();
            foreach (string c in allPlayersChallengesTogether)
            {
                if (!withoutRepeats.Contains(c))
                {
                    withoutRepeats.Add(c);
                }
            }
            return withoutRepeats;
        }



        var playerList = PlayerList.playerList;


        GetComponent<Quote>().Leave();
        bool corona = settings.SocialToggle;
        bool repeat = settings.RepeatToggle;
        bool playerRepeat = settings.PlayerRepeatToggle;
        bool repeatInTurn = settings.RepeatInTurnToggle;
        var results = GameObject.FindGameObjectsWithTag("result");
        var resultNumbers = GameObject.FindGameObjectsWithTag("resultNumber");
        var checks = GameObject.FindGameObjectsWithTag("playerCheck");


        foreach (GameObject check in checks)
        {
            StaticScripts.SetAlphaTo(0, check.GetComponentInChildren<Image>());
        }

        var listInThisTurn = new List<string>();

        for (int i = 0; i < playerList.Count; ++i)
        {
            var listOfKeys = new List<string>();
            foreach (Challenge c in challengeList)
            {
                listOfKeys.Add(c.key);
            }

            if (corona)
            {
                listOfKeys.RemoveAll((k) => !FindChallengeByKey(k).coronaFriendly);
            }

            if (!repeatInTurn)
            {
                listOfKeys.RemoveAll((k) => listInThisTurn.Contains(k));
                if (listOfKeys.Count <= 0)
                {
                    message.Appear("No more challenges, turn off some toggles");
                    return;
                }
            }

            if (!repeat)
            {
                var allChallengesWithoutRepeatsSoFar = GetCleanListOfAllPlayersChallenges(playerList);
                listOfKeys.RemoveAll((k) => allChallengesWithoutRepeatsSoFar.Contains(k));
                if (listOfKeys.Count <= 0)
                {
                    message.Appear("No more challenges, turn off some toggles");
                    return;
                }
            }

            if (!playerRepeat)
            {
                var allKeys = GetCleanListOfAllPlayersChallenges(new List<Player>() { playerList[i] });
                listOfKeys.RemoveAll((k) => allKeys.Contains(k));
                if (listOfKeys.Count <= 0)
                {
                    message.Appear("No more challenges, turn off some toggles");
                    return;
                }
            }


            var curCheckImage = checks[i].GetComponentInChildren<Image>();
            var curResultNumberGO = resultNumbers[i];
            var curResultGO = results[i];


            //var counterOfAdded = 0;
            //var originalCount = challengeList.Count;
            //for (int ind = 0; ind < originalCount; ++ind)
            //{
            //    if (challengeList[ind].isStared)
            //    {
            //        challengeList.Add(challengeList[ind]);
            //        ++counterOfAdded;
            //    }
            //}

            var resIndex = Random.Range(0, listOfKeys.Count);


            var curChallenge = FindChallengeByKey(listOfKeys[resIndex]);
            playerList[i].playerData.generatedChallenges.Add(new PlayerChallenge(curChallenge.key));
            playerList[i].ChangeToCross();

            //if (curChallenge.isStared)
            //{
            //    shouldShowStar = true;
            //}

            //for (int ind = 0; ind < counterOfAdded; ++ind)
            //{
            //    challengeList.RemoveAt(challengeList.Count - 1);
            //}

            //if (challengeList.Count != originalCount)
            //{
            //    Debug.LogWarning("Something is wrong here dude!");
            //}

            curResultGO.GetComponent<TextMeshProUGUI>().text = "";

            var realResIndex = challengeList.FindIndex((x) => x == curChallenge);
            var time = (realResIndex + 1) * 0.01f;
            LeanTween.value(resultNumbers[i], 1f, realResIndex + 1, time).setEaseInBounce().setOnUpdate((value) => ChangeValue(value, curResultNumberGO))
                .setOnComplete(() => PerformFinalChange(curResultGO, curChallenge.name, curCheckImage));
        }

        void ChangeValue(float value, GameObject resultNumber)
        {
            resultNumber.GetComponent<TextMeshProUGUI>().text = value.ToString("F0");
        }

        void PerformFinalChange(GameObject result, string challenge, Image checkImage)
        {
            result.transform.localScale = Vector3.zero;
            result.GetComponent<TextMeshProUGUI>().text = challenge;
            LeanTween.scale(result, Vector3.one, 0.2f).setEaseOutExpo();


            LeanTween.value(0, 1, 0.8f).setEaseOutExpo().setOnUpdate((value) =>
            {
                StaticScripts.SetAlphaTo(value, checkImage);
            });
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AdministerOpenedOptions(GameObject newListItem)
    {
        if (listItemWithOptions && listItemWithOptions != newListItem)
        {
            listItemWithOptions.GetComponent<ListItem>().ReceiveInfo(EListReceiverType.DisableOptions);
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
        listItem.ReceiveInfo(EListReceiverType.Number);
        challengeList.Add(chal);
        //actual.SaveChanges(list);

        ScrollToBottomOfList();
    }

    string KeyGenerator()
    {
        var date = System.DateTime.Now.ToString();
        date = date.Replace(" ", "-");
        var key = "new#kn" + date;
        return key;
    }

    void ScrollToBottomOfList()
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




    ///////////////////////////////////////////////////////////////////////

    public void LastTwoInList()
    {
        ScrollToBottomOfList();
    }

}
