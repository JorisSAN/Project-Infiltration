using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace props.door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] protected Animator _doorAnimation;


        public virtual void OnTriggerEnter(Collider collider)
        {
            OpenDoor();
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            CloseDoor();
        }

        protected void OpenDoor()
        {
            _doorAnimation.SetBool("isOpen", true);
        }

        protected void CloseDoor()
        {
            _doorAnimation.SetBool("isOpen", false);
        }
    }
}
