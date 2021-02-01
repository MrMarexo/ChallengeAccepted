using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChallengeList : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject optionsPrefab;
    [SerializeField] GameObject deleteCheckPrefab;


    [SerializeField] Transform contextTransform;


    [SerializeField] Toggle toggle;

    List<Challenge> list;

    [SerializeField] Color itemColor;

    public bool updated = false;


    void Start()
    {
        list = GetComponent<ActualList>().GetList();
        Debug.Log(list.Count);
        PopulatePhysicalList();
    }
    
    //Populate "physical" list on the popup with texts //////////////////////////////////////////////////////////////////////////////////////////
    void PopulatePhysicalList()
    {
        foreach (Transform child in contextTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < list.Count; ++i)
        {
            var newText = Instantiate(prefab, contextTransform);
            var tms = newText.GetComponentsInChildren<TextMeshProUGUI>();
            tms[0].text = (i + 1).ToString();
            tms[1].text = list[i].name;
            newText.GetComponentInChildren<ListItem>().SetIndex(i);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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

    public void DeleteChallenge(GameObject item, int index)
    {
        var optionsItem = item.transform.GetChild(1); 
        if (optionsItem.childCount > 3)
        {
            RemoveDeleteCheck(optionsItem.GetChild(optionsItem.childCount - 1).gameObject);
            return;
        }
        Instantiate(deleteCheckPrefab, optionsItem);
        StartCoroutine(WaitToDelete(item, index));
        //Destroy(item);
        //list.Remove(list[index]);
        //PopulatePhysicalList();
        //Debug.Log(list.Count);
    }

    IEnumerator WaitToDelete(GameObject item, int index)
    {
        yield return new WaitUntil(() => updated);
        Destroy(item);
        list.Remove(list[index]);
        PopulatePhysicalList();
        Debug.Log(list.Count);
    }

    public void Answer(bool answer)
    {

        updated = true;

    }

    void RemoveDeleteCheck(GameObject checkItem)
    {
        Destroy(checkItem);
    }

    public void AddOptions(GameObject item, int index)
    {
        var image = item.GetComponent<Image>();

        var color = image.color;
        var tms = item.GetComponentsInChildren<TextMeshProUGUI>();
        var tmColor = tms[0].color;
        if (item.transform.childCount > 1)
        {
            DeleteOptions(item, image, tms, color, tmColor);
            return;
        }
        var options = Instantiate(optionsPrefab, item.transform);
        var iconImages = options.GetComponentsInChildren<Image>();
        LeanTween.value(0.1f, 1f, 0.3f).setEaseOutQuint().setOnUpdate((value) =>
        {
            foreach (TextMeshProUGUI tm in tms)
            {
                tm.color = Color.Lerp(tmColor, Color.white, value);
            }
            image.color = Color.Lerp(color, Color.black, value);
            foreach (Image img in iconImages)
            {
                img.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, value);
            }
        });
        options.GetComponentInChildren<Delete>().SetIndex(index);

    }

    void DeleteOptions(GameObject parentItem, Image image, TextMeshProUGUI[] tms, Color imageColor, Color tmColor)
    {
        LeanTween.value(0.1f, 1f, 0.3f).setOnUpdate((value) =>
        {
            foreach (TextMeshProUGUI tm in tms)
            {
                tm.color = Color.Lerp(tmColor, Color.black, value);
            }
            image.color = Color.Lerp(imageColor, itemColor, value);
        });
        Destroy(parentItem.transform.GetChild(1).gameObject);
    }
   


}
