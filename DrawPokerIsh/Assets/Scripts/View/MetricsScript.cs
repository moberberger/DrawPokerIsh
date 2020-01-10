using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class MetricsScript : MonoBehaviour
{
    public float Width;
    public float Height;

    public float X;
    public float Y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rect = (RectTransform) transform;

        Width = rect.rect.width;
        Height = rect.rect.height;
        X = rect.position.x;
        Y = rect.position.y;
    }
}
