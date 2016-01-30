using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	public ScoreController scoreController;
	public SpellSearch spellSearch;
	public LineController lineController;
	public SymbolController symbolController;
	public Transform parentTransform;
	public Constants.PlayerIndex activePlayer;
	public string currentSpell = "";

	public float speed;

	public bool isPlaying = false;
	public bool[] isCooldown = new bool[12];
	public float[] cooldowns = new float[12];
	public string targetSpell = "";

	public Vector3 targetSymbolPos;
	public Vector3 currentMousePos;
	public GameObject targetSymbol;
	public Vector3 startPosition;
	public float lerpFloat;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlaying) {
			// update cooldown symbols
			for(int i = 0; i < cooldowns.Length; ++i) {
				if(isCooldown[i]) {
					cooldowns[i] -= Time.deltaTime;
					if(cooldowns[i] <= 0f) {
						cooldowns[i] = 0f;
						isCooldown[i] = false;
					}
				}
			}

			// do I have a target spell
			if(targetSpell.Length != 0) {
				if(targetSymbol == null) {
					targetSymbol = symbolController.GetObjectBySymbol(targetSpell[currentSpell.Length]);
					targetSymbolPos = targetSymbol.GetComponent<Symbol>().origPos + parentTransform.localPosition;
					startPosition = symbolController.GetObjectBySymbol(targetSpell[currentSpell.Length-1])
									.GetComponent<Symbol>().origPos + parentTransform.localPosition;
				}

				lerpFloat += Time.deltaTime*speed;

				currentMousePos = Vector3.Lerp(startPosition, targetSymbolPos, lerpFloat);
				lineController.SetAIMousePos(currentMousePos);

				// yes, lerp my "pointer" to next symbol in target
				if(Vector3.SqrMagnitude(targetSymbolPos - currentMousePos) <= 10f) {
					lineController.AddPoint(targetSymbolPos);
					currentSpell += targetSpell[currentSpell.Length];
					targetSymbol = null;
					lerpFloat = 0f;

					if(currentSpell.Length == targetSpell.Length) {
						scoreController.AddScore(activePlayer, currentSpell.Length);
						targetSpell = "";
						lineController.ResetLines();

						foreach(char c in currentSpell) {
							int ind = spellSearch.Symbols.BinarySearch(c);
							if(ind != -1) {
								isCooldown[ind] = true;
								cooldowns[ind] = Mathf.Lerp(Settings._.BaseCooldown3, Settings._.BaseCooldown12, (currentSpell.Length-3)/9.0f);
							}
						}
					} 
				}

			} else {
			// else
				// no,
				// search for all available spells with my currentSpell sequence
				List<string> spells = spellSearch.AllSpells;
				// strip all that have symbols on cooldown
				List<string> tmpSpells = new List<string> (spells);
				foreach(string spell in spells) {
					for(int i = 0; i < isCooldown.Length; ++i) {
						if(isCooldown[i] && spell.Contains("" + spellSearch.Symbols[i])) {
							tmpSpells.Remove(spell);
						}
					}
				}

				// is there leftover spells?
				if(tmpSpells.Count > 0) {
					// set target to random from leftover list
					targetSpell = tmpSpells[Random.Range(0, tmpSpells.Count)];
					currentMousePos = symbolController.GetObjectBySymbol(targetSpell[0]).transform.localPosition + parentTransform.localPosition;
					currentSpell = "" + targetSpell[0];

					lineController.ResetLines();
					lineController.SetAIMousePos(currentMousePos);
					lineController.AddPoint(currentMousePos);

					targetSymbol = symbolController.GetObjectBySymbol(targetSpell[currentSpell.Length]);
					targetSymbolPos = targetSymbol.GetComponent<Symbol>().origPos + parentTransform.localPosition;
					startPosition = currentMousePos;

					lerpFloat = 0f;
				} 
				// else
					// do nothing, next frame will update cooldowns
			}
		}
	}

	public void SetIsPlaying(bool playing) {
		isPlaying = playing;
	}

	public void Begin() {
		SetIsPlaying(true);
	}

	public void Stop() {
		SetIsPlaying(false);

		targetSpell = "";
		targetSymbol = null;
		lineController.ResetLines();
	}
}




