using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	private GvrViewer mCam;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public Transform weapon;

	void Awake() {
		mCam = GetComponentInChildren<GvrViewer> ();
		mCam.gameObject.SetActive (false);
	}

	void Update() {
		
		// If we are not the local player do not execute this script
		if (!isLocalPlayer) {
			return;
		}

		// Google cardboard trigger
		if (Input.GetButtonDown ("Fire1")) {    
			CmdFire ();
			CmdMelee ();
		}
	}

	// Fires a bullet
	[Command]   
	void CmdFire() {
		GameObject bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		
		bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 6;

		NetworkServer.Spawn (bullet);

		Destroy (bullet, 2f);
	}

	[Command]
	void CmdMelee() {
		// TODO: Figure out how to call the animation using animator
	}
		
	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer> ().material.color = Color.blue;
		mCam.gameObject.SetActive (true);
	}
		
}
