using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgrades;
    private List<GameObject> row;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject drug;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI upgradeNameField;
    [SerializeField] private AudioClip hovered;
    [SerializeField] private AudioClip selected;
    [SerializeField] private AudioSource actived;

    private void Awake()
    {
        row = new List<GameObject> { left, center, right };
    }

    public void PlayButtonHovered()
    {
        FindAnyObjectByType<AudioManager>().PlayUIClip(hovered, true);
    }

    public void GetUpgrades()
    {
        description.text = "";
        FindAnyObjectByType<CameraManager>().ForceStop();
        Time.timeScale = 0;
        FindAnyObjectByType<AudioManager>().SetFilter(true);
        get3();
    }

    public void OnSelected(Upgrade u)
    {
        if (upgrades.Count > u.GetIndex())
            upgrades.RemoveAt(u.GetIndex());
        int children = row.Count;

        for (int i = children - 1; i >= 0; --i)
        {
            Destroy(row[i].transform.GetChild(0).gameObject);
        }
        Time.timeScale = 1;
        FindAnyObjectByType<AudioManager>().PlayUIClip(selected);
        FindAnyObjectByType<AudioManager>().SetFilter(false);
        Clear();
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

    private bool CheckRepeats(List<GameObject> u)
    {
        return (string.Equals(upgrades[0].name, upgrades[1].name) ||
                string.Equals(upgrades[0].name, upgrades[2].name) ||
                string.Equals(upgrades[1].name, upgrades[2].name));
    }

    private bool CheckForHeals(List<GameObject> u)
    {
        return (upgrades[0].GetComponent<Heal>() != null) ||
                    (upgrades[1].GetComponent<Heal>() != null) ||
                    (upgrades[2].GetComponent<Heal>() != null);
    }

    public void get3()
    {
        actived.Play();
        bool isMaxHealth = FindAnyObjectByType<PlayerHealth>().IsAtMax();
        upgrades = upgrades.OrderBy(x => Random.value).ToList();
        int iters = 0;
        // do not give duplicates, and do not give healing while at full health
        while (iters < 50 && (CheckRepeats(upgrades) || (isMaxHealth && CheckForHeals(upgrades))) )
        {
            ++iters;
            upgrades = upgrades.OrderBy(x => Random.value).ToList();
        }
        for (int i = 0; i < 3; ++i)
        {
            GameObject g = Instantiate(upgrades[i], row[i].transform);
            g.GetComponent<Upgrade>().SetIndex(i);
        }
    }

    public void SetDescription(string desc, string name)
    {
        description.text = desc;
        description.gameObject.SetActive(true);
        this.upgradeNameField.text = name;
        this.upgradeNameField.gameObject.SetActive(true);
    }

    public void Clear()
    {
        description.gameObject.SetActive(false);
        description.text = "";
        upgradeNameField.gameObject.SetActive(false);
        upgradeNameField.text = "";
    }
}
