using UnityEngine;

public class CameraFollowAndZoom : MonoBehaviour {
    public Transform target; // The object the camera will follow
    public Vector3 revFromPos;
    private float followSpeed = 5f; // Speed at which the camera follows the target
    private float zoomDuration = 0.5f; // Time in seconds to zoom in fully

    private Camera cam; // Reference to the camera component
    private Vector3 initPos;
    private float targetZoom; // The zoom level the camera is moving towards
    private bool isZooming = false, tracking = false, revTracking = false; // Flag to indicate if zooming is in progress
    private float zoomStartTime; // Time when zoom started
    private float initialZoom; // Initial zoom level at the start of zoom

    void Start() {
        cam = GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("Camera component not found!");
            return;
        }
        initPos = cam.transform.position;
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }

    void Update() {
        if (target != null && tracking) {
            // Follow the target (ADJUST FOR CENTER)
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + new Vector3(-0.4f, 0.7f, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        if (target != null && revTracking) {
            transform.position = Vector3.Lerp(transform.position, initPos, followSpeed * Time.deltaTime);
        }
        if (isZooming) {
            PerformZoom();
        }
    }
    public void ZoomIn() {
        StartZoomToTarget(false);
    }
    public void ZoomOut() {
        StartZoomToTarget(true);
    }
    private void StartZoomToTarget(bool reverse=false) {
        if (target == null) return;
        tracking = true;
        revTracking = false;
        isZooming = true;
        zoomStartTime = Time.time;
        initialZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;

        // Calculate the target zoom level based on the target bounds
        Vector3 targetSize = target.GetComponent<Renderer>().bounds.size;
        float distance = Vector3.Distance(transform.position, target.position);
        float objectSize = Mathf.Min(targetSize.x, targetSize.y);

        if (cam.orthographic) {
            targetZoom = objectSize / 3.6f;
        }
        else {
            float fovRadians = Mathf.Deg2Rad * cam.fieldOfView / 2f;
            targetZoom = 2f * distance * Mathf.Tan(fovRadians);
        }
        if(reverse) {
            float temp = targetZoom;
            targetZoom = initialZoom;
            initialZoom = temp;
            tracking = false;
            revTracking = true;
            cam.transform.position = revFromPos;
        }
        //targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }
    private void PerformZoom() {
        float elapsed = Time.time - zoomStartTime;
        float t = Mathf.Clamp01(elapsed / zoomDuration);
        if (cam.orthographic) {
            cam.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, t);
        }
        else {
            cam.fieldOfView = Mathf.Lerp(initialZoom, targetZoom, t);
        }

        if (t >= 1f) {
            isZooming = false;
        }
    }
}
