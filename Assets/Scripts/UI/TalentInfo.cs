using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TalentInfo : MonoBehaviour
{
    public TalentData talent_data;
    public bool check_screen_position = true;

    RectTransform rect;
    public Vector2 start_position;

    // Start is called before the first frame update
    void Start()
    {
        Texture2D texture1 = Resources.Load<Texture2D>(talent_data.prototype.icon);
        transform.Find("Icon").GetComponent<Image>().sprite =
            Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.name;
        transform.Find("Type").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.type.ToString();
  

        transform.Find("Target").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.target.ToString();
        transform.Find("Range").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.target_range.ToString();

        transform.Find("Cooldown").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.cooldown.ToString();
        transform.Find("StaminaCost").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.cost_stamina.ToString();
        //transform.Find("ManaCost").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.cost_mana.ToString();
        transform.Find("PrepareTime").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.prepare_time.ToString();
        transform.Find("RecoverTime").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.recover_time.ToString();

        transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = talent_data.prototype.description;

        start_position.x = Mouse.current.position.x.ReadValue();
        start_position.y = Mouse.current.position.y.ReadValue();
    }

    public void CheckScreenPosition()
    {
        if (check_screen_position == false) return;
        
        rect = GetComponent<RectTransform>();

        if (rect.position.x < 0)
        {
            rect.position = new Vector3(0, rect.position.y, rect.position.z);
            return;
        }

        if (rect.position.x + rect.sizeDelta.x > Screen.width)
        {
            rect.position = new Vector3(Screen.width - rect.sizeDelta.x, rect.position.y, rect.position.z);
            return;
        }

        if (rect.position.y > Screen.height)
        {
            rect.position = new Vector3(rect.position.x, Screen.height, rect.position.z);
            return;
        }

        if (rect.position.y - rect.sizeDelta.y < 0)
        {
            rect.position = new Vector3(rect.position.x, rect.sizeDelta.y + 10, rect.position.z);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Mouse.current.position.ReadValue().x - start_position.x) > 150
        || Mathf.Abs(Mouse.current.position.ReadValue().y - start_position.y) > 150)
        {
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(talent_data);
        }
    }

    void FixedUpdate()
    {
        CheckScreenPosition();
    }
}
