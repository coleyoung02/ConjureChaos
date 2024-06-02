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
    Size = 4,
    Accuracy = 5,
    ShotCount = 6,
}

public abstract class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected string description;
    [SerializeField] protected string name;
    private Button button;
    private int index;

    private bool hovered = false;
    private float scale = 1f;
    private float maxScale = 1.05f;
    private float changeRate = .4f;
    private RectTransform rt;

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
        rt = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(DoUpgrade);
    }

    void Update()
    {
        if (hovered)
        {
            scale = Mathf.Clamp(scale + changeRate * Time.deltaTime, 1, maxScale);
        }
        else
        {
            scale = Mathf.Clamp(scale - changeRate * Time.deltaTime, 1, maxScale);
        }
        rt.localScale = new Vector3(scale, scale, scale);
    }

    private void OnEnable()
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradeManager uMan = FindObjectOfType<UpgradeManager>();
        uMan.SetDescription(description, name);
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        FindObjectOfType<UpgradeManager>().Clear();
        hovered = false;
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
