using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

	private Valve.VR.EVRButtonId gripButton = 
		Valve.VR.EVRButtonId.k_EButton_Grip;

	private Valve.VR.EVRButtonId triggerButton = 
		Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	public SteamVR_Controller.Device controller 
	{ get { return SteamVR_Controller.Input((int)trackedObj.index); } }

	private SteamVR_TrackedObject trackedObj;

	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	
	// Update is called once per frame
	void Update () {
		if (controller.GetPressDown (triggerButton)) {
			PlayerController.attackAction = true;
		}
	}
}
