using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GrappleRope grappleRope;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform grapplePoint;

    [Range(5, 100)][SerializeField] private float maxDistance;
    private Vector3 grappleDistance;


    // Start is called before the first frame update
    void Start()
    {
        grappleRope.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SetGrapplePoint();
        }
    }

    private void SetGrapplePoint()
    {
        // Get the mouse position from the camera and fire a ray out to see
        // if we hit anything in game
        Vector2 d = mainCamera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;

        if (Physics2D.Raycast(firePoint.position, d.normalized))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, d.normalized);
            if (Vector2.Distance(hit.point, firePoint.position) < maxDistance)
            {
                grapplePoint.position = hit.point;
                grappleDistance = grapplePoint.position - firePoint.position;
                grappleRope.enabled = true;
            }
        }
    }
}
