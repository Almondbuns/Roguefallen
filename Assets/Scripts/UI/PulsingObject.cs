using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingObject : MonoBehaviour
{
    Vector2 start_size;
    // Start is called before the first frame update
    void Start()
    {
        start_size = GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().sizeDelta = start_size * (0.5f + 0.5f * Mathf.Abs(Mathf.Sin(2 * Time.realtimeSinceStartup)));
    }
}
