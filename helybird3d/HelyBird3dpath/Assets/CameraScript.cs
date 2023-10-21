using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Transform target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    void Update()
    {
        //if (GameManager.isGameStarted == true)
        //{
            //target = GameObject.FindGameObjectWithTag("playerGFX").transform;
            transform.position = new Vector3(target.position.x + offsetX, target.transform.position.y + offsetY, target.position.z + offsetZ);
        //}
    }

    private void OnValidate()
    {
        transform.position = new Vector3(target.position.x + offsetX, target.transform.position.y + offsetY, target.position.z + offsetZ);
    }
}
