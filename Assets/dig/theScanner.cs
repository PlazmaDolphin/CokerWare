using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class theScanner : MonoBehaviour
{
    public TextMeshPro statusText; //Replace with images later
    public AudioSource winSFX, missSFX, beepSFX, beepHoldSFX;
    public Vector2 hiddenPoint;
    private List<float> scanRadiuses = new List<float> { 8f, 4f, 1.6f, 0.6f, 0f};
    private bool timedOut, gameEnded;
    private float beepTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) {
            audioSource.pitch = Time.timeScale;
        }
        //Cursor.visible = false;
        hiddenPoint = new Vector2(Random.Range(-8f, 8f), Random.Range(-3.8f, 3.8f));
        if (gameSpeed.currentDifficulty == 2) {
            beepTimer = 0.1f;
            genBeeps();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move to cursor (2D)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        if(gameEnded) return;
        if(barTimer.expired){
            dispatch.failed = true;
            gameEnded = true;
            StartCoroutine(sendBackAsync(0.7f));
        }
        if (timedOut) return;
        if (gameSpeed.currentDifficulty == 0) updateScreenD0();
        else if (gameSpeed.currentDifficulty == 1) updateScreenD1();
        else if (gameSpeed.currentDifficulty == 2) updateScreenD2();
        if (Input.GetMouseButtonDown(0)) {
            if (statusText.text == "!") {
                //win
                statusText.text = "W";
                statusText.color = Color.green;
                winSFX.Play();
                gameEnded = true;
                dispatch.failed = false;
                StartCoroutine(sendBackAsync(1.2f));
            }
            else {
                statusText.text = "X";
                timedOut = true;
                Invoke(nameof(untimeOut), 0.5f);
                missSFX.Play();
            }
        }
    }
    void untimeOut(){timedOut = false;}
    void updateScreenD0() {
        //print an arrow in the direction of the hidden point
        Vector2 dir = hiddenPoint - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        if (angle >= 315 || angle < 45) statusText.text = "→";
        else if (angle >= 45 && angle < 135) statusText.text = "↑";
        else if (angle >= 135 && angle < 225) statusText.text = "←";
        else if (angle >= 225 && angle < 315) statusText.text = "↓";
        if (dir.magnitude <= scanRadiuses[scanRadiuses.Count - 2])
            statusText.text = "!"; //If close enough, show exclamation mark
    }
    void updateScreenD1(){
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), hiddenPoint);
        int i = 0;
        for(; i < scanRadiuses.Count; i++){
            if(dist > scanRadiuses[i] * (1+ (1-gameSpeed.currentSpeed)*0.25)){
                break; //Make it easier at higher speeds
            }
        }
        statusText.text = $"{i}";
        if(i == scanRadiuses.Count - 1){
            statusText.text = "!";
        }
    }
    async void genBeeps() {
        while (!gameEnded) {
            if (timedOut || beepHoldSFX.isPlaying) {
                await System.Threading.Tasks.Task.Delay(10);
                continue;
            }
            beepSFX.Play();
            statusText.text = (statusText.text == ".") ? " " : ".";
            await System.Threading.Tasks.Task.Delay((int)(beepTimer * 1000));
        }
    }
    void updateScreenD2() {
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), hiddenPoint);
        //make frequency depend on distance, play sound quicker when closer
        beepTimer = dist / 48f;
        //play sound every betweenBeeps seconds
        if (dist <= scanRadiuses[scanRadiuses.Count - 2]*2) {
            statusText.text = "!";
            if (!beepHoldSFX.isPlaying) beepHoldSFX.Play();
            beepSFX.Stop();
        }
        else {
            beepHoldSFX.Stop();
        }

    }
    private IEnumerator sendBackAsync(float delay) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
}
