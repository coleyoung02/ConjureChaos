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
    private float lerpClock = 0f;
    private float maxScale = 1.15f;
    private float changeRate = 7.5f;
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
            lerpClock = Mathf.Clamp(lerpClock + changeRate * Time.unscaledDeltaTime, 0, 1);
        }
        else
        {
            lerpClock = Mathf.Clamp(lerpClock - changeRate * Time.unscaledDeltaTime * 1.5f, 0, 1);
        }
        rt.localScale = Vector3.Slerp(new Vector3(1, 1, 1), new Vector3(maxScale, maxScale, maxScale), lerpClock);
    }

    private void OnEnable()
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpgradeManager uMan = FindObjectOfType<UpgradeManager>();
        uMan.PlayButtonHovered();
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
