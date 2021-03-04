using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AddNewInput : MonoBehaviour, IPointerDownHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI placeholder;
    [SerializeField] TMP_InputField input;

    string placeholderText = "+ Add new";

    private void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        placeholder.text = "";
        FindObjectOfType<SaveSystem>().AddNewWasSelected();
        AddListenerOnChange();
    }

    public void OnDeselect(BaseEventData data)
    {
        if (!data.selectedObject.GetComponent<SaveItem>() && input.text == "")
        {
            Deselect();
        }
    }

    public void Deselect()
    {
        RemoveListener();
        input.text = "";
        placeholder.text = placeholderText;
    }

    void AddListenerOnChange()
    {
        input.onEndEdit.AddListener(Submit);

        void Submit(string arg)
        {
            FindObjectOfType<SaveSystem>().AddNewWasTyped(arg);
        }
    }

    void RemoveListener()
    {
        input.onValueChanged.RemoveAllListeners();
    }
}
