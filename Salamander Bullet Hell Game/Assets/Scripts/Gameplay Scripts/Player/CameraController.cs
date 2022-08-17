using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    [SerializeField, Range(0, 10)] float cameraZoomMultiplier = .1f;
    [SerializeField, Range(0, 100)] float cameraZoomRate = .1f;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Transform playerTransform;
    [SerializeField] float baseCameraZoom = 5f;
    private bool stopped = false;
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = baseCameraZoom;
    }
    private void OnEnable() => PlayerDeath.playerDeath += Death;

    private void OnDisable() => PlayerDeath.playerDeath -= Death;

    void LateUpdate()
    {
        float camTransformX = Mathf.Clamp(playerTransform.position.x, -25f + (cam.orthographicSize * (16f/9f)), 25f - (cam.orthographicSize * (16f/9f)));
        float camTransformY = Mathf.Clamp(playerTransform.position.y, -25f + cam.orthographicSize, 25f - cam.orthographicSize);
        transform.position = new Vector3(camTransformX, camTransformY, -10);

        if(stopped) return;

        float targetCameraSize = (playerRB.velocity.magnitude * cameraZoomMultiplier) + baseCameraZoom;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCameraSize, Time.deltaTime * cameraZoomRate);
    }
    private void Death()
    {
        stopped = true;
        StartCoroutine(ZoomIn());
    }
    WaitForFixedUpdate fixedUpdateWait = new WaitForFixedUpdate();
    IEnumerator ZoomIn()
    {
        while(true)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 0, Time.fixedDeltaTime);
            yield return fixedUpdateWait;
        }
    }
}
