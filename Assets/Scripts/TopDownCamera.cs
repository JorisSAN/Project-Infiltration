using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndiePixel.Cameras
{
    public class TopDownCamera : MonoBehaviour
    {
        #region Variables
        public Transform m_Target;

        [SerializeField]
        public float m_Height = 10f;

        [SerializeField]
        public float m_Distance = 3f;

        [SerializeField]
        public float m_Angle = 0f;

        [SerializeField]
        public float m_SmoothSpeed = 0.5f;

        private Vector3 refVelocity;
        #endregion

        #region Main Methods
        // Start is called before the first frame update
        void Start()
        {
            HandleCamera();
        }

        // Update is called once per frame
        void Update()
        {
            HandleCamera();
        }
        #endregion

        #region Helper Methods
        protected virtual void HandleCamera()
        {
            if(!m_Target)
            {
                return;
            }

            // Build World position vector
            Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
                Debug.DrawLine(m_Target.position, worldPosition, Color.red);

            // Build our Rotated Vector
            Vector3 rotatedVector = Quaternion.AngleAxis(m_Angle, Vector3.up) * worldPosition;
            Debug.DrawLine(m_Target.position, rotatedVector, Color.green);

            // Move our position
            Vector3 flatTargetPosition = m_Target.position;
            flatTargetPosition.y = 0f;
            Vector3 finalPosition = flatTargetPosition + rotatedVector;
            Debug.DrawLine(m_Target.position, finalPosition, Color.blue);

            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, m_SmoothSpeed);
            transform.LookAt(flatTargetPosition);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
            if(m_Target)
            {
                Gizmos.DrawLine(transform.position, m_Target.position);
            }
        }
        #endregion
    }
}
