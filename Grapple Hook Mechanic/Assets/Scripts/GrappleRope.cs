using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    public Transform gunPoint;
    public Transform grapplePoint;

    private LineRenderer lineRenderer;

    [SerializeField] private AnimationCurve animationCurve;

    [SerializeField] private AnimationCurve progressionCurve;

    private int numPointsOnCurve = 40;

    [SerializeField] private float waveHeight = 2f;
    private float activeTime = 0f;
    private float progressionSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        activeTime = 0f;
        lineRenderer.positionCount = numPointsOnCurve;

        InitGrappleLine();

    }

    // Update is called once per frame
    void Update()
    {
        activeTime += Time.deltaTime;
        RenderRope();
    }

    void InitGrappleLine()
    {
        for (int i = 0; i < numPointsOnCurve; i++)
        {
            lineRenderer.SetPosition(i, gunPoint.position);
        }
    }

    void RenderRope()
    {
        RenderRopeWaves();
    }

    void RenderRopeWaves()
    {
        for (int i = 0; i < numPointsOnCurve; ++i)
        {
            float deltaPosition = (float)i / ((float)numPointsOnCurve - 1f);

            Vector2 offset = Vector2.Perpendicular(grapplePoint.position).normalized *
                                animationCurve.Evaluate(deltaPosition) *
                                waveHeight;

            Vector2 target = Vector2.Lerp(gunPoint.position,
                                grapplePoint.position,
                                deltaPosition) + offset;

            Vector2 current = Vector2.Lerp(gunPoint.position,
                                target,
                                progressionCurve.Evaluate(activeTime) * progressionSpeed);

            lineRenderer.SetPosition(i, current);
        }
    }
}
