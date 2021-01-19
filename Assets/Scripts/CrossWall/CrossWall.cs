using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossWall : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public bool rightClick = false;

    void Start() {
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        int rayDistance = 3;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Did Hit");

            target.transform.position = transform.position + ((transform.TransformDirection(Vector3.forward) * hit.distance) + transform.TransformDirection(Vector3.forward));

            target.SetActive(true);

            if(Input.GetKeyDown(KeyCode.Mouse1)) {
                rightClick = true;
            }
            if(Input.GetKeyUp(KeyCode.Mouse1)) {
                transform.position = target.transform.position;
                rightClick = false;
            }

            if (!rightClick) {
                target.SetActive(false);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayDistance, Color.green);
            Debug.Log("Did not Hit");
            target.SetActive(false);
        }
    }
}
