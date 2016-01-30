using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Symbol : MonoBehaviour
{
	public char mySymbol;
	public bool isCooldown = false;

	private SymbolController controller;
	private float cooldownTime = 0;
	private Vector3 origPos;

	public void SetController( SymbolController controller){
		this.controller = controller;
	}

	public void OnMouseDown() {
		// start of spell
		controller.StartSpell(mySymbol);
	}
	public void OnMouseUp() {
		// cast spell if one exists
		controller.EndSpell();
	}
	public void OnMouseEnter() {
		if(isCooldown) {
			return;
		}
		// add to spell
		controller.AddToSpell(mySymbol);
	}
	public void OnMouseExit() {
		// probably don't need this
	}

	public void Shake() {
		if(origPos != Vector3.zero) {
			transform.localPosition = origPos;
		}
		//do shake
		StopCoroutine("DoShake");
		StartCoroutine("DoShake");
	}

	IEnumerator DoShake() {
		var timer = 0f;
		origPos = transform.localPosition;
		Vector2 randPos;
		while(true) {
			timer += Time.deltaTime;
			randPos = Random.insideUnitCircle*Settings._.FizzleShakeAmount;
			transform.localPosition = origPos + new Vector3(randPos.x, 0, 0);

			if(timer >= Settings._.FizzleShakeTime) {
				transform.localPosition = origPos;
				break;
			}
			yield return null;
		}

		origPos = Vector3.zero;
	}

	public void StartCooldown(float time) {
		cooldownTime = time;
		StopCoroutine("Cooldown");
		StartCoroutine("Cooldown");
	}

	IEnumerator Cooldown() {
		isCooldown = true;
		var img = gameObject.GetComponent<Image>();
		var timer = 0f;
		while(true) {
			timer += Time.deltaTime;
			img.CrossFadeColor(Color.Lerp(img.color, Color.white, Time.deltaTime), cooldownTime, true, false);

			if(timer >= cooldownTime) {
				break;
			}
			yield return null;
		}
		isCooldown = false;
	}
}



















