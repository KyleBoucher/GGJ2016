using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Symbol : MonoBehaviour
{
	private SymbolController controller;

	public void SetController( SymbolController controller){
		this.controller = controller;
	}
}