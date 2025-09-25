using UnityEngine;
using TMPro;

public class cursorTrack : MonoBehaviour
{
    public RectTransform canvasRect; // Assign the Canvas in the Inspector
    public RectTransform stopRadiusCircle; // UI Image for the stop radius
    public TextMeshProUGUI rotationText; // UI Text to display rotation info
    public float stopRadius = 50f; // Stop drag when this close to center (in UI pixels)

    public AudioSource squeakAudio, tooClose; // Assign an AudioSource with the squeak sound in the Inspector
    public GameObject guideArrows;
    private bool isDragging = false;
    private Vector2 lastMouseDirection;
    public float thisDragAngle, storedAngle;
    private Vector2 uiCenter; // UI-space center of the canvas
    private int lastFullRevolutions = 0; // To track when a full revolution occurs

    void Start() {
        UpdateUICenter();
        UpdateRotationText();
        if (screwDriver.reversed) {
            //vertically flip guideArrows, taking its current scale into account
            guideArrows.transform.localScale = new Vector3(-guideArrows.transform.localScale.x, guideArrows.transform.localScale.y, guideArrows.transform.localScale.z);
        }
        if (gameSpeed.currentDifficulty >= 2)
            stopRadius *= 3.5f;
        UpdateStopRadiusCircle();
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
            lastFullRevolutions = Mathf.FloorToInt((storedAngle + thisDragAngle) / 360f); // Reset revolution tracking
            UpdateRotationText();
        }
    }

    void StartDragging()
    {
        Vector2 mousePos = GetMouseUIPosition();
        Vector2 toMouse = mousePos - uiCenter; // Now correctly centered
        guideArrows.SetActive(false);

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
            isDragging = false;
            UpdateRotationText();
            tooClose.Play();
            return;
        }

        Vector2 currentDir = toMouse.normalized;
        float angleDelta = Vector2.SignedAngle(lastMouseDirection, currentDir);
        thisDragAngle += screwDriver.reversed ? angleDelta : -angleDelta; // Clockwise = positive
        lastMouseDirection = currentDir;

        CheckForFullRevolution();
        UpdateRotationText();
    }

    void CheckForFullRevolution()
    {
        int currentFullRevolutions = Mathf.FloorToInt((storedAngle + thisDragAngle) / (screwDriver.reversed ? 360f: 360f));
        
        if (currentFullRevolutions > lastFullRevolutions) // If a new full revolution is completed
        {
            PlaySqueakSound();
            lastFullRevolutions = currentFullRevolutions; // Update last counted revolution
        }
    }

    void PlaySqueakSound()
    {
        if (squeakAudio != null)
        {
            squeakAudio.Play();
        }
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
        float adjustedRadius = stopRadius ;//* canvasRect.lossyScale.x;
        stopRadiusCircle.sizeDelta = new Vector2(adjustedRadius * 2f, adjustedRadius * 2f);
    }

    void UpdateRotationText()
    {
        if (rotationText == null) return;

        rotationText.text = $"Drag angle: {thisDragAngle / 360f:F2} revs\nTotal angle: {(storedAngle + thisDragAngle) / 360f:F2} revs";
    }
}