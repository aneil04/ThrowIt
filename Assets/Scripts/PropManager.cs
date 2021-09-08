using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public List<GameObject> allObjects;

    public float force;

    public Vector2 spawnBoundsX;
    public Vector2 spawnBoundsY;

    public Vector2 spawnInterval;
    private float timeToSpawn = 0f;

    public float yOffset;
    public Vector3 scale;

    public Vector2 angularVelocityRange;

    public GameObject outline;

    void Update()
    {
        timeToSpawn -= Time.deltaTime;
        if (timeToSpawn <= 0)
        {
            timeToSpawn = Random.Range(spawnInterval.x, spawnInterval.y);
            spawnObject((int)Random.Range(0, allObjects.Count));
        }
    }

    private void spawnObject(int index)
    {
        Vector3 pos;
        Vector3 forceVector;
        if (Random.Range(0, 10) < 5) //left
        {
            pos = new Vector3(spawnBoundsX.x, Random.Range(spawnBoundsY.x, spawnBoundsY.y), this.transform.position.z);
            forceVector = Vector3.right;
        }
        else //right
        {
            pos = new Vector3(spawnBoundsX.y, Random.Range(spawnBoundsY.x, spawnBoundsY.y), this.transform.position.z);
            forceVector = Vector3.left;
        }
        forceVector.y += yOffset;
        forceVector.z -= 0.05f;

        GameObject obj = Instantiate(allObjects[index], pos, Quaternion.identity);
        GameObject objOutline = Instantiate(outline, pos, Quaternion.identity);


        objOutline.transform.SetParent(obj.transform);

        obj.transform.localScale = scale;
        obj.GetComponent<Rigidbody>().AddForce(forceVector * force, ForceMode.Impulse);
        obj.GetComponent<Rigidbody>().angularVelocity = new Vector3(
            Random.Range(angularVelocityRange.x, angularVelocityRange.y),
            Random.Range(angularVelocityRange.x, angularVelocityRange.y),
            Random.Range(angularVelocityRange.x, angularVelocityRange.y)
        );

        Destroy(obj, spawnInterval.y + 0.5f);
    }
}