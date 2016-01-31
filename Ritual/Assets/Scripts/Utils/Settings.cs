using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {
	private static Settings instance = null;
	public static Settings _ {
		get {
			if(null == instance) {
				instance =  FindObjectOfType(typeof (Settings)) as Settings;
			}
			
			// If it is still null, create a new instance
			if (instance == null) {
				GameObject obj = new GameObject("Settings");
				instance = obj.AddComponent(typeof (Settings)) as Settings;
				Debug.Log ("Could not locate an Settings object. \n Settings was Generated Automatically.");
			}

			return instance;
		}}

	public float SymbolFade = 0.2f;
	public float FizzleShakeAmount = 20f;
	public float FizzleShakeTime = 0.5f;
	public float BaseCooldown3 = 6.0f;
	public float BaseCooldown12 = 1.0f;

	public int MinScore = 0;
	public int StartingScore = 0;
	public int MaxScore = 100;
}
