using UnityEngine;
using UnityEngine.EventSystems;

public class Receiver : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ReceiverType myType;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponentInParent<ListItem>().ReceiveInfo(myType);

    }

    public ReceiverType GetInfoType()
    {
        return myType;
    }
}
