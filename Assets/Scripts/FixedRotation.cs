using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotation = new Vector3(90, 0, 0);
    void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
