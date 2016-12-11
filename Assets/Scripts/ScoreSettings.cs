using UnityEngine;
using System.Collections;

public class ScoreSettings : MonoBehaviour {

	public static bool scoreStatus = true;

	public void toggleScore(int toggleScore){
	
		if (toggleScore == 0)
			scoreStatus = false;

	}

}
