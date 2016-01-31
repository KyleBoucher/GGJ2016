using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellSearch : MonoBehaviour {
	[SerializeField] public List<string> AllSpells = new List<string>();
	[SerializeField] public List<char> Symbols = new List<char>();

	[SerializeField] public List<LengthCount> AutoGenSpells = new List<LengthCount>();
	public int MinLength = 3;
	public int MaxLength = 12;

	public string currentSpell = "";

	void Start() {
		foreach(var item in AutoGenSpells) {
			for(int i = 0; i < item.count; ++i) {
				AllSpells.Add(GetRandomSpell(item.length));
			}
		}
	}

	private string GetRandomSpell(int length) {
		if(length < MinLength) {
			length = MinLength;
		}
		if(length > MaxLength) {
			length = MaxLength;
		}

		string ret = "";
		List<char> localSymbols = new List<char>(Symbols);

		for(int i = 0; i < length; ++i) {
			int ind = Random.Range(0, localSymbols.Count);
			ret += localSymbols[ind];

			localSymbols.RemoveAt(ind);
		}

		return ret;
	}

	public List<string> SearchSpells(string search) {
		List<string> retList = new List<string>();
		retList.Add(currentSpell);

//		foreach(string spell in AllSpells) {
//			if(spell.IndexOf(search) == 0) {
//				retList.Add(spell);
//			}
//		}

		return retList;
	}

	public void GenerateNewSpell(int round) {
		currentSpell = GetRandomSpell(Random.Range(0, Mathf.Min(12, 3+(round-1))));
	}
}

[System.Serializable]
public class LengthCount {
	public int length;
	public int count;
}