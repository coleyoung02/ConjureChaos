using System.Collections;
using System.Collections.Generic;
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
        get3();
    }

    public void OnSelected()
    {
        int children = row.transform.childCount;
        for (int i = children - 1; i >= 0; --i)
        {
            Destroy(row.transform.GetChild(i).gameObject);
        }
        gameObject.SetActive(false);
    }

    public void get3()
    {
        int index;
        for (int i = 0; i < 3; ++i)
        {
            index = UnityEngine.Random.Range(0, upgrades.Count);
            Instantiate(upgrades[index], row.transform);
            upgrades.RemoveAt(index);
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
