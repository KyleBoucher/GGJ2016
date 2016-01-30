using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SymbolController : MonoBehaviour {

	public SpellSearch spellSearch;
	public GameObject symbolPrefab;
	public GameObject parentPrefab;
	public List<Sprite> sprites = new List<Sprite>();
	public LineController lineController;

	//private List<Symbol> symbols = new List<Symbol>();
	private int symbolCount = 12;
	private float circleWidth = Constants.screenWidth * 0.5f - Constants.symbolWidth*2f;

	public string currentSpell = "";

	// Use this for initialization
	void Start () {
		//generate symbol list
		var maxAngle = 360f / symbolCount;
		for (var i = 0; i < symbolCount; i++) {
			var symbolObject = Instantiate(symbolPrefab) as GameObject;
			var symbol = symbolObject.GetComponent<Symbol>();
			if(symbol != null ){
				symbol.SetController(this);
			}
			symbolObject.transform.SetParent(parentPrefab.transform);
			symbolObject.transform.localScale = Vector3.one;

			//rotate and position symbol into position
			var newPos = new Vector3();
			var angle = maxAngle*i*Mathf.Deg2Rad;
			newPos.x = circleWidth*Mathf.Sin(angle);
			newPos.y = circleWidth*Mathf.Cos(angle);

			symbolObject.transform.localPosition = newPos;

			// set render image
			var symbolImage = symbolObject.GetComponent<Image>();
			if(symbolImage != null) {
				symbolImage.sprite = sprites[i];
			}

			// set spell symbol
			symbol.mySymbol = spellSearch.Symbols[i];
		}
	}

	private GameObject GetObjectBySymbol(char symbol) {
		var ind = spellSearch.Symbols.BinarySearch(symbol);
		if(ind != -1) {
			var child = parentPrefab.transform.GetChild(ind);
			return child.gameObject;
		}

		return null;
	}
	
	public void StartSpell(char symbol) {
		currentSpell = "" + symbol;

		//add to line renderer
		lineController.AddPoint (GetObjectBySymbol (symbol).transform.localPosition);

		HandleSpellSearch(symbol);
	}
	public void EndSpell() {
		List<string> spells = spellSearch.SearchSpells(currentSpell);
		bool isSpell = false;
		foreach(string spell in spells) {
			// this is the completed spell
			if(spell.Length == currentSpell.Length 
			   && spell.Equals(currentSpell)) {
				isSpell = true;
			}
		}

		if(isSpell) {
			CastSpell();
		} else {
			FizzleSpell();
		}

		currentSpell = "";
		ResetHighlights();
	}
	public void AddToSpell(char symbol) {
		// validate "symbol"
		if(currentSpell.Length == 0 || currentSpell.Contains("" + symbol)) {
			return;
		}

		//add to line renderer
		lineController.AddPoint (GetObjectBySymbol (symbol).transform.localPosition);

		currentSpell += symbol;

		HandleSpellSearch(symbol);
	}

	private void ResetHighlights() {
		for(int i = 0; i < parentPrefab.transform.childCount; ++i) {
			var child = parentPrefab.transform.GetChild(i);
			var o = child.GetComponent<Outline>();
			if(o != null) {
				o.enabled = false;
			}
		}
	}

	private void ResetDimmedSymbols(bool fizzle) {
		for(int i = 0; i < parentPrefab.transform.childCount; ++i) {
			var child = parentPrefab.transform.GetChild(i);
			var img = child.GetComponent<Image>();
			if(img != null) {
				img.CrossFadeColor(Color.white, Settings._.SymbolFade, true, false);
			}
			var s = child.GetComponent<Symbol>();
			if(fizzle && currentSpell.Contains("" + s.mySymbol)) {
				s.Shake();
			}
		}
	}

	private void HandleSpellSearch(char lastSymbol) {
		// reset all highlights
		ResetHighlights();

		// dim Symbol
		int ind = spellSearch.Symbols.BinarySearch(lastSymbol);
		if(ind != -1) {
			var ch = parentPrefab.transform.GetChild(ind);
			var img = ch.GetComponent<Image>();
			img.CrossFadeColor(Color.grey, Settings._.SymbolFade, false, false);
		}

		// search with "currentSpell"
		List<string> spells = spellSearch.SearchSpells(currentSpell);
		if(spells.Count == 0) {
			// no available spell
			FizzleSpell();
		}

		Outline outline = null;
		GameObject obj = null;
		foreach(string spell in spells) {
			// this is a spell with only one more symbol
			if(spell.Length == currentSpell.Length+1){
				obj = GetObjectBySymbol(spell[currentSpell.Length]);
				outline = obj.GetComponent<Outline>();
				outline.effectColor = Color.magenta;
			}
			// else a few more symbols to go
			else if(spell.Length > currentSpell.Length+1){
				obj = GetObjectBySymbol(spell[currentSpell.Length]);
				outline = obj.GetComponent<Outline>();
				outline.effectColor = Color.red;
			}

			if(outline != null) {
				outline.enabled = true;
			}
		}
	}

	public void FizzleSpell() {
		Debug.Log("Spell Fizzled");
		ResetHighlights();
		ResetDimmedSymbols(true);

		currentSpell = "";

		//clear the line renderer
		lineController.ResetLines ();
	}

	public void CastSpell() {
		Debug.Log ("Spell cast: " + currentSpell);
		GameObject obj = null;
		foreach(char c in currentSpell) {
			obj = GetObjectBySymbol(c);
			Symbol s = obj.GetComponent<Symbol>();
			s.StartCooldown(1f);
		}
		
		//clear the line renderer
		lineController.ResetLines ();
	}


}




