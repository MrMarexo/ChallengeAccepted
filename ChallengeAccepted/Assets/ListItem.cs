using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ListItem : MonoBehaviour, IPointerDownHandler
{
    int listItemIndex;

    public void SetIndex(int index)
    {
        listItemIndex = index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " Was Clicked.");
        FindObjectOfType<ChallengeList>().AddOptions(gameObject.transform.parent.parent.gameObject, listItemIndex);
    }
}
