using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSphere : MonoBehaviour
{
    private float _range;
    private Color _color;
    private Material _mat;
    private float _startTime;
    void TheStart()
    {
        _mat = GetComponent<Renderer>().material;
        _mat.SetColor("_Color", _color);

        this.transform.localScale = new Vector3(_range * 2, _range * 2, _range * 2);
        _startTime = Time.time;
    }
    void Update()
    {
        float i = Time.time - _startTime;
        if (i < 1) // makes Progress property of shader go from 0.2 to 0.5 aprox in the span of 1 second (we don't need the lower half because it won't be visible
        {
            _mat.SetFloat("_DissolveProgress", 0.2f + i/2); // 0.2f to start as soon as possible,
                                                            // because until that point the sphere doesn't show the rings
                                                            // i/2 to go slower
        }
        else // when a second has passed, destroy the gameObject
        {
            Destroy(gameObject);
        }
    }
    public void SetRangeAndColor(float range, Color color)
    {
        _range = range;
        _color = color;
    }
}
