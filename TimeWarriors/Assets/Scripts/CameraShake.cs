using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;

    float shakeAmount = 0;

	// Use this for initialization
	void Awake () {
        mainCam = Camera.main;
	}
	
	void DoShake() {
        if(shakeAmount > 0) {
            Vector3 camPos = mainCam.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPos.x = offsetX;
            camPos.x = offsetY;

            mainCam.transform.localPosition = camPos;
        }
    }

    void StopShake() {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }

    public void Shake(float amt, float length) {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.2f);
        Invoke("StopShake", length);
    }
}
