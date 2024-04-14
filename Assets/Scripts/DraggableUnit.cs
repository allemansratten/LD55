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
        return Camera.main.ScreenToWorldPoint(mousePoint);
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
