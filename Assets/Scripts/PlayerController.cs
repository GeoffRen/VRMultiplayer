using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// TODO: Turn off gvrViewer and Camera for non LocalPlayer without turning off models
// TODO: Clean up magic numbers 
// Remember to remove movement testing for deployment!!!
public class PlayerController : NetworkBehaviour {

	private GvrViewer gvrViewer;
	//private Camera mCam;
	private GameObject mHead;

	private bool canShoot;
	private bool canMelee;

	public GameObject bulletPrefab;
	public GameObject weaponPrefab;

	public Transform bulletSpawn;

	void Awake() {
//		gvrViewer = GetComponentInChildren<GvrViewer> ();
//		gvrViewer.enabled = false;
//
//		mCam = GetComponentInChildren<Camera> ();
//		mCam.gameObject.SetActive (false);

		// Currently setting everything inactive if not LocalPlayer
		gvrViewer = GetComponentInChildren<GvrViewer> ();
		gvrViewer.gameObject.SetActive (false);

		canShoot = true;
		canMelee = true;
	}

	void Update() {
		
		// If we are not the local player do not execute this script
		if (!isLocalPlayer) {
			return;
		}

		// Keyboard movement for testing purposes only
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
		// End testing movement

		// Google cardboard trigger
		if (Input.GetButtonDown ("Fire1")) {   
			if (canShoot) {
				StartCoroutine (shoot ());
			}

			if (canMelee) {
				StartCoroutine (melee ());
			}
		}
	}

	// If the player is looking at a close enemy: melee, otherwise shoot
	void FixedUpdate() {

		// Layer the enemies reside in
		int layerMask = 1 << 8;
		RaycastHit hit;
		if (mHead) {
			if (Physics.Raycast (mHead.transform.position, mHead.transform.forward, out hit, 2f, layerMask)) {
				canShoot = false;
				canMelee = true;

			} else {
				canShoot = true;
				canMelee = false;
			}
		}
	}

	// Coroutine for shooting
	public IEnumerator shoot () {
		CmdFire ();

		canShoot = false;
		yield return new WaitForSeconds (.1f);
		canShoot = true;
	}

	// Coroutine for meleeing
	public IEnumerator melee () {
		CmdMelee ();

		canMelee = false;
		yield return new WaitForSeconds (1f);
		canMelee = true;
	}

	// Fires a bullet from the gun tip
	[Command]   
	void CmdFire() {
		GameObject bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
		
		bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10f;

		NetworkServer.Spawn (bullet);

		Destroy (bullet, 2f);
	}

	// Melees
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
		
	// Initializes the LocalPlayer
	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer> ().material.color = Color.blue;

//		gvrViewer.enabled = true;
//		mCam.gameObject.SetActive (true);

		gvrViewer.gameObject.SetActive (true);

		if (gvrViewer.gameObject.activeSelf) {
			mHead = GetComponentInChildren<GvrHead> ().gameObject;
		}
	}
		
}
