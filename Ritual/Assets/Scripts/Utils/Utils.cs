using UnityEngine;

public class Utils
{
	public static int ConvertSpellToScore( string spell ){
		return spell.Length;
	}

	public static float CalcCooldown(string spell){
		return Mathf.Lerp (Settings._.BaseCooldown3, Settings._.BaseCooldown12, (spell.Length - 3) / 9.0f);
	}

	public static float GetGlowChance( string spell ){
		return (float)spell.Length / 12f;
	}
}

