using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimpleRotator : MonoBehaviour
{
    public Vector3 Axis = Vector3.up;

    void Update()
    {
        transform.Rotate( Axis, 360 * Time.deltaTime );
    }
}
