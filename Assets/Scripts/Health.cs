// Modified from https://unity3d.com/learn/tutorials/topics/multiplayer-networking/networking-player-health?playlist=29690
// Health script manages both players and enemies
// Check destroyOnDeath for enemies

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public const int maxHealth = 100;

	public bool destroyOnDeath;

	private float timePoison = 0;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;

	public RectTransform healthBar;

	void Update() {
		if (timePoison > 1) {
			currentHealth -= 2;
			timePoison = 0;
		}
		timePoison += Time.deltaTime;
	}

	public void TakeDamage(int amount) {
		if (!isServer) {
			return;
		}

		currentHealth -= amount;
		if (currentHealth <= 0) {
			if (destroyOnDeath) {
				Destroy(gameObject);
			} else {
				currentHealth = maxHealth;

				// called on the Server, will be invoked on the Clients
				RpcRespawn();
			}
		}
	}

	void OnChangeHealth (int currentHealth) {
		Debug.Log ("Health changed!");
//		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}

//	[Command]
//	public void CmdRestoreHealth(int amount) {
//		if (!isServer) {
//			return;
//		}
//		currentHealth += amount;
//	}

//	[Command]
	public void RestoreHealth(int amount) {
		if (!isServer) {
			return;
		}
		currentHealth += amount;
	}


	[ClientRpc]
	void RpcRespawn() {        
		if (isLocalPlayer) {
			// Set the player’s position to origin
			transform.position = Vector3.zero;
		}
	}
}