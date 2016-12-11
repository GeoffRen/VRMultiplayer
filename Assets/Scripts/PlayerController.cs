using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// TODO: Turn off gvrViewer and Camera for non LocalPlayer without turning off models
// TODO: Clean up magic numbers 
// Remember to remove movement testing for deployment!!!
public class PlayerController : NetworkBehaviour
{
	// Player camera, where the raycast that determines attack type emits from
    private GameObject mHead;

	// Control whether meleeing or shooting occurs
    private bool canShoot;
    private bool canMelee;

	// Prefabs used to shoot or melee
    public GameObject bulletPrefab;
    public GameObject weaponPrefab;

	// Where the weapons spawn from
    public Transform bulletSpawn;
	public Transform meleeSpawn;

	public static bool attackAction = false;
	 
	public float collisionBuffer = 1.0f;
	private float timeSinceCollision = 1.0f;

    void Update()
    {

        // If we are not the local player do not execute this script
        if (!isLocalPlayer)
        {
            return;
        }

        // Keyboard movement for testing purposes only
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        // End testing movement

        // Google cardboard trigger
		if (attackAction)
        {
            if (canShoot)
            {
                StartCoroutine(shoot());
            }

            if (canMelee)
            {
                StartCoroutine(melee());
            }

			attackAction = false;
        }
    }

    // If the player is looking at a close enemy: melee, otherwise shoot
    void FixedUpdate()
    {

        // Layer the enemies reside in
        int layerMask = 1 << 8;
        RaycastHit hit;
        if (mHead)
        {
            if (Physics.Raycast(mHead.transform.position, mHead.transform.forward, out hit, 2f, layerMask))
            {
                canShoot = false;
                canMelee = true;

            }
            else
            {
                canShoot = true;
                canMelee = false;
            }
        }

		timeSinceCollision += Time.deltaTime;
    }

    // Coroutine for shooting
    public IEnumerator shoot()
    {
        CmdFire();

        canShoot = false;
        yield return new WaitForSeconds(.1f);
        canShoot = true;
    }

    // Coroutine for meleeing
    public IEnumerator melee()
    {
        CmdMelee();

        canMelee = false;
        yield return new WaitForSeconds(1f);
        canMelee = true;
    }

    // Fires a bullet from the gun tip
    [Command]
    void CmdFire()
    {
        GameObject bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 10f;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2f);
    }

    // Instantiate the melee animation in front of the player
    [Command]
    void CmdMelee()
    {
        GameObject weapon = (GameObject)Instantiate(
            weaponPrefab,
			meleeSpawn.position,
			meleeSpawn.rotation,
            this.transform);

        NetworkServer.Spawn(weapon);

        Destroy(weapon, .5f);
    }

    // Initializes the LocalPlayer
    public override void OnStartLocalPlayer()
    {
		mHead = GetComponentInChildren<Camera>().gameObject;
    }


    public void Die()
    {
        Debug.Log("Died");

        Destroy(gameObject);
        if (isLocalPlayer)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().PlayerDied();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
			if (timeSinceCollision > collisionBuffer) {
				GetComponent<Health>().TakeDamage(20);
				timeSinceCollision = 0.0f;
			}
        }
    }
}