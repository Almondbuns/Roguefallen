using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorDeathScript : MonoBehaviour
{
    float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time < .5f)
            transform.Rotate(Vector3.forward, 15 * Time.deltaTime);

        if (time < 0.5f)
            transform.position += new Vector3(0, 0.01f * Time.deltaTime, 0);

        gameObject.GetComponent<SpriteRenderer>().color -= new Color(Time.deltaTime, Time.deltaTime, Time.deltaTime, 0.1f * Time.deltaTime);
    }
}
