using UnityEngine;
using TMPro;

public class cursorTrack : MonoBehaviour
{
    public RectTransform canvasRect; // Assign the Canvas in the Inspector
    public RectTransform stopRadiusCircle; // UI Image for the stop radius
    public TextMeshProUGUI rotationText; // UI Text to display rotation info
    public float stopRadius = 50f; // Stop drag when this close to center (in UI pixels)

    private bool isDragging = false;
    private Vector2 lastMouseDirection;
    public float thisDragAngle, storedAngle;
    private Vector2 uiCenter; // UI-space center of the canvas

    void Start()
    {
        UpdateUICenter();
        UpdateStopRadiusCircle();
        UpdateRotationText();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click starts tracking
        {
            StartDragging();
        }

        if (isDragging && Input.GetMouseButton(0)) // While holding the mouse button
        {
            UpdateDragging();
        }

        if (Input.GetMouseButtonUp(0)) // Stop dragging when released
        {
            isDragging = false;
            storedAngle += thisDragAngle;
            thisDragAngle = 0f;
            UpdateRotationText();
        }
    }

    void StartDragging()
    {
        Vector2 mousePos = GetMouseUIPosition();
        Vector2 toMouse = mousePos - uiCenter; // Now correctly centered

        if (toMouse.magnitude < stopRadius) return; // Don't start if too close

        isDragging = true;
        lastMouseDirection = toMouse.normalized;

        UpdateRotationText();
    }

    void UpdateDragging()
    {
        Vector2 mousePos = GetMouseUIPosition();
        Vector2 toMouse = mousePos - uiCenter; // Now correctly centered

        if (toMouse.magnitude < stopRadius && toMouse.magnitude != 0) // Stop if mouse gets too close
        {
            Debug.Log("Stopping drag, " + toMouse.magnitude + " < " + stopRadius);
            isDragging = false;
            UpdateRotationText();
            return;
        }

        Vector2 currentDir = toMouse.normalized;
        float angleDelta = Vector2.SignedAngle(lastMouseDirection, currentDir);
        thisDragAngle -= angleDelta; //clockwise = positive
        lastMouseDirection = currentDir;

        UpdateRotationText();
    }

    Vector2 GetMouseUIPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePos, null, out anchoredPos);
        
        // Adjust position to ensure (0,0) is center of the UI
        return anchoredPos;
    }

    void UpdateUICenter()
    {
        // The center of the canvas in local UI coordinates is always (0,0)
        uiCenter = Vector2.zero;
    }

    void UpdateStopRadiusCircle()
    {
        if (stopRadiusCircle == null) return;

        stopRadiusCircle.anchoredPosition = uiCenter; // Set to UI center

        // Adjust the size to match stopRadius
        float adjustedRadius = stopRadius / canvasRect.lossyScale.x;
        stopRadiusCircle.sizeDelta = new Vector2(adjustedRadius*1.2f, adjustedRadius*1.2f);
    }

    void UpdateRotationText()
    {
        if (rotationText == null) return;

        rotationText.text = $"Drag angle: {thisDragAngle/360f:F2} revs\nTotal angle: {(storedAngle+thisDragAngle)/360f:F2} revs";
    }
}
