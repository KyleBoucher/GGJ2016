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
	public GameObject lerperPrefab;
	public ScoreController scoreController;
	public Constants.PlayerIndex activePlayer;
	public RoundController roundController;
	public List<char> SpecialSymbols = new List<char>();

	public Transform parentTransform;

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
				var childImg = symbol.GetComponentsInChildren<Image>()[1];
				childImg.sprite = sprites[i];
				childImg.fillAmount = 1f;
			}

			// set spell symbol
			symbol.mySymbol = spellSearch.Symbols[i];
		}

		spellSearch.GenerateNewSpell();
	}

	public GameObject GetObjectBySymbol(char symbol) {
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
		lineController.AddPoint (GetObjectBySymbol (symbol).transform.localPosition + parentTransform.localPosition);
		roundController.BeginRound ();

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
//		if(currentSpell.Length < 3) {
//			FizzleSpell();
//		} else {
			CastSpell();
//		}
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
		lineController.AddPoint (GetObjectBySymbol (symbol).transform.localPosition + parentTransform.localPosition);

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

	public void StopCooldowns(){
		for (int i = 0; i < parentPrefab.transform.childCount; ++i) {
			var child = parentPrefab.transform.GetChild (i);
			var symbol = child.GetComponent<Symbol>();
			symbol.StopCooldown ();
		}

		ResetDimmedSymbols (false);
	}

	private void ResetDimmedSymbols(bool fizzle) {
		for(int i = 0; i < parentPrefab.transform.childCount; ++i) {
			var child = parentPrefab.transform.GetChild(i);
			var img = child.GetComponentsInChildren<Image>()[1];
			var s = child.GetComponent<Symbol>();
			if(fizzle && currentSpell.Contains("" + s.mySymbol)) {
				s.Shake();
			}

			if(s.isCooldown) {continue;}

			if(img != null && img.fillAmount != 1f) {
				var a = (Instantiate(lerperPrefab) as GameObject).GetComponent<Lerper>();
				a.Init(0f, 1f, 0f, img);
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
			var img = ch.GetComponentsInChildren<Image>()[1];
			var a = (Instantiate(lerperPrefab) as GameObject).GetComponent<Lerper>();
			a.Init(1f, 0f, 0f, img);
		}

		// search with "currentSpell"
		List<string> spells = spellSearch.SearchSpells(currentSpell);
		if(spells.Count == 0) {
			// no available spell
			FizzleSpell();
		}
//		List<char> symbols = new List<char>(spellSearch.Symbols);
		Outline outline = null;
		GameObject obj = null;
//		foreach(char c in symbols) {
//			obj = GetObjectBySymbol(c);
//			var s = obj.GetComponent<Symbol>();
//			if(s.isCooldown || currentSpell.Contains(""+c)) { continue; }
//			outline = obj.GetComponent<Outline>();
//			if(outline != null) {
//				outline.enabled = true;
//				outline.effectColor = new Color(Color.red.r, Color.red.g, Color.red.b, outline.effectColor.a);
//			}
//		}

		foreach(string spell in spells) {
			// this is a spell with only one more symbol
			if(spell.Length == currentSpell.Length+1){
				obj = GetObjectBySymbol(spell[currentSpell.Length]);
				outline = obj.GetComponent<Outline>();
				outline.effectColor = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, outline.effectColor.a);
			}
			// else a few more symbols to go
			else if(spell.Length > currentSpell.Length+1){
				obj = GetObjectBySymbol(spell[currentSpell.Length]);
				outline = obj.GetComponent<Outline>();
				outline.effectColor = new Color(Color.red.r, Color.red.g, Color.red.b, outline.effectColor.a);
			}

			if(outline != null) {
				outline.enabled = true;
			}
		}
	}

	public void FizzleSpell() {
		Debug.Log("Spell Fizzled");

		//add the cooldowns for the symbols
//		GameObject obj = null;
//		foreach(char c in currentSpell) {
//			obj = GetObjectBySymbol(c);
//			Symbol s = obj.GetComponent<Symbol>();
//			s.StartCooldown(Utils.CalcCooldown(currentSpell));
//		}

		//ResetHighlights();
		ResetDimmedSymbols(true);

		currentSpell = "";

		//clear the line renderer
		lineController.ResetLines ();
	}

	public void CastSpell() {
		Debug.Log ("Spell cast: " + currentSpell);
		var scoreMult = 1f;
		GameObject obj = null;
		foreach(char c in currentSpell) {
			obj = GetObjectBySymbol(c);
			Symbol s = obj.GetComponent<Symbol>();
			s.StartCooldown(Utils.CalcCooldown(currentSpell));

			if(SpecialSymbols.Contains(c)) {
				scoreMult *= 2f;
				SpecialSymbols.Remove(c);
				s.StopGlow ();
			}
		}
		
		//clear the line renderer
		lineController.ResetLines ();

		scoreController.AddScore (activePlayer, (int)(Utils.ConvertSpellToScore(currentSpell) * scoreMult));

		// make one a special one
		List<char> ssss = new List<char>(spellSearch.Symbols);
		while(SpecialSymbols.Count < 12 && ssss.Count != 0) {
			char s = ssss[Random.Range(0, ssss.Count)];
			if(false == SpecialSymbols.Contains(s)) {
				SpecialSymbols.Add(s);
				GetObjectBySymbol (s).GetComponent<Symbol> ().StartGlow ();
				break;
			} else {
				ssss.Remove(s);
			}
		}

		spellSearch.GenerateNewSpell();
	}


}




