using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSetup : NetworkBehaviour 
{

	[SerializeField]
	public Behaviour[] localComponentsToEnable;
	bool disabled = false;
	// Use this for initialization
	void Start () 
	{
		if (isLocalPlayer) {

			//Camera.main.gameObject.SetActive (false);
			GameManager.numPlayers++;
			foreach (Behaviour comp in localComponentsToEnable) {
				comp.enabled = true;
			}
		}

		if (!isLocalPlayer) {

			//Camera.main.gameObject.SetActive (false);
			GameManager.numPlayers++;
			foreach (Behaviour comp in localComponentsToEnable) {
				comp.enabled = false;
			}
		}
	}

}
