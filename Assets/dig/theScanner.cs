using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class theScanner : MonoBehaviour
{
    public TextMeshPro statusText; //Replace with images later
    public AudioSource winSFX, missSFX;
    public Vector2 hiddenPoint;
    private List<float> scanRadiuses = new List<float> { 8f, 4f, 1.6f, 0.6f, 0f};
    private bool timedOut, gameEnded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)){
            audioSource.pitch = Time.timeScale;
        }
        //Cursor.visible = false;
        hiddenPoint = new Vector2(Random.Range(-8f, 8f), Random.Range(-3.8f, 3.8f));
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
        updateScreen();
        if(Input.GetMouseButtonDown(0)){
            if(statusText.text == "!"){
                //win
                statusText.text = "W";
                statusText.color = Color.green;
                winSFX.Play();
                gameEnded = true;
                dispatch.failed = false;
                StartCoroutine(sendBackAsync(1.2f));
            }
            else{
                statusText.text = "X";
                timedOut = true;
                Invoke(nameof(untimeOut), 0.5f);
                missSFX.Play();
            }
        }
    }
    void untimeOut(){timedOut = false;}
    void updateScreen(){
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
    private IEnumerator sendBackAsync(float delay){ 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
}
