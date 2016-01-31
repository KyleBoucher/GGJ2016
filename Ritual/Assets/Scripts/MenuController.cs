using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VolumetricLines;

public class MenuController : MonoBehaviour
{
	public VolumetricLineBehavior line;

	public GameObject selecter;
	public GameObject play;

	private bool startdragging = false;
	private bool lockedPlay = false;

	public void OnClick(){
		startdragging = true;
		line.gameObject.SetActive(true);

		line.StartPos = selecter.transform.localPosition;
	}

	public void OnDragIn(){
		if (startdragging) {
			line.EndPos = play.transform.localPosition;
			lockedPlay = true;
		}
	}

	private void Update(){
		if (startdragging) {

			if (lockedPlay == false) {
				var UIMousePos = Input.mousePosition;
				UIMousePos.x = UIMousePos.x / Screen.width * Constants.screenWidth - Constants.screenWidth * 0.5f;
				UIMousePos.y = UIMousePos.y / Screen.height * Constants.screenHeight - Constants.screenHeight * 0.5f;
				line.EndPos = UIMousePos;
			}

			if (Input.GetMouseButton (0) == false) {
				line.gameObject.SetActive(false);

				if (lockedPlay) {
					SceneManager.LoadScene ("Game");
				} else {
					startdragging = false;
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}

