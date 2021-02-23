using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TextMeshProUGUI number;
    [SerializeField] TextMeshProUGUI challengeName;

    public PlayerData playerData;

    void Awake()
    {
        UpdateNameListener();
        PlayerList.playerList.Add(this);
        playerData = new PlayerData(new List<PlayerChallenge>(), "");
    }


    public void WasAdded(PlayerData data, bool showLastChallenge = false)
    {
        playerData.generatedChallenges = data.generatedChallenges;
        playerData.name = data.name;
        GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 112);
        var inputChildrenArr = nameInput.transform.GetComponentsInChildren<TextMeshProUGUI>();

        if (showLastChallenge)
        {
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

    public void UpdateNameListener()
    {
        nameInput.onEndEdit.AddListener(Submit);
    }

    void Submit(string arg)
    {
        playerData.name = arg;
    }
}
