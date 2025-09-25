using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class screwDriver : MonoBehaviour
{
    private const float SCREWPITCH = 0.023f;
    private int TURNS = 9;
    private float INITPOSX, FINALPOSX;
    public TextMeshProUGUI text, winned;
    public cursorTrack ct;
    private bool gameEnded = false;
    public static bool reversed;
    public AudioSource win;
    public GameObject deadCircle;
    public Animator winAnim;
    public Transform otherScrew;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) {
            audioSource.pitch = Time.timeScale;
        }
        if (gameSpeed.currentDifficulty >= 2) TURNS -= 2; //cant make it too hard right
        FINALPOSX = transform.position.x;
        INITPOSX = FINALPOSX + TURNS * SCREWPITCH;
        if (gameSpeed.currentDifficulty >= 1 && UnityEngine.Random.Range(0, 2) == 1) {
            float temp;
            temp = FINALPOSX;
            FINALPOSX = INITPOSX;
            INITPOSX = temp;
            reversed = true;
        }
        else reversed = false;
        transform.position = new Vector3(INITPOSX, transform.position.y, transform.position.z);
        otherScrew.position = new Vector3(FINALPOSX, otherScrew.position.y, otherScrew.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;
        float pos = (ct.storedAngle + ct.thisDragAngle) / 360f * SCREWPITCH;
        float ang = (ct.storedAngle + ct.thisDragAngle) / 180f * math.PI;
        transform.position = new Vector3(INITPOSX+ (reversed ? pos : -pos), transform.position.y, transform.position.z);
        transform.rotation = quaternion.Euler(ang, 0, 0);
        //Display number of turns and total progress
        text.text = "Screw IN:  I\nScrew OUT: O\n\nScrew Position:\n"+((INITPOSX - transform.position.x) / SCREWPITCH).ToString("F3")+"Turns\nProgress: "+((INITPOSX - transform.position.x) / (FINALPOSX - INITPOSX) * -100).ToString("F1")+"%";
        if (transform.position.x <= FINALPOSX && !reversed ||
            transform.position.x >= FINALPOSX && reversed) {
            winned.text = "you winned";
            winned.enabled = true;
            barTimer.halt = true;
            dispatch.failed = false;
            gameEnded = true;
            win.Play();
            winAnim.enabled = true;
            deadCircle.SetActive(false);
            StartCoroutine(sendBackAsync(1.5f));
        }
        //if time expired
        if (barTimer.expired) {
            winned.text = "you lossed :(";
            winned.color = Color.red;
            winned.enabled = true;
            barTimer.halt = true;
            dispatch.failed = true;
            gameEnded = true;
            StartCoroutine(sendBackAsync(0.7f));
        }
    }

    private IEnumerator sendBackAsync(float delay){ 
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
}
