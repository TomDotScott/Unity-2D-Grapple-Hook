using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    public enum ERopeState
    {
        eFiring,
        eStraightening,
        eStraight
    }

    public ERopeState RopeState;

    public Transform GunPoint;
    public Transform GrapplePoint;

    private LineRenderer lineRenderer;

    [SerializeField] private AnimationCurve animationCurve;

    [SerializeField] private AnimationCurve progressionCurve;

    private int numPointsOnCurve = 40;

    [SerializeField] private float waveHeight = 2f;
    private float currentWaveHeight;

    private float activeTime = 0f;

    [SerializeField] private float progressionSpeed = 1f;

    [SerializeField] private float straightenSpeed;

    private void OnEnable()
    {
        InitGrappleRope();
    }

    // Update is called once per frame
    private void Update()
    {
        activeTime += Time.deltaTime;
        RenderRope();
    }

    private void InitGrappleRope()
    {
        RopeState = ERopeState.eFiring;
        currentWaveHeight = waveHeight;
        lineRenderer = GetComponent<LineRenderer>();

        activeTime = 0f;
        lineRenderer.positionCount = numPointsOnCurve;

        for (int i = 0; i < numPointsOnCurve; i++)
        {
            lineRenderer.SetPosition(i, GunPoint.position);
        }
    }

    private void RenderRope()
    {
        switch (RopeState)
        {
            case ERopeState.eFiring:
                var lineRendererPosition = lineRenderer.GetPosition(numPointsOnCurve - 1).x;
                var gunPosition = GunPoint.position.x;


                if (lineRenderer.GetPosition(numPointsOnCurve - 1).x == GrapplePoint.position.x)
                {
                    RopeState = ERopeState.eStraightening;
                }
                RenderRopeWaves();
                break;
            case ERopeState.eStraightening:
                // When we hit the target, we want to make the waves smaller until they appear straight
                if (currentWaveHeight > 0f)
                {
                    currentWaveHeight -= Time.deltaTime * straightenSpeed;
                    RenderRopeWaves();
                }
                else
                {
                    // Reset the line to be straight
                    currentWaveHeight = 0f;
                    lineRenderer.positionCount = 2;
                    RopeState = ERopeState.eStraight;
                    RenderRopeStraight();
                }
                break;
            case ERopeState.eStraight:
                RenderRopeStraight();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RenderRopeStraight()
    {
        // Set the start and end points to be the grapple point and gun
        lineRenderer.SetPosition(0, GunPoint.position);
        lineRenderer.SetPosition(1, GrapplePoint.position);
    }

    private void RenderRopeWaves()
    {
        for (int i = 0; i < numPointsOnCurve; ++i)
        {
            float deltaPosition = i / (numPointsOnCurve - 1f);

            Vector2 offset = Vector2.Perpendicular(GrapplePoint.position).normalized *
                                animationCurve.Evaluate(deltaPosition) *
                                currentWaveHeight;

            Vector2 target = Vector2.Lerp(GunPoint.position,
                                GrapplePoint.position,
                                deltaPosition) + offset;

            Vector2 current = Vector2.Lerp(GunPoint.position,
                                target,
                                progressionCurve.Evaluate(activeTime) * progressionSpeed);

            lineRenderer.SetPosition(i, current);
        }
    }
}
