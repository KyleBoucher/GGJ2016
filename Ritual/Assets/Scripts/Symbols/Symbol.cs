using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Symbol : MonoBehaviour
{
	public char mySymbol;

	private SymbolController controller;

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
		// add to spell
		controller.AddToSpell(mySymbol);
	}
	public void OnMouseExit() {
		// probably don't need this
	}
}