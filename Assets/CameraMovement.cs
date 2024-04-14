using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 boundsMin = new Vector3(-50, 10, -50);
    private Vector3 boundsMax = new Vector3(50, 50, 50);

    private float zoomSpeed = 200;
    private float movementSpeed = 25;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Zoom based on scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            var desiredMovement = transform.forward * scroll * zoomSpeed * Time.deltaTime;
            var clippedMovement = ClipMovementToBounds(transform.position, desiredMovement, false);
            transform.position += clippedMovement;
        }

        // Move based on arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            var desiredMovement = new Vector3(horizontal, 0, vertical) * movementSpeed * Time.deltaTime;
            var clippedMovement = ClipMovementToBounds(transform.position, desiredMovement, true);
            transform.position += clippedMovement;
        }
    }

    // Note that this implementation is a bit weird because the camera isn't looking directly down the y-axis.
    // This means that on the edges, depending on how far you've zoomed, you'll be allowed to move to a different bound.
    Vector3 ClipMovementToBounds(Vector3 currentPosition, Vector3 movement, bool partialClipping)
    {
        Vector3 projectedPosition = currentPosition + movement;
        Vector3 clippedMovement = movement;

        // Check and clip each component individually
        for (int i = 0; i < 3; i++)
        {
            if (movement[i] == 0) continue;

            float scale = 1.0f;
            if (projectedPosition[i] < boundsMin[i])
            {
                float delta = boundsMin[i] - currentPosition[i];
                scale = Math.Min(1, delta / movement[i]);
            }
            else if (projectedPosition[i] > boundsMax[i])
            {
                float delta = boundsMax[i] - currentPosition[i];
                scale = Math.Min(1, delta / movement[i]);
            }

            if (partialClipping)
            {
                clippedMovement[i] = movement[i] * scale;
            }
            else
            {
                // Apply to clippedMovement and not movement in case multiple axes force the shortening.
                clippedMovement = clippedMovement * scale;
            }
        }

        return clippedMovement;
    }
}
