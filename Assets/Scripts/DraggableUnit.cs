using UnityEngine;
using System;

public class UnitDragHandler : MonoBehaviour
{
    public event Action<Vector3> MouseDragged;
    public event Action MouseDown;

    private Vector3 dragStartTransformPosition;
    public Vector3 DragStartTransformPosition => dragStartTransformPosition;
    private float startZCoord;

    /// <summary>
    /// Get the mouse position in world coordinates based on the dragged object
    /// </summary>
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = startZCoord;
        // Convert it to world points
        return ProjectOntoGround(Camera.main.ScreenToWorldPoint(mousePoint));
    }

    public Vector3 ProjectOntoGround(Vector3 coordinates)
    {
        // Cast a ray from the coordinates downward
        Ray ray = new(coordinates + Vector3.up * 1000f, Vector3.down);

        // Check for intersection with the ground plane collider
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point; // Return the intersection point
        }
        else
        {
            return coordinates; // If no intersection, return the original coordinates
        }
    }

    /// <summary>
    /// Save the object's start position when it is clicked
    /// </summary>
    public void OnMouseDown()
    {
        MouseDown?.Invoke();
        SaveStartPos();
    }

    public void SaveStartPos()
    {
        startZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        dragStartTransformPosition = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    public void OnMouseDrag()
    {
        MouseDragged?.Invoke(GetMouseAsWorldPoint());
    }
}
