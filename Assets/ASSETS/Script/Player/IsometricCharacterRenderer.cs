using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCharacterRenderer : MonoBehaviour
{
    public static readonly string[] staticDirections =
{
    "Idle_Up",
    "Idle_UpRight",
    "Idle_Right",
    "Idle_DownRight",
    "Idle_Down",
    "Idle_DownLeft",
    "Idle_Left",
    "Idle_UpLeft"
};

    public static readonly string[] movingDirections =
    {
    "Move_Up",
    "Move_UpRight",
    "Move_Right",
    "Move_DownRight",
    "Move_Down",
    "Move_DownLeft",
    "Move_Left",
    "Move_UpLeft"
};


    Animator animator;
    int lastDirection = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 direction)
    {
        string[] directionArray;

        // Jika diam → idle
        if (direction.magnitude < 0.01f)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = movingDirections;
            lastDirection = DirectionToIndex(direction, directionArray.Length);
        }

        string stateName = directionArray[lastDirection];
        animator.Play(stateName);
    }

    // Mengubah vector gerakan menjadi index animasi
    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normDir = dir.normalized;
        float step = 360f / sliceCount;
        float halfStep = step / 2f;

        float angle = Vector2.SignedAngle(Vector2.up, normDir);
        angle += halfStep;

        if (angle < 0)
            angle += 360f;

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}
