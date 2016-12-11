using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectionInput : MonoBehaviour {

	public EventSystem eventsystem;
	public GameObject selectedObject;
	private bool selected = false;

	void Update () {
		if (Input.GetAxisRaw("Vertical") != 0 && selected == false)
		{
			eventsystem.SetSelectedGameObject(selectedObject);
			selected = true;
		}
	}

	private void OnDisable()
	{
		selected = false;
	}
}