using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != "Player") { return; }

        this.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag != "Player") { return; }

        this.gameObject.SetActive(false);
    }
}
