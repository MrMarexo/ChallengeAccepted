using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerToList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] bool answer = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " Was Clicked.");
        FindObjectOfType<ChallengeList>().Answer(answer);
    }
}
