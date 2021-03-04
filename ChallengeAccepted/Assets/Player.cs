using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI number;
    [SerializeField] TextMeshProUGUI challengeName;
    [SerializeField] GameObject toggle;

    Image toggleImage;

    [SerializeField] Sprite checkmark;
    [SerializeField] Sprite cross;

    public PlayerData playerData;

    void Awake()
    {
        AddNameListener();
        //AddToggleListener();
        PlayerList.playerList.Add(this);
        playerData = new PlayerData(new List<PlayerChallenge>(), "");
        toggleImage = toggle.GetComponentInChildren<Image>();
    }

    public void WasAdded(PlayerData data, bool showLastChallenge = false)
    {
        playerData.generatedChallenges = data.generatedChallenges;
        playerData.name = data.name;
        GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 112);
        var inputChildrenArr = nameInput.transform.GetComponentsInChildren<TextMeshProUGUI>();
        ChangeToCross();


        if (showLastChallenge)
        {
            StaticScripts.SetAlphaTo(1, toggleImage);
            if (playerData.generatedChallenges[playerData.generatedChallenges.Count - 1].isFinished)
            {
                toggleImage.sprite = checkmark;
            }
            else
            {
                toggleImage.sprite = cross;
            }

            var lastChallenge = ChallengeList.FindChallengeByKey(data.generatedChallenges[data.generatedChallenges.Count - 1].key);
            if (lastChallenge.key != data.generatedChallenges[data.generatedChallenges.Count - 1].key)
            {
                Debug.LogWarning("This challenge isnt in this list!");
            }
            nameInput.transform.localScale = Vector3.zero;
            number.transform.localScale = Vector3.zero;
            challengeName.transform.localScale = Vector3.zero;
            nameInput.text = data.name;
            number.text = ChallengeList.GetNumberOfChallengeInList(lastChallenge).ToString();
            challengeName.text = lastChallenge.name;
            LeanTween.value(0, 1, 0.2f).setEaseOutElastic().setOnUpdate((value) =>
            {
                number.transform.localScale = new Vector3(value, value, value);
                challengeName.transform.localScale = new Vector3(value, value, value);
                nameInput.transform.localScale = new Vector3(value, value, value);
                toggle.transform.localScale = new Vector3(value, value, value);
            });
        }
        else
        {
            nameInput.transform.localScale = Vector3.zero;
            nameInput.text = data.name;
            LeanTween.scale(nameInput.gameObject, Vector3.one, 0.2f).setEaseOutElastic();
        }
    }

    public void WasRemoved()
    {
        LeanTween.value(0, 1, 0.1f).setEaseInExpo().setOnUpdate((value) =>
        {
            foreach (Transform child in transform)
            {
                child.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, value);
            }
        }).setOnComplete(() =>
        {
            PlayerList.playerList.Remove(this);
            Destroy(gameObject);
        });
    }

    void AddNameListener()
    {
        nameInput.onEndEdit.AddListener(Submit);

        void Submit(string arg)
        {
            playerData.name = arg;
        }
    }

    public void ClickToggle()
    {
        if (playerData.generatedChallenges[playerData.generatedChallenges.Count - 1].isFinished)
        {
            toggleImage.sprite = cross;
            playerData.generatedChallenges[playerData.generatedChallenges.Count - 1].isFinished = false;
        }
        else
        {
            toggleImage.sprite = checkmark;
            playerData.generatedChallenges[playerData.generatedChallenges.Count - 1].isFinished = true;

        }
    }

    public bool ShouldGeneratorStop()
    {
        Debug.Log(playerData.generatedChallenges.Count);
        if (playerData.generatedChallenges.Count > 0)
        {
            return !playerData.generatedChallenges[playerData.generatedChallenges.Count - 1].isFinished;
        }
        return false;
    }

    public void ChangeToCross()
    {
        toggleImage.sprite = cross;
    }



}
