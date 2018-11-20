// Stores Option values across all scenes
using UnityEngine;

public class TimeEngine : MonoBehaviour {

    public int jumpMode = 1;


	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
