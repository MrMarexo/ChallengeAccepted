using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveItemData
{
    public string nameToShow;
    public string key;
    public SaveItemData(string nameToShow, string key)
    {
        this.nameToShow = nameToShow;
        this.key = key;
    }
}

public class SettingsData
{
    public bool socialToggle;
    public bool repeatToggle;
    public bool playerRepeatToggle;
    public bool repeatInTurnToggle;
    public bool freeGeneratingToggle;

    public SettingsData(bool socialToggle = false, bool repeatToggle = true, bool playerRepeatToggle = false, bool repeatInTurnToggle = false, bool freeGeneratingToggle = false)
    {
        this.socialToggle = socialToggle;
        this.repeatToggle = repeatToggle;
        this.playerRepeatToggle = playerRepeatToggle;
        this.repeatInTurnToggle = repeatInTurnToggle;
        this.freeGeneratingToggle = freeGeneratingToggle;
    }
}


public class SaveSystem : MonoBehaviour
{
    string selectedKey = "";
    bool newlistSelected;


    bool saveIsOpened = true;


    [SerializeField] Popup mainPopup;
    [SerializeField] GameObject listGO;

    [SerializeField] GameObject listItemPrefab;
    [SerializeField] GameObject newInputPrefab;

    SaveLoadButton button;
    MessageScript message;
    OptionsSave options;

    SearchSettings settings;

    List<SaveItemData> listOfSaveData = new List<SaveItemData>();

    private void Start()
    {
        listOfSaveData = Progress.LoadSaveItemData();
        PopulateList();
        button = mainPopup.GetComponentInChildren<SaveLoadButton>();
        message = mainPopup.GetComponentInChildren<MessageScript>();
        options = mainPopup.GetComponentInChildren<OptionsSave>();

        settings = GetComponent<SearchSettings>();

        //var richData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn15", true), new PlayerChallenge("#kn19", true), new PlayerChallenge("#kn36", true), new PlayerChallenge("#kn57", true), new PlayerChallenge("#kn118", true), new PlayerChallenge("#kn109", false) }, "Richiak");
        //var marexoData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn15", true), new PlayerChallenge("#kn14", true), new PlayerChallenge("#kn62", true), new PlayerChallenge("#kn97", true), new PlayerChallenge("#kn53", true) }, "Marexo");
        //var withRich = new List<PlayerData>() { richData, marexoData };
        //var key = listOfSaveData.Find((d) => d.nameToShow == "withRich").key;
        //Progress.SavePlayerData(withRich, GenerateSpecificKeyType(key, EKeyType.playerData));
        //Progress.SaveCurrentChallengeList(GenerateSpecificKeyType(key, EKeyType.list));

        //var michalData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn74", true), new PlayerChallenge("#kn50", true), }, "Michal");
        //var sethData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn113", true), new PlayerChallenge("#kn108", true), }, "Seth");
        //var andreData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn55", true), new PlayerChallenge("#kn2", true), }, "Andre");
        //var marexoData = new PlayerData(new List<PlayerChallenge>() { new PlayerChallenge("#kn48", true), new PlayerChallenge("#kn87", true), }, "Marexo");
        //var withGroup = new List<PlayerData>() { michalData, sethData, andreData, marexoData };
        //var key = listOfSaveData.Find((d) => d.nameToShow == "withGroup").key;
        //Progress.SavePlayerData(withGroup, GenerateSpecificKeyType(key, EKeyType.playerData));
        //Progress.SaveCurrentChallengeList(GenerateSpecificKeyType(key, EKeyType.list));
    }

