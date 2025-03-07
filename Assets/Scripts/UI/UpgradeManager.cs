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
    [SerializeField] private GameObject reroll;
    private List<GameObject> lastSeen;

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
        if (FindAnyObjectByType<ProjectileConjurer>().GetProjectileEffects()
            .Contains(ProjectileConjurer.ProjectileEffects.Gambling))
        {
            reroll.SetActive(true);
        }
        description.text = "";
        FindAnyObjectByType<CameraManager>().ForceStop();
        Time.timeScale = 0;
        FindAnyObjectByType<AudioManager>().SetFilter(true);
        actived.Play();
        lastSeen = new List<GameObject>();
        lastSeen = get3(lastSeen);
    }

    public void OnSelected(Upgrade u)
    {
        if (upgrades.Count > u.GetIndex())
            upgrades.RemoveAt(u.GetIndex());
        Time.timeScale = 1;
        ClearCurrent();
        FindAnyObjectByType<AudioManager>().PlayUIClip(selected);
        FindAnyObjectByType<AudioManager>().SetFilter(false);
        gameObject.SetActive(false);
    }

    private void ClearCurrent()
    {
        Clear();
        int children = row.Count;
        for (int i = children - 1; i >= 0; --i)
        {
            Destroy(row[i].transform.GetChild(0).gameObject);
        }
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

    private bool CheckOverlap(List<GameObject> u, List<GameObject> v)
    {
        for (int i = 0; i < u.Count; ++i)
        {
            for (int j = 0; j < v.Count; ++j)
            {
                if (string.Equals(u[i].name, v[j].name))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckForHeals(List<GameObject> u)
    {
        return (upgrades[0].GetComponent<Heal>() != null) ||
                    (upgrades[1].GetComponent<Heal>() != null) ||
                    (upgrades[2].GetComponent<Heal>() != null);
    }

    private bool OverlapRerollCriteria(bool isReroll)
    {
        return CheckOverlap(lastSeen, upgrades.GetRange(0, 3)) &&
                    (isReroll || UnityEngine.Random.Range(0f, 1f) < .2f);
    }

    public List<GameObject> get3(List<GameObject> lastSeen, bool isReroll=false)
    {
        bool isMaxHealth = FindAnyObjectByType<PlayerHealth>().IsAtMax();
        bool isNearDeath = FindAnyObjectByType<PlayerHealth>().NearDeath();
        //upgrades = upgrades.OrderBy(x => Random.value).ToList();
        int iters = 0;
        // do not give duplicates, and do not give healing while at full health
        while (iters < 75 && (CheckRepeats(upgrades) || (isMaxHealth && CheckForHeals(upgrades)) || 
            (isNearDeath && !CheckForHeals(upgrades) && UnityEngine.Random.Range(0f, 1f) < .75f) ||
                OverlapRerollCriteria(isReroll) ) )
        {
            ++iters;
            //upgrades = upgrades.OrderBy(x => Random.value).ToList();
        }
        for (int i = 0; i < 3; ++i)
        {
            GameObject g = Instantiate(upgrades[i], row[i].transform);
            g.GetComponent<Upgrade>().SetIndex(i);
        }
        return upgrades.GetRange(0, 3);
    }

    public void ReRoll()
    {
        ClearCurrent();
        lastSeen = get3(lastSeen);
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
