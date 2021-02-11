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


    [SerializeField] Toggle toggle;

    List<Challenge> list;

    GameObject listItemWithOptions;

    ActualList actual;






    void Start()
    {
        actual = GetComponent<ActualList>();
        list = actual.GetList();
        PopulateVisualList();

        var stars = GameObject.FindGameObjectsWithTag("playerStar");
        foreach (GameObject star in stars)
        {
            SetAlphaTo(0, star.GetComponent<Image>());
        }

    }

    //Populate "physical" list on the popup with texts //////////////////////////////////////////////////////////////////////////////////////////
    void PopulateVisualList()
    {
        foreach (Transform child in contextTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < list.Count; ++i)
        {
            var newText = Instantiate(prefab, contextTransform);
            newText.GetComponent<ListItem>().Initialize(i, list[i]);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void UpdateStarStatus(bool starStatus, int index)
    {
        list[index].isStared = starStatus;
        Debug.Log("Challenge number " + index + " was changed status to: " + list[index].isStared);
        //actual.SaveChanges(list);
    }

    //Generate  new challenge for each player /////////////////////////////////////////////////////////////////////////////////////////////////
    public void Generate()
    {
        GetComponent<Quote>().Leave();
        bool corona = toggle.isOn;
        int playerCount = GetComponent<PlayerList>().GetPlayerCount();
        var results = GameObject.FindGameObjectsWithTag("result");
        var resultNumbers = GameObject.FindGameObjectsWithTag("resultNumber");
        var stars = GameObject.FindGameObjectsWithTag("playerStar");

        foreach (GameObject star in stars)
        {
            SetAlphaTo(0, star.GetComponent<Image>());
        }

        for (int i = 0; i < playerCount; ++i)
        {
            var shouldShowStar = false;
            var curStarImage = stars[i].GetComponent<Image>();
            var curResultNumberGO = resultNumbers[i];
            var curResultGO = results[i];
            curResultGO.GetComponent<TextMeshProUGUI>().text = "";


            var counterOfAdded = 0;
            var originalCount = list.Count;
            for (int ind = 0; ind < originalCount; ++ind)
            {
                if (list[ind].isStared)
                {
                    list.Add(list[ind]);
                    ++counterOfAdded;
                }
            }

            var resIndex = Random.Range(0, list.Count);
            if (corona)
            {
                while (!list[resIndex].coronaFriendly)
                {
                    resIndex = Random.Range(0, list.Count);
                }
            }
            var curChallenge = list[resIndex];
            if (curChallenge.isStared)
            {
                shouldShowStar = true;
            }

            for (int ind = 0; ind < counterOfAdded; ++ind)
            {
                list.RemoveAt(list.Count - 1);
            }

            if (list.Count != originalCount)
            {
                Debug.LogWarning("Something is wrong here dude!");
            }
            var realResIndex = list.FindIndex((x) => x == curChallenge);
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
        list.Remove(list[index]);
        PopulateVisualList();
    }


    ////////////////////////////////////////////////////////////////////

    public void AddNewChallenge(Challenge chal)
    {
        var newText = Instantiate(prefab, contextTransform);
        var listItem = newText.GetComponent<ListItem>();
        listItem.Initialize(list.Count, chal);
        listItem.ReceiveInfo(ReceiverType.Number);
        list.Add(chal);
        //actual.SaveChanges(list);

        //var heightOffest = contextTransform.GetComponent<RectTransform>().sizeDelta.y / 2;
        //LeanTween.moveY(contextTransform.gameObject, contextTransform.position.y + 10000, 0.3f);
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
        list[index] = chal;
        //actual.SaveChanges(list);
    }


    ///////////////////////////////////////////////////////////////////////////

    void SetAlphaTo(float alpha, Image star)
    {
        var newColor = star.color;
        newColor.a = alpha;
        star.color = newColor;
    }









}
