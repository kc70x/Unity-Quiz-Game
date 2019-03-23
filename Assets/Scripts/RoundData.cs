using UnityEngine;
using System.Collections;

[System.Serializable]
public class RoundData 
{
	public string name;//name of the round
	public int timeLimitInSeconds;//time available for the round
	public int pointsAddedForCorrectAnswer;//points per correct answer
	public QuestionData[] questions;//list of questions for the round

}