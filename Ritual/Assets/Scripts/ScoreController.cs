using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public RoundController roundController;
	public Image playerBar;
	public Image opponentBar;
	public GameObject slider;

	private int gameScore;

	public void StartGame(){
		gameScore = Settings._.StartingScore;
		UpdateUI ();
	}
	
	public void AddScore( Constants.PlayerIndex playerIndex, int score){
		if (playerIndex == Constants.PlayerIndex.PLAYER_1) {
			gameScore += score;
		} else {
			gameScore -= score;
		}

		UpdateUI ();
		CheckVictoryConditions ();
	}

	private void UpdateUI(){
		var scoreOffset = Settings._.MaxScore - Settings._.MinScore;
		var currentScore = gameScore - Settings._.MinScore;

		var scoreRatio = (float)currentScore / (float)scoreOffset;
		playerBar.fillAmount = scoreRatio;
		opponentBar.fillAmount = 1 - scoreRatio;

		var sliderOffset = gameScore - Settings._.StartingScore;
		var sliderRatio = (float)sliderOffset / ((float)scoreOffset * 0.5f);
		var position = slider.transform.localPosition;
		position.x = sliderRatio * Constants.scoreWidth / 2f;
		slider.transform.localPosition = position;
	}

	private void CheckVictoryConditions(){
		if (gameScore <= Settings._.MinScore) {
			roundController.LoseGame ();
			return;
		}

		if (gameScore >= Settings._.MaxScore) {
			roundController.NextRound ();
			return;
		}
	}
}
