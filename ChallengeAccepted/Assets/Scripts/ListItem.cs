using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class ListItem : MonoBehaviour
{
    int listItemIndex;

    Challenge challenge;
    string challengeName;
    string challengeSocial;


    bool isStared;

    bool starIsAnimating;

    Image image;

    [SerializeField] Color normalItemColor;
    [SerializeField] Color optionsItemColor;
    [SerializeField] Color normalStarColor;
    [SerializeField] Color activeStarColor;
    [SerializeField] Color normalTextColor;
    [SerializeField] Color optionsTextColor;
    [SerializeField] Color activeNumberColor;
    [SerializeField] Color normalSocialColor;
    [SerializeField] Color optionsSocialColor;


    public void Initialize(int index, Challenge chal)
    {
        var tms = GetComponentsInChildren<TextMeshProUGUI>();
        tms[0].text = (index + 1).ToString();
        tms[1].text = chal.name;

        if (!chal.wasCreated)
        {
            FindReceiver(ReceiverType.Edit).SetActive(false);
        }
        else
        {
            FindReceiver(ReceiverType.Edit).SetActive(true);
            FindReceiver(ReceiverType.Number).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Italic | FontStyles.Bold;
        }

        if (chal.coronaFriendly)
        {
            transform.Find(Names.MAIN).GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
        listItemIndex = index;
        challenge = chal;
        isStared = chal.isStared;
        if (chal.isStared)
        {
            FindReceiver(ReceiverType.Star).GetComponent<Image>().color = activeStarColor;
            FindReceiver(ReceiverType.Number).GetComponent<TextMeshProUGUI>().color = activeNumberColor;
        }
        image = GetComponent<Image>();
        transform.Find(Names.OPTIONS).gameObject.SetActive(false);
    }

    public void ChangeAfterEdit(Challenge chal)
    {
        var tms = GetComponentsInChildren<TextMeshProUGUI>();
        tms[1].text = chal.name;
        challenge = chal;
        var social = transform.Find(Names.MAIN).GetComponentInChildren<Image>(true);
        social.gameObject.SetActive(!chal.coronaFriendly);
        if (!chal.coronaFriendly)
        {
            social.color = optionsSocialColor;
        }
        FindObjectOfType<ChallengeList>().SaveEditedChallenge(listItemIndex, chal);
    }

    GameObject FindReceiver(ReceiverType type)
    {
        var receivers = GetComponentsInChildren<Receiver>();
        foreach (Receiver r in receivers)
        {
            if (r.GetInfoType() == type)
            {
                return r.gameObject;
            }
        }
        Debug.LogWarning("Looking for a nonexisting receiver type!");
        return null;
    }

    public void ReceiveInfo(ReceiverType type)
    {
        Debug.Log(type + " received");
        switch (type)
        {
            case ReceiverType.Delete:
                DeleteItem();
                break;

            case ReceiverType.Edit:
                Debug.Log(challenge.name);
                Resources.FindObjectsOfTypeAll<EditPopup>()[0].InitializeEdit(challenge, this);
                break;

            case ReceiverType.Star:
                Star();
                break;

            case ReceiverType.Trash:
                EnableDeleteItemPrompt();
                break;

            case ReceiverType.CancelDeletePrompt:
                EnableDeleteItemPrompt();
                break;

            case ReceiverType.Number:
                Options();
                break;

            case ReceiverType.DisableOptions:
                DisableOptions();
                break;

            case ReceiverType.None:
                Debug.LogWarning("Set a value for Infotype in the prefab!");
                break;

        }
    }



    void Star()
    {
        if (starIsAnimating)
        {
            return;
        }
        var star = FindReceiver(ReceiverType.Star).transform;
        if (isStared)
        {
            Unstar(star.gameObject);
            return;
        }
        var image = star.GetComponent<Image>();
        image.color = activeStarColor;
        starIsAnimating = true;
        LeanTween.rotateAroundLocal(image.gameObject, Vector3.forward, 360f, 0.8f).setEaseOutExpo().setOnComplete(() =>
        {
            star.rotation = Quaternion.identity;
            starIsAnimating = false;
            SendStarUpdate(true);
        });
        //LeanTween.value(0, 1, 0.2f).setEaseOutExpo().setOnUpdate((value) =>
        //{
        //    var newRot = image.transform.rotation;
        //    newRot.z = value * 360;
        //    image.transform.rotation = newRot;
        //});
    }

    void Unstar(GameObject star)
    {
        var image = star.GetComponent<Image>();
        starIsAnimating = true;
        image.color = normalStarColor;
        LeanTween.rotateAroundLocal(image.gameObject, Vector3.forward, 360f, 0.8f).setEaseOutExpo().setOnComplete(() =>
        {
            star.transform.rotation = Quaternion.identity;
            starIsAnimating = false;
            SendStarUpdate(false);
        });
    }

    void SendStarUpdate(bool status)
    {
        isStared = status;
        FindObjectOfType<ChallengeList>().UpdateStarStatus(status, listItemIndex);
    }

    void DeleteItem()
    {
        var tms = GetComponentsInChildren<TextMeshProUGUI>();
        var images = GetComponentsInChildren<Image>();

        LeanTween.value(1, 0, 0.1f).setEaseInBounce().setOnUpdate((value) =>
        {
            foreach (TextMeshProUGUI tm in tms)
            {
                var newColor = tm.color;
                newColor.a = value;
                tm.color = newColor;
            }
            foreach (Image img in images)
            {
                var newColor = img.color;
                newColor.a = value;
                img.color = newColor;
            }
            var newScale = transform.localScale;
            newScale.y = value;
            transform.localScale = newScale;
        })
            .setOnComplete(() =>
        {
            FindObjectOfType<ChallengeList>().DeleteChallengeFromList(listItemIndex, gameObject);
            Destroy(gameObject);
        });

    }



    void EnableDeleteItemPrompt()
    {
        var options = transform.Find(Names.OPTIONS);
        var deletePrompt = options.Find(Names.DELETE_PROMPT);
        var tms = deletePrompt.GetComponentsInChildren<TextMeshProUGUI>();
        if (deletePrompt.gameObject.activeInHierarchy)
        {
            DisableDeleteItemPrompt(deletePrompt.gameObject, tms);
            return;
        }
        //foreach (TextMeshProUGUI tm in tms)
        //{
        //    tm.color = Color.white;
        //}
        deletePrompt.gameObject.SetActive(true);
        LeanTween.value(0, 1f, 0.3f).setEaseOutBounce().setOnUpdate((value) =>
        {
            foreach (TextMeshProUGUI tm in tms)
            {
                tm.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, value);
            }
        });

    }



    void DisableDeleteItemPrompt(GameObject deletePrompt, TextMeshProUGUI[] tms)
    {
        LeanTween.value(0, 1f, 0.2f).setEaseInBounce().setOnUpdate((value) =>
        {
            foreach (TextMeshProUGUI tm in tms)
            {
                tm.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, value);
            }
        }).setOnComplete(() =>
        {
            deletePrompt.SetActive(false);
        });
    }


    void Options()
    {
        var options = transform.Find(Names.OPTIONS).gameObject;
        if (options.activeInHierarchy)
        {
            DisableOptions();
            return;
        }
        var tms = GetComponentsInChildren<TextMeshProUGUI>();
        var iconImages = options.GetComponentsInChildren<Image>();
        var socialImage = transform.Find(Names.MAIN).GetComponentInChildren<Image>();
        FindObjectOfType<ChallengeList>().AdministerOpenedOptions(gameObject);
        options.SetActive(true);

        if (listItemIndex >= ChallengeList.challengeList.Count - 2)
        {
            FindObjectOfType<ChallengeList>().LastTwoInList();
        }

        LeanTween.value(0, 1f, 0.3f).setOnUpdate((value) =>
        {
            if (!isStared)
            {
                tms[0].color = Color.Lerp(normalTextColor, optionsTextColor, value);
            }
            tms[1].color = Color.Lerp(normalTextColor, optionsTextColor, value);

            if (isStared) { }
            image.color = Color.Lerp(normalItemColor, optionsItemColor, value);
            foreach (Image img in iconImages)
            {
                img.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, value);
            }
            if (socialImage)
            {
                socialImage.color = Color.Lerp(normalSocialColor, optionsSocialColor, value);
            }
        });

    }

    void DisableOptions()
    {
        var options = transform.Find(Names.OPTIONS);
        if (!options.gameObject.activeInHierarchy)
        {
            return;
        }
        var tms = GetComponentsInChildren<TextMeshProUGUI>();
        var socialImage = transform.Find(Names.MAIN).GetComponentInChildren<Image>();
        LeanTween.value(0, 1f, 0.3f).setOnUpdate((value) =>
        {
            if (!isStared)
            {
                tms[0].color = Color.Lerp(optionsTextColor, normalTextColor, value);
            }
            tms[1].color = Color.Lerp(optionsTextColor, normalTextColor, value);

            image.color = Color.Lerp(optionsItemColor, normalItemColor, value);
            if (socialImage)
            {
                socialImage.color = Color.Lerp(optionsSocialColor, normalSocialColor, value);
            }
        });
        options.Find(Names.DELETE_PROMPT).gameObject.SetActive(false);
        options.gameObject.SetActive(false);
    }

}
