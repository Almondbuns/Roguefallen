using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArrowPulse : MonoBehaviour
{
    bool up_scale = false;
    float scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (up_scale == false)
        {
            scale -= Time.deltaTime;
            if (scale <= 0.25)
                up_scale = true;
        }
        else
        {
            scale += Time.deltaTime;
            if (scale >= 1)
                up_scale = false;
        }
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
