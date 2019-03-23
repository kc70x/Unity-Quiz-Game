using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour {

	public void StartGame()//custom function to be called by start button
	{
		SceneManager.LoadScene("Game");
	}
}