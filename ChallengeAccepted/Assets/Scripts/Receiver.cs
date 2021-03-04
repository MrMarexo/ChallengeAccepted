using UnityEngine;
using UnityEngine.EventSystems;


public class Receiver : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] EListReceiverType myListType;


    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponentInParent<ListItem>().ReceiveInfo(myListType);
    }

    public EListReceiverType GetInfoType()
    {
        return myListType;
    }
}
