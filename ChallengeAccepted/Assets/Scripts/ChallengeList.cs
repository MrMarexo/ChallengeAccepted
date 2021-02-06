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
        Debug.Log(list.Count);
        PopulateVisualList();
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

        for (int i = 0; i < playerCount; ++i)
        {

            var curResultNumber = resultNumbers[i];
            var curResult = results[i];
            curResult.GetComponent<TextMeshProUGUI>().text = "";
            var resIndex = Random.Range(0, list.Count);
            if (corona)
            {
                while (!list[resIndex].coronaFriendly)
                {
                    resIndex = Random.Range(0, list.Count);
                }

            }

            var curChallenge = list[resIndex].name;
            var time = (resIndex + 1) * 0.01f;
            LeanTween.value(resultNumbers[i], 1f, resIndex + 1, time).setEaseInBounce().setOnUpdate((value) => ChangeValue(value, curResultNumber))
                .setOnComplete(() => ChangeText(curResult, curChallenge));
        }

        void ChangeValue(float value, GameObject resultNumber)
        {
            resultNumber.GetComponent<TextMeshProUGUI>().text = value.ToString("F0");
        }

        void ChangeText(GameObject result, string challenge)
        {
            result.transform.localScale = Vector3.zero;
            result.GetComponent<TextMeshProUGUI>().text = challenge;
            LeanTween.scale(result, Vector3.one, 0.1f).setEaseOutElastic();
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









}
