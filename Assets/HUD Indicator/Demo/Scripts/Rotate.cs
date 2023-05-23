using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    [SerializeField] private float speed = 20f;

    void Update() {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
