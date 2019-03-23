using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

	public Text answerText;//Text to display
	private GameController gameController;//reference to game controller

	private AnswerData answerData;//store the answer instance


	// Use this for initialization
	void Start () 
	{
		gameController = FindObjectOfType<GameController>(); // find the game controller
	}

	public void Setup(AnswerData data)//pass in answer data and set up for display
	{
		answerData = data;
		answerText.text = answerData.answerText;
	}


	void Update()
	{

	}

	//handle the button click and send if the answer was correct to the game controller
	public void HandleClick()
	{
		gameController.AnswerButtonClicked(answerData.isCorrect);
	}

}
