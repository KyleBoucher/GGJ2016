using UnityEngine;
using System.Collections;

public class RoundController : MonoBehaviour {

	public ScoreController scoreController;

	private int currentRound = 0;

	private void Start(){
		StartGame ();
	}

	public void StartGame(){
		currentRound = 0;
		NextRound ();
		scoreController.StartGame ();
	}

	public void NextRound(){
		currentRound++;

		//show ui stuff
	}

	public void LoseGame(){
		//lose the game

		//show ui stuff
	}
}
