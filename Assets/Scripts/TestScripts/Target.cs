using UnityEngine;

public class Target: MonoBehaviour {

    private void Update() {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += dir * 20 * Time.deltaTime;
    }
}