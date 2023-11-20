using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> upgrades;
    [SerializeField] GameObject row;
    [SerializeField] TextMeshProUGUI description;

    void OnEnable()
    {
        Time.timeScale = 0;
        get3();
    }

    public void OnSelected(Upgrade u)
    {
        Debug.Log("REMOVING " + u + " i " + u.GetIndex());
        upgrades.RemoveAt(u.GetIndex());
        int children = row.transform.childCount;
        for (int i = children - 1; i >= 0; --i)
        {
            Destroy(row.transform.GetChild(i).gameObject);
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void get3()
    {
        upgrades = upgrades.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < 3; ++i)
        {
            GameObject g = Instantiate(upgrades[i], row.transform);
            g.GetComponent<Upgrade>().SetIndex(i);
        }
    }

    public void SetDescription(string desc)
    {
        description.text = desc;
        description.gameObject.SetActive(true);
    }

    public void Clear()
    {
        description.gameObject.SetActive(false);
        description.text = "";
    }
}
