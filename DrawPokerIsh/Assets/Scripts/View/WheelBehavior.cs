using System.Collections;
using System.Collections.Generic;
using Morpheus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

using DG.Tweening;


public class WheelBehavior : MonoBehaviour
{
    public TextMeshProUGUI[] Segments;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var segment in Segments)
            segment.text = (Rng.Default.Next( 20 ) * 5).ToString();

        transform.DORotate( new Vector3( 0, 0, 360 * 4 + Rng.Default.Next( 360 ) ), 5, RotateMode.FastBeyond360 )
            .SetEase( Ease.InOutCirc );
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate( new Vector3( 0, 0, -5 ) );
    }
}
