using NUnit.Framework.Constraints;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class screwDriver : MonoBehaviour
{
    private const float SCREWPITCH = 0.023f;
    private const int TURNS = 9;
    private float INITPOSX, FINALPOSX;
    public TextMeshProUGUI text, winned;
    public cursorTrack ct;
    private bool gameEnded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FINALPOSX = transform.position.x;
        INITPOSX = FINALPOSX + TURNS * SCREWPITCH;
        transform.position = new Vector3(INITPOSX, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;
        transform.position = new Vector3(INITPOSX - (ct.storedAngle+ct.thisDragAngle)/360f * SCREWPITCH, transform.position.y, transform.position.z);
        transform.rotation = quaternion.Euler((ct.storedAngle+ct.thisDragAngle)/180f*math.PI, 0, 0);
        //Display number of turns and total progress
        text.text = "Screw IN:  I\nScrew OUT: O\n\nScrew Position:\n"+((INITPOSX - transform.position.x) / SCREWPITCH).ToString("F3")+"Turns\nProgress: "+((INITPOSX - transform.position.x) / (FINALPOSX - INITPOSX) * -100).ToString("F1")+"%";
        if (transform.position.x <= FINALPOSX) {
            winned.text = "you winned";
            winned.enabled = true;
            barTimer.halt = true;
            dispatch.failed = false;
            gameEnded = true;
        }
        //if time expired
        if (barTimer.expired) {
            winned.text = "you lossed :(";
            winned.color = Color.red;
            winned.enabled = true;
            barTimer.halt = true;
            gameEnded = true;
        }
    }
}
