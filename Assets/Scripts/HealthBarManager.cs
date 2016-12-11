using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour {

	public static int health;
	Text text;

	void Awake()
	{
		text = GetComponent<Text>();
		health = Health.maxHealth;
	}

	void Update()
	{
		if(HealthSettings.healthStatus)
			text.text = "Health: " + health;
	}
}