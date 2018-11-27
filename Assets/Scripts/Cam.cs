using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    [SerializeField] Transform point;
    [SerializeField] float speed;
    void Update() {

        if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0) {

            transform.RotateAround(point.position, Vector3.up, Input.GetAxis("Mouse X") * speed * Time.deltaTime);

        }
        transform.LookAt(point.position);
    }
}
