using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviour
{
    Animator animation;
    
    bool bcrouch = false;
    bool brun = false;
    bool bwalk = false;
    bool broll = false;

    bool up = false, down = false, left = false, right = false;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            bcrouch = true;
        }
        else if (Input.GetKeyUp(KeyCode.C)) {
            bcrouch = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            brun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            brun = false;
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            up = true;
        }
        else if (Input.GetKeyUp(KeyCode.Z)) {
            up = false;
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            down = true;
        }
        else if (Input.GetKeyUp(KeyCode.S)) {
            down = false;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            left = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q)) {
            left = false;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            right = true;
        }
        else if (Input.GetKeyUp(KeyCode.D)) {
            right = false;
        }

        if (up | down | left | right) {
            bwalk = true;
        }
        else {
            bwalk = false;
        }

        if (broll) {
            broll = false;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            broll = true;
        }

        animation.SetBool("crouch", bcrouch);
        animation.SetBool("run", brun);
        animation.SetBool("walk", bwalk);
        animation.SetBool("roll", broll);
    }
}
