using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VolumetricLines;

public class LineController : MonoBehaviour {

	public Canvas canvas;

	public VolumetricLineBehavior[] lines;

	private int lineCount = 0;

	// Use this for initialization
	void Start () {

		var scale = canvas.transform.localScale;
		scale.z = 1f;
		transform.localScale = scale;

		ResetLines ();
	}

	private void Update(){
		if (gameObject.activeSelf) {
			lines[lineCount].EndPos = GetMousePos();
		}
	}

	public void AddPoint(Vector3 point){
		gameObject.SetActive (true);

		if (lineCount >= 0) {
			lines [lineCount].EndPos = point;
		}

		lineCount++;

		point.z = 0;
		lines[lineCount].StartPos = point;
		lines[lineCount].EndPos = GetMousePos();
		lines[lineCount].gameObject.SetActive(true);

	}

	public void ResetLines(){
		lineCount = -1;
		//reset all lines

		foreach (var line in lines) {
			line.StartPos = -Vector3.one * float.MaxValue;
			line.EndPos = Vector3.one * float.MaxValue;
			line.gameObject.SetActive(false);
		}

		gameObject.SetActive (false);
	}

	private Vector3 GetMousePos(){
		var UIMousePos = Input.mousePosition;
		UIMousePos.x = UIMousePos.x/Screen.width * Constants.screenWidth - Constants.screenWidth*0.5f;
		UIMousePos.y = UIMousePos.y/Screen.height * Constants.screenHeight - Constants.screenHeight*0.5f;

		return UIMousePos;
	}
}