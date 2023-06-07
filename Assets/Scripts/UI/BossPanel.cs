using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    public ActorData boss;

    public void Activate(ActorData boss)
    {
        this.boss = boss;
        Refresh();
    }

    public void Refresh()
    {
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = boss.prototype.name;
        transform.Find("Health").GetComponent<CurrentMaxBar>().SetValues(boss.Health_current,boss.GetHealthMax());
    }
}
