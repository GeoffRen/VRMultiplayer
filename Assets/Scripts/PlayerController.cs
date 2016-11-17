using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	private GvrViewer mCam;
	private GameObject mHead;

	private bool canShoot;
	private bool canMelee;

	public GameObject bulletPrefab;
	public GameObject weaponPrefab;

	public Transform bulletSpawn;

	void Awake() {
		mCam = GetComponentInChildren<GvrViewer> ();
		mCam.gameObject.SetActive (false);

		canShoot = true;
		canMelee = true;
	}

	void Update() {
		
		// If we are not the local player do not execute this script
		if (!isLocalPlayer) {
			return;
		}

		// Google cardboard trigger
		if (Input.GetKeyDown(KeyCode.Space) ||
			Input.GetButtonDown ("Fire1")) {    
			if (canShoot) {
				StartCoroutine (shoot ());
			}

			if (canMelee) {
				StartCoroutine (melee ());
			}
		}
	}

	// TODO: Stop raycast from colliding with face
	void FixedUpdate() {
		RaycastHit hit;
		int layerMask = 1 << 8;
		if (mHead) {
			Debug.DrawRay (mHead.transform.position, mHead.transform.forward * 50f, Color.red, .01f, false);
			if (Physics.Raycast(mHead.transform.position, mHead.transform.forward, out hit, 50f)) {
//				Debug.Log ("Hit");
			}
		}
	}


	public IEnumerator shoot () {
		CmdFire ();

		canShoot = false;
		yield return new WaitForSeconds (.1f);
		canShoot = true;
	}

	public IEnumerator melee () {
		CmdMelee ();

		canMelee = false;
		yield return new WaitForSeconds (1f);
		canMelee = true;
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
		GameObject weapon = (GameObject)Instantiate (
			weaponPrefab,
			this.transform.position,
			this.transform.rotation,
			this.transform);

		NetworkServer.Spawn (weapon);

		Destroy (weapon, .5f);
	}
		
	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer> ().material.color = Color.blue;
		mCam.gameObject.SetActive (true);

		if (mCam.gameObject.activeSelf) {
			mHead = GetComponentInChildren<GvrHead> ().gameObject;
		}
	}
		
}
