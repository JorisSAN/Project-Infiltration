﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // Start is called before the first frame update
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask PlayerMask;
    public LayerMask ObstacleMask;

    public bool playerSeen;
    public float vision;
    public float maxVision;
    public List<Transform> visiblePlayers = new List<Transform>();

    public float meshResolution;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    public int edgeResolveIterations;
    public float edgeDistanceTreshold;


    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindPlayerWithDelay", 0.2f);
    }


    IEnumerator FindPlayerWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            playerSeen=FindVisiblePlayer();
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal )
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void LateUpdate()
    {
        DrawFieldView();
    }
    public bool FindVisiblePlayer()
    {
        visiblePlayers.Clear();
        Collider[] playerInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, PlayerMask);
    
        for(int i = 0; i < playerInViewRadius.Length; i++)
        {
            Transform player = playerInViewRadius[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, ObstacleMask))
                {
                    visiblePlayers.Add(player);
                    vision = vision + 0.2f;
                    if(vision >= maxVision)
                    {
                        return true;

                    }
                    return false;
                }
            }
        }
        vision = 0;
        return false;

    }
   
           void DrawFieldView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle,1.0f);
            if( i>0)
            {

                bool edgeDstTresholdExceed = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDistanceTreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstTresholdExceed))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] =Vector3.zero;
        for(int i = 0;i<vertexCount -1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3 ]=0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();


    }
    ViewCastInfo ViewCast(float globalAngle, float viewRadiusRatio)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius * viewRadiusRatio, ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius * viewRadiusRatio, viewRadius * viewRadiusRatio, globalAngle);
        }
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast,ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector2.zero;

        for(int i = 0; i < edgeResolveIterations; i++)
        {
            float angle= (minAngle+maxAngle)/ 2;
            ViewCastInfo newViewCast = ViewCast(angle,1);

            bool edgeDstTresholdExceed = Mathf.Abs(minViewCast.dist - maxViewCast.dist) > edgeDistanceTreshold;

            if ( newViewCast.hit == minViewCast.hit&& !edgeDstTresholdExceed){
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);

    }


    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;
        public ViewCastInfo(bool _hit,Vector3 _point,float _dist,float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;

        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA,Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }

    }

}



