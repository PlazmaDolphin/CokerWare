using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dispatch : MonoBehaviour {
    public static bool firstGame = true;
    public static bool failed; //Games MUST update this after winning/losing
    public TextMeshPro BBText;
    public TextMeshProUGUI failText;
    public lifeCount lives;
    public CameraFollowAndZoom cam;
    public AudioSource winSFX, loseSFX, nextSFX, speedUpSFX, levelUpSFX;
    public GameObject deadUI, scoreSign;
    public gameSpeed theSpeed;
    public int nextIndex;
    private bool speedingUp = false, levelingUp = false;
    private const int IMPLEMENTEDGAMES = 4;
    private int TESTINGGAME = 0;
    private const float IGNOREINPUTTIME = 1f;
    private bool ignoreInput = true;
    private List<string> prompts = new() {
        "YOU CANT SEE THIS",
        "Click in order!",
        "Screw!",
        "Detect Treasure!",
        "Defuse!"
    };
    private void listen() {
        ignoreInput = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        resetAll();
        Invoke(nameof(listen), IGNOREINPUTTIME);
        if (failed) {
            BBText.text = "You Suck!";
            BBText.color = Color.red;
            lives.loseLife();
            if (!lives.isAlive()) {
                gameOver();
                return;
            }
        }
        else if (!firstGame) {
            BBText.text = "Good Boy!";
            BBText.color = Color.green;
            scoreCount.score += 1;
            //Check for speed up
            int anyChange = theSpeed.shouldSpeed(scoreCount.score);
            if (anyChange == -1) {
                Invoke(nameof(SP), 4f);
                speedingUp = true;
                BBText.text = "Speed Up!";
                BBText.color = Color.blue;
            }
            else if (anyChange == 1) {
                SP();
                levelingUp = true;
                BBText.text = "Level Up!";
                BBText.color = Color.yellow;
            }
        }
        firstGame = false;
        doTimeline();
    }
    private void ZO() { cam.ZoomOut(); }
    private void ZI() { cam.ZoomIn(); }
    private void SP() { theSpeed.speedUp(); }
    private void TILT() { cam.StartSlowTilt(); }
    private void doTimeline() {
        if (speedingUp) {
            speedUpSFX.Play();
            transform.position += new Vector3(10, 0, 0);
            Invoke(nameof(ZO), 0f);
            Invoke(nameof(clearScreen), 4.0f);
            Invoke(nameof(pickNext), 5.1f);
            Invoke(nameof(ZI), 6f);
            Invoke(nameof(loadNext), 6.5f);
            return;
        }
        if (levelingUp) {
            levelUpSFX.Play();
            transform.position += new Vector3(10, 0, 0);
            Invoke(nameof(ZO), 0f);
            //Invoke(nameof(TILT), 1f);
            Invoke(nameof(clearScreen), 4.0f);
            Invoke(nameof(pickNext), 5.1f);
            Invoke(nameof(ZI), 6f);
            Invoke(nameof(loadNext), 6.5f);
            return;
        }
        if (failed) loseSFX.Play();
        else winSFX.Play();
        Invoke(nameof(ZO), 0f);
        Invoke(nameof(clearScreen), 1.0f);
        Invoke(nameof(pickNext), 2.1f);
        Invoke(nameof(ZI), 3f);
        Invoke(nameof(loadNext), 3.5f);
    }
    private void clearScreen() {
        BBText.text = string.Empty; BBText.color = Color.white;
    }
    public void pickNext() {
        nextIndex = (TESTINGGAME == 0) ? UnityEngine.Random.Range(1, IMPLEMENTEDGAMES + 1) : TESTINGGAME;
        BBText.text = prompts[nextIndex];
        nextSFX.Play();
    }

    public void loadNext() {
        BBText.text = "";
        StartCoroutine(sendBackAsync());
    }
    private IEnumerator sendBackAsync() {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIndex);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(0.7f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
    public void resetAll() {
        if (ignoreInput) return;
        scoreCount.score = 0;
        lives.resetLives();
        failed = false;
        firstGame = true;
        deadUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
    private void gameOver() {
        theSpeed.resetSpeed();
        scoreSign.SetActive(false);
        deadUI.SetActive(true);
        failText.text = "You made it:\n" + (scoreCount.score > 0 ? (scoreCount.score + " mi") : "NOWHERE");
    }
    public void quitLikeABitch() {
        if (ignoreInput) return;
        SceneManager.LoadScene(IMPLEMENTEDGAMES + 1);
    }
}