    void PopulateList()
    {
        if (listGO.transform.childCount > 0)
        {
            foreach (Transform child in listGO.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (listOfSaveData.Count > 0)
        {
            foreach (SaveItemData data in listOfSaveData)
            {
                var item = Instantiate(listItemPrefab, listGO.transform);
                item.GetComponent<SaveItem>().Initiate(data);
            }
        }
        selectedKey = "";

    }


    void PrepareSavePopup()
    {
        var item = Instantiate(newInputPrefab, listGO.transform);
        button.ChangeButton("Save", false);
    }

    void PrepareLoadPopup()
    {
        button.ChangeButton("Load", false);
        if (listOfSaveData.Count == 0)
        {
            message.Appear("No saves yet");
            button.ChangeButton("Exit", false);
        }
    }

    public void OpenSavePopup()
    {
        mainPopup.OpenPopup();
        saveIsOpened = true;
        PrepareSavePopup();
    }

    public void OpenLoadPopup()
    {
        mainPopup.OpenPopup();
        saveIsOpened = false;
        PrepareLoadPopup();
    }


    public void DeleteConfirmation()
    {
        if (selectedKey == "")
        {
            message.Appear("You must choose a saved game to delete");
            return;
        }
        message.Appear("Are you sure you want to delete this save?", false);
        button.Hide();
        options.Show();
    }


    public void ReceiveAnswer(bool delete)
    {
        if (delete)
        {
            Delete();
        }
        options.Hide();
        button.Show();
        message.Disappear();

        void Delete()
        {
            if (selectedKey != "")
            {
                var deleteData = listOfSaveData.Find((d) => d.key == selectedKey);
                listOfSaveData.Remove(deleteData);
                Progress.RemoveSavedString(GenerateSpecificKeyType(selectedKey, EKeyType.list));
                Progress.RemoveSavedString(GenerateSpecificKeyType(selectedKey, EKeyType.playerData));
                PopulateList();
                if (saveIsOpened)
                {
                    PrepareSavePopup();
                }
                else
                {
                    PrepareLoadPopup();
                }
            }
            else
            {
                message.Appear("You must choose a saved game to delete");
            }

        }
    }


    //on button
    public void LoadOrSave()
    {
        if (listOfSaveData.Count == 0 && !saveIsOpened)
        {
            JustClose();
            return;
        }

        if (selectedKey == "")
        {
            message.Appear("You must choose a saved game or create a new one");
            return;
        }

        StartCoroutine(Cleanup());
        if (saveIsOpened)
        {
            if (newlistSelected)
            {
                var name = selectedKey;
                selectedKey = GenerateKeyFromName(name);
                listOfSaveData.Add(new SaveItemData(name, selectedKey));
                Progress.SaveNewSaveItemData(listOfSaveData);
            }
            SaveCurrentGroup(selectedKey);
        }
        else
        {
            LoadGroup(selectedKey);
        }

        IEnumerator Cleanup()
        {
            Debug.Log("cleanup runs");
            yield return new WaitForSecondsRealtime(0.5f);
            if (saveIsOpened)
            {
                listGO.GetComponentInChildren<AddNewInput>().Deselect();
            }
            message.StopWaitAndDisappear();
            mainPopup.GetComponent<Popup>().ClosePopup();
            PopulateList();
            options.Hide();
        }
    }

    public void JustClose()
    {
        mainPopup.GetComponent<Popup>().ClosePopup();
        StartCoroutine(Cleanup());
        options.Hide();

        IEnumerator Cleanup()
        {
            yield return new WaitForSecondsRealtime(0.3f);
            message.StopWaitAndDisappear();
            message.Disappear();
            PopulateList();
        }
    }

    public void SaveCurrentGroup(string key)
    {
        var pdKey = GenerateSpecificKeyType(key, EKeyType.playerData);
        var listKey = GenerateSpecificKeyType(key, EKeyType.list);
        var setKey = GenerateSpecificKeyType(key, EKeyType.settings);
        var listOfData = new List<PlayerData>();
        var settingsData = new SettingsData(socialToggle: settings.SocialToggle,
            repeatToggle: settings.RepeatToggle, playerRepeatToggle: settings.PlayerRepeatToggle, repeatInTurnToggle: settings.RepeatInTurnToggle,
            freeGeneratingToggle: settings.FreeGeneratingToggle);
        foreach (Player p in PlayerList.playerList)
        {
            listOfData.Add(p.playerData);
        }
        Progress.SavePlayerData(listOfData, pdKey);
        Progress.SaveCurrentChallengeList(listKey);
        Progress.SaveSearchSettings(settingsData, setKey);
    }

    public void LoadGroup(string key)
    {
        var pdKey = GenerateSpecificKeyType(key, EKeyType.playerData);
        var listKey = GenerateSpecificKeyType(key, EKeyType.list);
        var setKey = GenerateSpecificKeyType(key, EKeyType.settings);

        var loadedPlayerList = Progress.LoadPlayerData(pdKey);
        Debug.Log(loadedPlayerList.Count);
        var loadedChallengeList = Progress.LoadChallengeList(listKey);
        ChallengeList.challengeList = loadedChallengeList;
        GetComponent<ChallengeList>().PopulateVisualList();
        GetComponent<PlayerList>().LoadPlayers(loadedPlayerList);
        var loadedSettings = Progress.LoadSearchSettings(setKey);
        SetSettings();

        void SetSettings()
        {
            settings.SocialToggle = loadedSettings.socialToggle;
            settings.RepeatToggle = loadedSettings.repeatToggle;
            settings.PlayerRepeatToggle = loadedSettings.playerRepeatToggle;
            settings.RepeatInTurnToggle = loadedSettings.repeatInTurnToggle;
            settings.FreeGeneratingToggle = loadedSettings.freeGeneratingToggle;
        }
    }

    public void SelectThisKey(string key)
    {
        foreach (Transform child in listGO.transform)
        {
            var si = child.GetComponent<SaveItem>();
            if (si)
            {
                if (child.GetComponent<SaveItem>().data.key != key)
                {
                    child.GetComponent<SaveItem>().Deselect();
                }
                else
                {
                    selectedKey = key;
                    if (saveIsOpened)
                    {
                        button.ChangeButton("Overwrite");
                    }
                    else
                    {
                        button.ChangeButton("Load");
                    }
                }
            }
            if (child.GetComponent<AddNewInput>())
            {
                child.GetComponent<AddNewInput>().Deselect();
                newlistSelected = false;
            }

        }
    }

    public void AddNewWasSelected()
    {
        selectedKey = "";
        newlistSelected = true;
        var listSI = listGO.GetComponentsInChildren<SaveItem>();
        foreach (SaveItem si in listSI)
        {
            si.Deselect();
        }
        button.ChangeButton("Save as new");
    }

    public void AddNewWasTyped(string name)
    {
        selectedKey = name;
    }

    string GenerateKeyFromName(string name)
    {
        var date = System.DateTime.Now.ToString();
        date = date.Replace(" ", "-");
        var key = name + date;
        return key;
    }

    string GenerateSpecificKeyType(string key, EKeyType type)
    {
        if (type == EKeyType.list)
        {
            return "li#" + key;
        }
        if (type == EKeyType.playerData)
        {
            return "pd#" + key;

        }
        return "se#" + key;
    }

    enum EKeyType
    {
        playerData,
        list,
        settings,
    }

}
