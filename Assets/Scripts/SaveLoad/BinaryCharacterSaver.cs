using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BinaryCharacterSaver : MonoBehaviour
{
    public Button checkpointButton; // Game Menu
    public Transform buttons;   // LevelSelect
    public Transform Checkpoints;

    // Data path
    const string folderName = "CharacterData";
	const string fileName = "CharacterData";
    const string fileExtension = ".txt";

    // Path store at startup
    private string filePath;

    void Start()
    {
        // file path
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        filePath = Path.Combine(folderPath, fileName + fileExtension);

        // Create empty save data when not exist
        // Case folder not exist
        if (!Directory.Exists (folderPath))
        {
            Directory.CreateDirectory (folderPath);
            CreateEmptyData();
        }
        // Case file not exist
        else
        {
            string[] filePaths = Directory.GetFiles (folderPath,  "*" + fileExtension);
            if (filePaths.Length == 0)
            {
                CreateEmptyData();
            }
        }
    }

    // --------------------------------------------------------------------- Game menu scene ---------------------------------------------------------------------
    public void LoadStartGameMenu()
    {
        CharacterData characterData = LoadCharacter ();

        CheckPoint checkpoint = characterData.checkpoint;
        if (checkpoint.stage != null)
        {
            // buttona active and path set
            string checkpointStage = checkpoint.stage;
            checkpointButton.onClick.AddListener(() => { SceneManager.LoadScene(checkpointStage); });
            checkpointButton.enabled = true;
        }
    }

    // --------------------------------------------------------------------- end Game menu scene ---------------------------------------------------------------------

    // --------------------------------------------------------------------- Level select scene ---------------------------------------------------------------------
    public void LoadLevelSelectMenu()
    {
        CharacterData characterData = LoadCharacter ();

        string stageProgress = characterData.stageProgress;
        string[] stageSplit = stageProgress.Split('-');
        int episode = int.Parse(stageSplit[0]);
        int level = int.Parse(stageSplit[1]);

        int loadedEpisode = 1;
        int loadedLevel = 1;

        while (!(loadedEpisode == episode && loadedLevel == level)) // e.g stageProgress = 1-2 stop at 1-2 after update 1-1
        {
            string loadedStage = loadedEpisode + "-" + loadedLevel;
            int score = characterData.scoreRecord[loadedStage];

            // Update score and unlock button
            GameObject button = buttons.GetChild(loadedLevel-1).gameObject;
            button.transform.GetChild(0).GetComponent<Text>().text = "" + score;    // load score
            button.transform.GetChild(1).GetComponent<Text>().text = loadedStage;    // load stage
            button.GetComponent<Button>().enabled = true;

            if (loadedLevel == 10)    // last level of episode
            {
                loadedEpisode += 1;
                loadedLevel = 1;
            }
            else
            {
                loadedLevel += 1;
            }
        }

        // New stage
        if (level != 10)    // not the last stage of the episode
        {
            // Update score and unlock button
            GameObject button = buttons.GetChild(loadedLevel-1).gameObject;
            button.transform.GetChild(0).GetComponent<Text>().text = "0";    // load score
            button.transform.GetChild(1).GetComponent<Text>().text = loadedEpisode + "-" + loadedLevel;    // load stage
            button.GetComponent<Button>().enabled = true;
        }

        // unlock latest stage
        buttons.GetChild(loadedLevel-1).GetComponent<Button>().enabled = true;
    }
    // --------------------------------------------------------------------- end Level select scene ---------------------------------------------------------------------

    // --------------------------------------------------------------------- game scene ---------------------------------------------------------------------

    public void Checkpoint(int checkpointId, Bag bag)
    {        
        // Load data
        CharacterData characterData = LoadCharacter();

        // Get checkpoint info        
        // stage
        string stage = SceneManager.GetActiveScene().name;
        // time
        TimeGUI timeGUI = GameObject.FindObjectOfType(typeof(TimeGUI)) as TimeGUI;
        float time = timeGUI.GetTime();        
        // key, jewellery
	    List<ItemColor> keys = bag.GetAllKeys();
	    Dictionary<ItemColor, int> jewellarys = bag.GetAllJewellarys();

        // Store to character data
        CheckPoint checkpoint = new CheckPoint();

        checkpoint.stage = stage;
        checkpoint.checkPointId = checkpointId;
        checkpoint.time = time;
        checkpoint.keys = keys;
        checkpoint.jewellarys = jewellarys;

        characterData.checkpoint = checkpoint;

        // Save
        SaveCharacter(characterData);
    }
    
    public void LoadCheckpoint()
    {
        CharacterData characterData = LoadCharacter ();

        // Get checkpoint info
        CheckPoint checkpoint = characterData.checkpoint;

        // have checkpoint     
        if (checkpoint.stage != null)
        {
            float time = checkpoint.time;

            // Set character data
            // time
            TimeGUI timeGUI = GameObject.FindObjectOfType(typeof(TimeGUI)) as TimeGUI;
            timeGUI.SetTime(time);        
            // key, jewellery
            List<ItemColor> keys = checkpoint.keys;
            Dictionary<ItemColor, int> jewellarys = checkpoint.jewellarys;
            Bag bag = GameObject.FindGameObjectWithTag("Player").GetComponent<Bag>();
            bag.AddList(keys);
            bag.AddList(jewellarys);        
            // checkpoint and position
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            int checkpointId = checkpoint.checkPointId;
            Vector2 startPosition = Checkpoints.GetChild(checkpointId).position;

            for (int index = 0; index <= checkpointId; index++) // remove previous checkpoint first
            {
                // Destroy(Checkpoints.GetChild(index).gameObject);
                Checkpoints.GetChild(index).gameObject.gameObject.SetActive(false);
            }

            // Also remove collected item
            Key[] sceneKeys = GameObject.FindObjectsOfType(typeof(Key)) as Key[];

            foreach (Key sceneKey in sceneKeys) // Loop by scene key
            {
                // destroy scene key gameObject if collected before
                if (keys.Contains(sceneKey.ItemColor))
                {
                    Destroy(sceneKey.gameObject);
                }
            }

            Jewellary[] sceneJewellarys = GameObject.FindObjectsOfType(typeof(Jewellary)) as Jewellary[];
            Goal[] goals = GameObject.FindObjectsOfType(typeof(Goal)) as Goal[];

            foreach (Jewellary sceneJewellary in sceneJewellarys) // Loop by scene key
            {
                // destroy scene key gameObject if collected before
                // And disable the red door
                ItemColor color = sceneJewellary.ItemColor;

                if (jewellarys.ContainsKey(sceneJewellary.ItemColor))
                {
                    Destroy(sceneJewellary.gameObject);

                    if (color == ItemColor.red) // Red door can't open again
                    {
                        foreach (Goal goal in goals)
                        {
                            if (goal.redKey)
                            {
                                goal.DisableDoor(false);

                                break;  // break search red door
                            }
                        }
                    }
                }
            }
            
            // Load player to checkpoint position
            player.position = startPosition;

            // Transition for player ready
            // StartCoroutine(LoadCheckpointTransition());
        }
    }

    // private IEnumerator LoadCheckpointTransition()
    // {
    //     // stop all object
    //     Time.timeScale = 0;

    //     // count time
    //     float countdown = 3.0f;
    //     float period = 0.1f;
        
    //     while(countdown >= 0)
    //     {
    //         // update time
    //         countdown -= period;
    //         Debug.Log(countdown);
    //         // update gui

    //         // wait
    //         yield return new WaitForSecondsRealtime(period);
    //     }

    //     // resume
    //     Time.timeScale = 1;
    // }

    public void ClearCheckpoint()
    {
        CharacterData characterData = LoadCharacter ();
        characterData.checkpoint = new CheckPoint();
        SaveCharacter(characterData);
    }

    private string GetNextStage(string stage)
    {
        string[] stageSplit = stage.Split('-');
        int episode = int.Parse(stageSplit[0]);
        int level = int.Parse(stageSplit[1]);

        if (level == 10)    // last level of episode
        {
            episode += 1;
            level = 1;
        }
        else
        {
            level += 1;
        }

        return episode + "-" + level;
    }

    public void StageClear(int score)
    {
        // Load data
        CharacterData characterData = LoadCharacter();

        // Latest stage
        string stage = SceneManager.GetActiveScene().name;
        string stageProgress = characterData.stageProgress;
        if (stage == stageProgress)
        {
            // Update stage progress
            // Debug.Log("Unlock next stage");
            string nextStage = GetNextStage(stage);
            characterData.stageProgress = nextStage;

            // Update score record directly
            // Debug.Log("New record");
            characterData.scoreRecord[stage] = score;
        }
        // Clear stage alreadly
        else
        {
            // Update score record if larger record
            int record = characterData.scoreRecord[stage];
            // Debug.Log(record);
            if (score > record)
            {
                // Debug.Log("New record");
                characterData.scoreRecord[stage] = score;
            }
        }

        // Clear check point
        characterData.checkpoint = new CheckPoint();

        // Save
        SaveCharacter(characterData);
    }
    // --------------------------------------------------------------------- end game scene ---------------------------------------------------------------------

    void CreateEmptyData()
    {
        // Empty data
        CharacterData characterData = new CharacterData();

        characterData.stageProgress = "1-1";
        characterData.scoreRecord = new System.Collections.Generic.Dictionary<string, int>();
        characterData.checkpoint = new CheckPoint();

        // Saving data
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        string dataPath = Path.Combine(folderPath, fileName + fileExtension);
        SaveCharacter(characterData);
    }
    void SaveCharacter (CharacterData data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open (filePath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize (fileStream, data);
        }
    }

    CharacterData LoadCharacter ()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open (filePath, FileMode.Open))
        {
            return (CharacterData)binaryFormatter.Deserialize (fileStream);
        }
    }

    // string[] GetFilePaths ()
    // {
    //     string folderPath = Path.Combine(Application.persistentDataPath, folderName);
    //     return Directory.GetFiles (folderPath,  "*" + fileExtension);
    // }

    // void Update ()
    // {
    //     if (Input.GetKeyDown (KeyCode.S))
    //     {
    //         string folderPath = Path.Combine(Application.persistentDataPath, folderName);
    //         if (!Directory.Exists (folderPath))
    //             Directory.CreateDirectory (folderPath);            

    //         string dataPath = Path.Combine(folderPath, fileName + fileExtension);       
    //         SaveCharacter (characterData, dataPath);
    //     }

    //     if (Input.GetKeyDown (KeyCode.L))
    //     {
    //         string[] filePaths = GetFilePaths ();
	// 		Debug.Log(filePaths.Length);
            
    //         if(filePaths.Length > 0)
    //             characterData = LoadCharacter (filePaths[0]);
    //     }
    // }

}