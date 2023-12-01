using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> upgrades;
    [SerializeField] private GameObject row;
    [SerializeField] private GameObject drug;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private AudioSource upgradeSFX; 

    public void GetUpgrades()
    {
        description.text = "";
        Time.timeScale = 0;
        get3();
    }

    public void OnSelected(Upgrade u)
    {
        Debug.Log("REMOVING " + u + " i " + u.GetIndex());
        if (upgrades.Count > u.GetIndex())
            upgrades.RemoveAt(u.GetIndex());
        int children = row.transform.childCount;
        for (int i = children - 1; i >= 0; --i)
        {
            Destroy(row.transform.GetChild(i).gameObject);
        }
        Time.timeScale = 1;
        upgradeSFX.Play();
        gameObject.SetActive(false);
    }

    public void Drugs()
    {
        upgrades = new List<GameObject>();
        for (int i = 0; i < 100; ++i)
        {
            upgrades.Add(drug);
        }
    }

    public void get3()
    {
        //upgrades = upgrades.OrderBy(x => Random.value).ToList();
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
