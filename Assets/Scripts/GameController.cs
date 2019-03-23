using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour {


	public Text questionDisplayText;
	public Text scoreDisplayText;
	public Text timeRemainingDisplayText;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;
	public GameObject questionDisplay;
	public GameObject roundEndDisplay;
    public GameObject nextRoundDisplay;
    public Text highScoreDisplay;

    private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private float timeRemaining;
	private int questionIndex;
	private int playerScore;
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();

	// Use this for initialization
	void Start () 
	{
		dataController = FindObjectOfType<DataController>();//we can use find because we start with the Persistent scene which has a data controller
        SetUpRound();
    }

    public void SetUpRound()
    {
        //Once dataController is loaded initialize game variables with the data found
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;
        timeRemaining = currentRoundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();

        //initialize remaining game data and show first question
        playerScore = 0;
        questionIndex = 0;

        ShowPlayerScore();
        ShowQuestion();
        isRoundActive = true;
    }

    private void ShowPlayerScore()
    {
        scoreDisplayText.text = "Score: " + playerScore.ToString();
    }

    private void ShowQuestion()
	{   //Remove old answers get current question and display the text
		RemoveAnswerButtons();
		QuestionData questionData = questionPool[questionIndex];
		questionDisplayText.text = questionData.questionText;

		//Get all answers for the question, create new buttons for each and add them to the answerButtonParent object(AnswerPanel) 
		for (int i = 0; i < questionData.answers.Length; i++) 
		{
			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
			answerButtonGameObjects.Add(answerButtonGameObject);
			answerButtonGameObject.transform.SetParent(answerButtonParent);

			//we get a reference to the answer button then use its attatched script to set the answer
			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();//Give the gameobject an answer button component
			answerButton.Setup(questionData.answers[i]);
		}
	}

	private void RemoveAnswerButtons()
	{//if answer buttons exists remove them , remove the game object and add it to the available object pool
		while (answerButtonGameObjects.Count > 0) 
		{
			answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
			answerButtonGameObjects.RemoveAt(0);
		}
	}

	public void AnswerButtonClicked(bool isCorrect)
	{
		//Increase player score if answer is correct and update the display
		if (isCorrect) 
		{
			playerScore += currentRoundData.pointsAddedForCorrectAnswer;
			scoreDisplayText.text = "Score: " + playerScore.ToString();
		}

		//If we have more questions show the next question otherwise end the round
		if (questionPool.Length > questionIndex + 1) {
			questionIndex++;
			ShowQuestion();
		} else 
		{
			EndRound();
		}

	}

	public void EndRound()
	{   // set the round over and turn off question display then turn on round end display panel
		isRoundActive = false;

        dataController.SubmitNewPlayerScore(playerScore);
        highScoreDisplay.text = dataController.GetHighestPlayerScore().ToString();

        questionDisplay.SetActive (false);
		roundEndDisplay.SetActive (true);

        if (dataController.HasMoreRounds())
        {
            nextRoundDisplay.SetActive(true);
        }
        else
        {
            nextRoundDisplay.SetActive(false);
        }
    }

    public void GoToNextRound()
    {
        //Move to the next round
        dataController.GetNextRound();

        //Reset variables for the new round
        SetUpRound();

        //show questions again
        questionDisplay.SetActive(true);
        roundEndDisplay.SetActive(false);

    }

    public void ReturnToMenu()
	{
        dataController.ResetCurrentRound();
        SceneManager.LoadScene("MenuScreen");
	}

	private void UpdateTimeRemainingDisplay()
	{
		timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();
	}

	// Update is called once per frame
	void Update () 
	{
		//if round is active decrement time remaining and update display
		if (isRoundActive) 
		{
			timeRemaining -= Time.deltaTime;
			UpdateTimeRemainingDisplay();

			//if time is 0 or less end round
			if (timeRemaining <= 0f)
			{
				EndRound();
			}

		}
	}
}