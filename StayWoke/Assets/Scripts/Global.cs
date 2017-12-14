using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

	public static Global instance = null;
	public GameObject winText;

	public float resetDelay;

	void Awake()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != null) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Win()
	{
		winText.SetActive (true);
		Time.timeScale = 1.0f;
		Invoke ("Reset", 10);
	}

	public void Reset()
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel (Application.loadedLevel);
	}
}
