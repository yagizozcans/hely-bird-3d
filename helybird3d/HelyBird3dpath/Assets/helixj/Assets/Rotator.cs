using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField] float rotationSensitivity;

	void Update() {
			transform.Rotate(-Vector3.up * rotationSensitivity * Time.deltaTime, Space.World);
	}
}
