using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SymbolController : MonoBehaviour {

	public GameObject symbolPrefab;
	public GameObject parentPrefab;

	private List<Symbol> symbols = new List<Symbol>();
	private int symbolCount = 12;
	private float circleWidth = Constants.screenWidth * 0.5f - 100f;

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
			symbolObject.transform.parent = parentPrefab.transform;
			symbolObject.transform.localScale = Vector3.one;

			//rotate and position symbol into position
			var newPos = new Vector3();
			var angle = maxAngle*i*Mathf.Deg2Rad;
			newPos.x = circleWidth*Mathf.Cos(angle);
			newPos.y = circleWidth*Mathf.Sin(angle);

			symbolObject.transform.localPosition = newPos;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
