using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float maxYMoveSpeed;
    [SerializeField] private float minY;
    [SerializeField] private float yOffset;
    [SerializeField] private float maxY;
    [SerializeField] private float maxXSize;
    [SerializeField] private float aimWeight;
    [SerializeField] private float maxXGap;
    [SerializeField] private float shakeTargetSize;
    private GameObject player;
    private PlayerMovement playerMovement;
    private float zOffset;
    private Camera cam;
    private Vector3 lastPos;
    private Vector3 playerPos;
    private float baseSize;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        player = playerMovement.gameObject;
        zOffset = transform.position.z;
        cam = GetComponent<Camera>();
        baseSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.timeScale > .2f)
        {
            SetPos(player.transform.position);
        }
    }

    private IEnumerator Resume()
    {
        for (float f = 0f; f < .35f; f += Time.unscaledDeltaTime)
        {
            cam.orthographicSize = Mathf.Lerp(baseSize, shakeTargetSize, f / .35f);
            transform.position = Vector3.Lerp(lastPos, (playerPos + lastPos * 2) / 3, f / .35f);
            if (f > .15f)
            {
                Shake();
            }
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = .1f;
        cam.orthographicSize = shakeTargetSize;
        for (float f = 0f; f < .3f; f += Time.unscaledDeltaTime)
        {
            Shake();
            yield return new WaitForEndOfFrame();
        }
        for (float f = 0f; f < .3f; f += Time.unscaledDeltaTime)
        {
            cam.orthographicSize = Mathf.Lerp(shakeTargetSize, baseSize, f / .3f);
            transform.position = Vector3.Lerp((playerPos + lastPos * 2) / 3, lastPos, f / .3f);
            if (f < .15f)
            {
                Shake();
            }
            yield return new WaitForEndOfFrame();
        }
        cam.orthographicSize = baseSize;
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 1;
    }

    private void Shake()
    {
        Vector3 v = (playerPos + lastPos * 2) / 3f;
        v.x += UnityEngine.Random.Range(-.25f, .25f);
        v.y += UnityEngine.Random.Range(-.25f, .25f);
        transform.position = v;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ForceStop()
    {
        StopAllCoroutines();
    }

    private void SetPos(Vector3 playerPos)
    {
        Vector2 aimPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        float x = playerPos.x;
        float y = playerPos.y + yOffset;
        Vector2 aimDist = aimPoint - new Vector2(x, y);

        x += aimDist.x * aimWeight;
        y += aimDist.y * aimWeight;
        y = Mathf.Clamp(y, transform.position.y - maxYMoveSpeed * Time.deltaTime * 2.5f, transform.position.y + maxYMoveSpeed * Time.deltaTime);
        x = Mathf.Clamp(x, -maxXSize, maxXSize);
        y = Mathf.Clamp(y, minY, maxY);
        transform.position = new Vector3(x, y, zOffset);
    }

    public void TakeDamage()
    {
        lastPos = transform.position;
        playerPos = player.transform.position;
        playerPos.z = lastPos.z;
        Time.timeScale = 0f;
        StopAllCoroutines();
        FindAnyObjectByType<AudioManager>().PitchDown(1.5f);
        StartCoroutine(Resume());
    }
}
