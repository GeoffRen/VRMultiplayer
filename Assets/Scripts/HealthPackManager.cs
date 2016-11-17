using UnityEngine;
using UnityEngine.Networking;

public class HealthPackManager : NetworkBehaviour {

	public int restoreAmount;

	GameObject player;

	void Awake() {
//		player = GameObject.FindGameObjectWithTag ("Player");
	}
//
//	[ClientRpc]
//	void RpcHealthPackAction() {
//		if (isLocalPlayer) {
//			player.GetComponent<Health> ().CmdRestoreHealth (restoreAmount);
//		}
//	}
//
//	[Command]
//	void CmdHealthPack() {
//		if (!isServer) {
//			return;
//		}
//		RpcHealthPackAction ();
//	}

//	[Command]
//	void CmdOnCollision(Collision other) {
//		Destroy (this.gameObject);	// Destroy health pack
//		Destroy (other.gameObject);	// Destroy bullet
//		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
//			player.GetComponent<Health> ().CmdRestoreHealth (restoreAmount);
//		}
//	}


	// If we make the health pack a trigger, it may be spawned on top of other elements
	void OnCollisionEnter(Collision other) {
		NetworkServer.Destroy (this.gameObject);	// Destroy health pack
		NetworkServer.Destroy (other.gameObject);	// Destroy bullet
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			player.GetComponent<Health> ().RestoreHealth (restoreAmount);
		}

//		CmdOnCollision(other);

//		GameObject.FindGameObjectWithTag ("Player").GetComponent<Health> ().CmdRestoreHealth (restoreAmount);
//		Debug.Log(NetworkServer.connections);
//		Debug.Log(NetworkServer.connections.Count);
		// called on the Server, will be invoked on the Clients
		//		RestoreHealth(restoreAmount);
	}

//		if (isLocalPlayer) {
//			// TODO: how do we get the local player object?
//			//			playerHealth.RestoreHealth (restoreAmount);
//		}
//		// restores health to players by looking at the health pack and clicking the trigger
//	}
	// TODO: maybe instead of a fixed number of health packs, we want to spawn them slowly over time?
}
