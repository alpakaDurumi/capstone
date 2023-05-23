using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
public class SliceObject : MonoBehaviour
{
    //public Transform planeDebug;
    //public GameObject target;
    public Material crossSection;
    public Transform cutStartPoint;
    public Transform cutEndPoint;
    public VelocityEstimator velocityEstimator;

    private float cutForce = 1000f;

    public LayerMask sliceableLayer;

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(cutStartPoint.position, cutEndPoint.position, out RaycastHit raycastHit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = raycastHit.collider.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(cutEndPoint.position - cutStartPoint.position, velocity);
        planeNormal.Normalize();
        SlicedHull hull = target.Slice(cutEndPoint.position, planeNormal);
        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target,crossSection);
            SetUpSliceObject(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target,crossSection);
            SetUpSliceObject(lowerHull);
            Destroy(target);
        }
    }
    public void SetUpSliceObject(GameObject sliceObject)
    {
        Rigidbody rb = sliceObject.AddComponent<Rigidbody>();
        MeshCollider collider = sliceObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, sliceObject.transform.position, 1);

    }
}
