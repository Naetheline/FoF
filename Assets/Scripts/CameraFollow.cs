using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Tuto by xOctoManx : 
// https://www.youtube.com/watch?v=Eyga3DzFZo8


/*
 * Camera that follow the player around be retain the same angle.
 *
 */
public class CameraFollow : MonoBehaviour
{
    public const float MIN_ZOOM = 5f;
    public const float MAX_ZOOM = 15f;

    public Transform target;

    public float distance = 10f;
    public Vector3 offset;

    public float speed = 5f;
    public float scrollSensitivity = 2;

    
    void LateUpdate()
    {
        if(target == null)
        {
            Debug.LogError("Camera has no target.");
            return;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSensitivity;
        distance = Mathf.Clamp(distance, MIN_ZOOM, MAX_ZOOM);

        Vector3 position = target.position + offset;
        position -= transform.forward * distance;

        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);

    }
}
