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
	public GameObject lerperPrefab;
	public Outline outline;

	private SymbolController controller;
	private float cooldownTime = 0;
	private Vector3 origPos;
	private Lerper lerper;

	public void SetController( SymbolController controller){
		this.controller = controller;
	}

	public void OnMouseDown() {
		if(isCooldown) {
			return;
		}

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

	public void StopCooldown(){
		if (lerper != null) {
			lerper.Stop ();
			lerper = null;
		}

		isCooldown = false;

		StopCoroutine ("Cooldown");

		outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 1f);
	}

	public void StartCooldown(float cooldown) {
		cooldownTime = cooldown;
		StartCoroutine("Cooldown");
	}

	IEnumerator Cooldown() {
		isCooldown = true;
		outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 0f);

		if (lerper == null) {
			CreateLerper ();
		}

		var img = gameObject.GetComponentsInChildren<Image>()[1];
		lerper.Init (0f, 1f, cooldownTime, img);

		yield return new WaitForSeconds(cooldownTime);

		outline.effectColor = new Color(outline.effectColor.r, outline.effectColor.g, outline.effectColor.b, 1f);
		isCooldown = false;
	}

	private void CreateLerper(){
		lerper = (Instantiate(lerperPrefab) as GameObject).GetComponent<Lerper>();
	}
}



















