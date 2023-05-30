using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Important - Object destroys parent when selfremoving!

public class FloatingInfo : MonoBehaviour
{
    public bool is_floating = true;
    public bool is_scaling = false;
    public float max_time_alive = 2.0f;

    Vector2 size;
    bool size_increase = true;
    float position_x;
    float time_alive;
    // Start is called before the first frame update
    void Start()
    {
        size = new Vector2(0.1f, 0.1f);
        position_x = GetComponent<RectTransform>().localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        time_alive += Time.deltaTime;

        if (is_scaling == true)
        {
            if (size_increase == true)
            {
                size += new Vector2(0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
            }
            if (size.x > 0.4f)
            {
                size_increase = false;
                size = new Vector2(0.4f, 0.4f);
            }
            if (size_increase == false)
            {
                size -= new Vector2(0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
            }
            if (size.x < 0.0f)
            {
                size = new Vector2(0.0f, 0.0f);
                Destroy(transform.parent.gameObject);
                return;
            }

            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = size;
        }

        if (is_floating == true)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.localPosition = new Vector3(position_x + 0.1f * Mathf.Sin(3 * time_alive), rect.localPosition.y + 0.75f * Time.deltaTime, rect.localPosition.z);
        }

        if (time_alive > max_time_alive)
            Destroy(transform.parent.gameObject);

    }
}
