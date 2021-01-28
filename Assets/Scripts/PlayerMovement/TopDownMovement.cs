using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private Material[] dissolveMaterials;

    private float targetDissolveValue = 0f;
    private float currentDissolveValue = 0f;

    public float speed = 3f;
    bool shift = false;
    bool crouch = false;
    bool Z = false;
    bool S = false;
    bool Q = false;
    bool D = false;
    bool leftClick = false;
    bool roll = false;

    float timer = 0.7857143f;
    float timeleft = 0;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool IsWalking
    {
        get
        {
            bool walkVertical = !((Z & S) | !(Z | S));
            bool walkHorizontal = !((Q & D) | !(Q | D));
            return walkVertical || walkHorizontal;
        }
    }

    public bool IsRunning
    {
        get
        {
            return IsWalking && shift;
        }
    }

    public bool IsCrouching
    {
        get
        {
            return IsWalking && crouch;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            shift = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            shift = false;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            crouch = true;
        }
        if (Input.GetKeyUp(KeyCode.C)) {
            crouch = false;
        }

        float realspeed;

        if (shift) {
            realspeed = speed*2;
        }
        else if(crouch) {
            realspeed = speed/2;
        }
        else {
            realspeed = speed;
        }

        if(Input.GetKeyDown(KeyCode.Z)) {
            Z = true;
        }
        if(Input.GetKeyUp(KeyCode.Z)) {
            Z = false;
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            S = true;
        }
        if(Input.GetKeyUp(KeyCode.S)) {
            S = false;
        }
        
        float vertical;
        if((Z & S) | !(Z | S)) {
            vertical = 0;
        }
        else if(Z) {
            vertical = 1;
        }
        else {
            vertical = -1;
        }
        

        if(Input.GetKeyDown(KeyCode.Q)) {
            Q = true;
        }
        if(Input.GetKeyUp(KeyCode.Q)) {
            Q = false;
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            D = true;
        }
        if(Input.GetKeyUp(KeyCode.D)) {
            D = false;
        }

        float horizontal;
        if((Q & D) | !(Q | D)) {
            horizontal = 0;
        }
        else if(D) {
            horizontal = 1;
        }
        else {
            horizontal = -1;
        }

        if (roll) {
            roll = false;
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            roll = true;
            timeleft = timer;
        }

        if(timeleft > 0) {
            timeleft -= Time.deltaTime;
            realspeed = speed * 2;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * realspeed * Time.deltaTime);
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            leftClick = true;
        }
        if(Input.GetKeyUp(KeyCode.Mouse0)) {
            leftClick = false;
        }

        if(leftClick) {
            targetDissolveValue = 1f;
        }
        else {
            targetDissolveValue = 0f;
        }

        currentDissolveValue = Mathf.Lerp(currentDissolveValue, targetDissolveValue, 2f * Time.deltaTime);

        foreach(Material renderer in dissolveMaterials) {
            renderer.SetFloat("_Dissolve", currentDissolveValue);
        }
        
        /*
        if (IsRunning)
        {
            Debug.Log("Is running");
        }
        else if (IsCrouching)
        {
            Debug.Log("Is crouching");
        }
        else if (IsWalking)
        {
            Debug.Log("Is walking");
        }
        */
    }
}
