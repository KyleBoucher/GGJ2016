using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Lerper : MonoBehaviour {
	private float endValue;
	private float length;
	private float timer;
	private float perSec;
	private Image theImg;

	public void Init(float from, float to, float time, Image img) {
		endValue = to;
		timer = 0f;
		length = time;
		perSec = (to-from)/time;

		theImg = img;
		theImg.fillAmount = from;
	}

	void Update() {
		timer += Time.deltaTime;

		theImg.fillAmount += perSec*Time.deltaTime;

		if(timer >= length) {
			Stop ();
		}
	}

	public void Stop(){
		theImg.fillAmount = endValue;
		Destroy(this.gameObject);
	}

}
