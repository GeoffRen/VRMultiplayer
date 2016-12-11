using UnityEngine;
using System.Collections;

public class HealthSettings : MonoBehaviour {

	public static bool healthStatus = true;

	public void toggleHealthStatus(int toggleHealth){

		if (toggleHealth == 0)
			healthStatus = false;
	}
}
