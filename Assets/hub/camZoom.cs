using UnityEngine;

public class CameraFollowAndZoom : MonoBehaviour {
    public Transform target; // The object the camera will follow
    private float tilt = 30f; // degrees to tilt when zoomed out (positive tilts down)
    private const float TILTPERLEVEL = 15f;
    public Vector3 revFromPos;
    private float followSpeed = 5f; // Speed at which the camera follows the target
    private float zoomDuration = 0.5f; // Time in seconds to zoom in fully
    private float tiltDuration = 4f; // Time to slow tilt on level up

    private Camera cam; // Reference to the camera component
    private Vector3 initPos;
    private float targetZoom; // The zoom level the camera is moving towards
    private bool isZooming = false, isTilting = false, tracking = false, revTracking = false; // Flag to indicate if zooming is in progress
    private float zoomStartTime; // Time when zoom started
    private float initialZoom; // Initial zoom level at the start of zoom
    private float initialTilt; // Initial camera X rotation at start of zoom
    private float targetTilt; // Target camera X rotation while zoomed out

    void Start() {
        tilt = TILTPERLEVEL * gameSpeed.currentDifficulty;
        cam = GetComponent<Camera>();
        if (cam == null) {
            Debug.LogError("Camera component not found!");
            return;
        }
        initPos = cam.transform.position;
        targetZoom = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    // store initial tilt (X rotation) in degrees
    initialTilt = cam.transform.eulerAngles.z;
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
            PerformZoom(isTilting);
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

        float computedTargetZoom;
        if (cam.orthographic) {
            computedTargetZoom = objectSize / 3.6f;
        }
        else {
            float fovRadians = Mathf.Deg2Rad * cam.fieldOfView / 2f;
            computedTargetZoom = 2f * distance * Mathf.Tan(fovRadians);
        }
        // capture current tilt and compute target tilt based on natural direction
        initialTilt = cam.transform.eulerAngles.z;
        if (isTilting) targetTilt = initialTilt - TILTPERLEVEL;
        else targetTilt = initialTilt + (reverse ? -tilt : tilt);

        // assign the computed target zoom (actual swap may happen below for reverse)
        targetZoom = computedTargetZoom;
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
    public void StartSlowTilt() //BROKEN DONT USE
    {
        isTilting = true;
        tilt = TILTPERLEVEL * gameSpeed.currentDifficulty;
        StartZoomToTarget(reverse:true);
    }
    private void PerformZoom(bool tiltOnly = false)
    {
        float elapsed = Time.time - zoomStartTime;
        float t = Mathf.Clamp01(elapsed / (tiltOnly ? tiltDuration : zoomDuration));
        float newZ = Mathf.LerpAngle(initialTilt, targetTilt, t);
        Vector3 e = cam.transform.eulerAngles;
        cam.transform.eulerAngles = new Vector3(e.x, e.y, newZ);
        if (t >= 1f)
        {
            isZooming = false;
            isTilting = false;
        }
        if (tiltOnly) return;
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, t);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(initialZoom, targetZoom, t);
        }
    }
}
