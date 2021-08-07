using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * force, ForceMode.Impulse);
    }
}
