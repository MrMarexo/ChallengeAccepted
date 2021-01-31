using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Delete : MonoBehaviour, IPointerDownHandler
{
    int deleteIndex;

    public void SetIndex(int index)
    {
        deleteIndex = index;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log(gameObject.name + " Was Clicked.");
        FindObjectOfType<ChallengeList>().DeleteChallenge(gameObject.transform.parent.parent.gameObject, deleteIndex);
    }

    
}
