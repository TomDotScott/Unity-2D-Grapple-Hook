using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGun : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GrappleRope grappleRope;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform grapplePoint;

    [Range(1, 10)][SerializeField] private float maxDistance;
    private Vector3 grappleDistance;

    [SerializeField] private LayerMask grappleLayer;

    [SerializeField] private bool showDebugInfo = true;

    private bool firing = false;

    // Start is called before the first frame update
    void Start()
    {
        grappleRope.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firing = Fire();
        }
        else if (Input.GetButton("Fire1"))
        {
            // TODO: Keep the rope active whilst mouse is held down and pull player closer to the grapple point
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            firing = false;
        }

        grappleRope.gameObject.SetActive(firing);
    }

    private bool Fire()
    {
        // Get the mouse position from the camera and fire a ray out to see
        // if we hit anything in game
        Vector2 d = mainCamera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, d.normalized, maxDistance, grappleLayer);
        if (hit && Vector2.Distance(hit.point, firePoint.position) < maxDistance)
        {
            grapplePoint.position = hit.point;
            grappleDistance = grapplePoint.position - firePoint.position;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (showDebugInfo)
        {
            // Draw the radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);

            if (firing && grappleRope.RopeState != GrappleRope.ERopeState.eStraight)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(firePoint.position, grapplePoint.position);
            }
        }
    }
}
