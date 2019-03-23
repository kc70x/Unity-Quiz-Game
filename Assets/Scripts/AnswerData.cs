using UnityEngine;
using System;

//Need serializable for access to class in the editor
[Serializable]
public class AnswerData 
{
	public string answerText;
	public bool isCorrect;
}