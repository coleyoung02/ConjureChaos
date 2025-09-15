using UnityEngine;

public class VoidPortalSpin : MonoBehaviour
{
    [SerializeField] private float rate;
    private float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * rate;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
