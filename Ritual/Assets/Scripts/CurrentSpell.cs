using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSpell : MonoBehaviour
{
	public GameObject rowOne;
	public GameObject rowTwo;
	public GameObject unused;

	public Text multiplier;

	float duration = 0.25f;
	float smoothness = 0.02f;
	float startScale = 2.5f;
	Color startColor = Color.white;
	Color targetColor = Color.white;

	public void SetCurrentSpell( string spell ){
		ResetRows ();

		int index = 0;
		foreach (var c in spell) {
			var g = unused.transform.Find ("Spell_" + c);
			if (g != null) {
				if (index < 6) {
					g.transform.SetParent (rowOne.transform);
				} else {
					g.transform.SetParent (rowTwo.transform);
				}
				index++;
			}
		}
	}

	public void SetMultiplier( float multiplier ){
		this.multiplier.text = string.Format ("x{0}", multiplier.ToString ("n0"));
		StopCoroutine ("LerpColorAndScale");
		startColor = Color.yellow;
		targetColor = Color.white;
		StartCoroutine("LerpColorAndScale");
	}

	public void ResetMultiplier( bool useColorAndScale ){
		this.multiplier.text = "x1";
		StopCoroutine ("LerpColorAndScale");
		if (useColorAndScale) {
			startColor = Color.red;
			targetColor = Color.white;
			StartCoroutine ("LerpColorAndScale");
		} else {
			this.multiplier.color = Color.white;
			this.multiplier.transform.localScale = Vector3.one;
		}
	}

	private IEnumerator LerpColorAndScale()
	{
		float progress = 0;
		float increment = smoothness/duration;
		while(progress < 1)
		{
			this.multiplier.color = Color.Lerp(startColor, targetColor, progress);
			this.multiplier.transform.localScale = Vector3.one * Mathf.Lerp(startScale, 1f, progress);
			progress += increment;
			yield return new WaitForSeconds(smoothness);
		}
	}

	public void UpdateCurrentSpell( string currentSpell ){
		int index = 0;
		foreach (var c in currentSpell) {
			var parent = rowOne;
			if (index >= 6) {
				parent = rowTwo;
			}
			var g = parent.transform.Find ("Spell_" + c);
			if (g != null) {
				var image = g.GetComponent<Image> ();
				image.color = Color.red;
			}
			index++;
		}
	} 

	private void ResetRows(){
		foreach (var child in rowOne.GetComponentsInChildren<Image>()) {
			child.transform.SetParent (unused.transform);
			child.color = Color.white;
		}

		foreach (var child in rowTwo.GetComponentsInChildren<Image>()) {
			child.transform.SetParent (unused.transform);
			child.color = Color.white;
		}
	}
}

