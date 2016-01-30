using System;
using UnityEngine;

public class MenuLineController : MonoBehaviour
{
	public Canvas canvas;

	public void Start(){
		var scale = canvas.transform.localScale;
		scale.z = 1f;
		transform.localScale = scale;
	}
}

