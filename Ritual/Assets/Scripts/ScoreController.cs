using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public RoundController roundController;
	public Image playerBar;
	public Image opponentBar;
	public GameObject slider;

	private int playerOneScore;
	private int playerTwoScore;

	public void StartGame(){
		playerOneScore = Settings._.StartingScore;
		playerTwoScore = Settings._.StartingScore;
		UpdateUI ();
	}
	
	public void AddScore( Constants.PlayerIndex playerIndex, int score){
		if (playerIndex == Constants.PlayerIndex.PLAYER_1) {
			playerOneScore += score;
		} else {
			playerTwoScore += score;
		}

		UpdateUI ();
		CheckVictoryConditions ();

		Debug.Log ("" + playerIndex + " : " + score);
	}

	private void UpdateUI(){
		var scoreOffset = Settings._.MaxScore - Settings._.MinScore;
		//var currentScore = playerOneScore - Settings._.MinScore;

		var scoreRatio = (float)playerOneScore / (float)scoreOffset;
		scoreRatio = Mathf.Clamp (scoreRatio, 0f, 1f);
		playerBar.fillAmount = scoreRatio;

		scoreRatio = (float)playerTwoScore / (float)scoreOffset;
		scoreRatio = Mathf.Clamp (scoreRatio, 0f, 1f);
		opponentBar.fillAmount = scoreRatio;

		/*var sliderOffset = gameScore - Settings._.StartingScore;
		var sliderRatio = (float)sliderOffset / ((float)scoreOffset * 0.5f);
		sliderRatio = Mathf.Clamp (sliderRatio, -1f, 1f);
		var position = slider.transform.localPosition;
		position.x = sliderRatio * Constants.scoreWidth / 2f;
		slider.transform.localPosition = position;*/
	}

	private void CheckVictoryConditions(){
		if (playerTwoScore >= Settings._.MaxScore) {
			roundController.LoseGame ();
			return;
		}

		if (playerOneScore >= Settings._.MaxScore) {
			roundController.NextRound ();
			return;
		}
	}
}
