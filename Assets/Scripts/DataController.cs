using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;//Load scenes
using System.IO;

public class DataController : MonoBehaviour 
{
	private RoundData[] allRoundData;//hold data for each round
    private PlayerProgress playerProgress;
    private string gameDataFileName = "data.json";

    // Use this for initialization
    void Start ()  
	{
		DontDestroyOnLoad(gameObject);//when load new scenes this data will persist

        LoadGameData();

        LoadPlayerProgress();

        SceneManager.LoadScene ("MenuScreen");
	}

	public RoundData GetCurrentRoundData()//returns round one data..we only have one round
	{
		return allRoundData[playerProgress.currentRound];
	}


    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }

        if (PlayerPrefs.HasKey("currentRound"))
        {
            playerProgress.currentRound = PlayerPrefs.GetInt("currentRound");
        }
    }


    private void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }

    private void SaveCurrentRound()
    {
        // Save the value playerProgress.currentRound to PlayerPrefs, with a key of "currentRound"
        PlayerPrefs.SetInt("currentRound", playerProgress.currentRound);
    }

    public void ResetCurrentRound()
    {
        //set current round back to zero aka the start of the rounds and save to player prefs
        playerProgress.currentRound = 0;
        SaveCurrentRound();
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public bool HasMoreRounds()
    {
        return (allRoundData.Length - 1 > playerProgress.currentRound);
    }

    public void GetNextRound()
    {
        //If we have more rounds increment current round 
        if (HasMoreRounds())
        {
            playerProgress.currentRound++;

            //save current round to player prefs so we can load at this round
            SaveCurrentRound();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    // Update is called once per frame
    void Update () {

	}
}