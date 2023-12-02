using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Stats
{
    Damage = 0,
    Speed = 1,
    Range = 2,
    Rate = 3,
    Size = 4
}

public abstract class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string description;
    private Button button;
    private int index;


    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex()
    {
        return this.index;
    }

    protected void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DoUpgrade);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradeManager uMan = FindObjectOfType<UpgradeManager>();
        uMan.SetDescription(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<UpgradeManager>().Clear();
    }

    private void destruction()
    {
        UpgradeManager uMan = FindObjectOfType<UpgradeManager>();
        uMan.OnSelected(this);
    }

    public virtual void DoUpgrade()
    {
        destruction();
    }
}
