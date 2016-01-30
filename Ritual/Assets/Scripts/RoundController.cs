using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RoundController : MonoBehaviour {

	public ScoreController scoreController;
	public SymbolController symbolController;
	public AIController aiController;
	public Text roundNumber;
	public GameObject loseScreen;

	private int currentRound = 0;

	private void Start(){
		StartGame ();
	}

	public void StartGame(){
		currentRound = 0;
		loseScreen.SetActive(false);
		NextRound ();
	}

	public void NextRound(){
		currentRound++;
		scoreController.StartGame ();

		roundNumber.text = currentRound.ToString();
		roundNumber.gameObject.SetActive (true);

		//stop all controls and fizzle all spells, active and future
		symbolController.StopCooldowns();
		aiController.Stop();

		//show ui stuff
	}

	public void BeginRound(){
		aiController.Begin();

		roundNumber.gameObject.SetActive (false);
	}

	public void LoseGame(){
		//lose the game
		aiController.Stop();

		//show ui stuff
		loseScreen.SetActive(true);
	}

	public void QuitGame(){
		SceneManager.LoadScene ("Menu");
	}
}
