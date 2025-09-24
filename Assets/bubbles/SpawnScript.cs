using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnScript : MonoBehaviour
{
    public TextMeshProUGUI theWinTxt;
    public static TextMeshProUGUI winTxt;
    public GameObject circlePrefab;
    private int objects = 8;
    private float radius = 3f;
    private List<int> nums = new();
    private bool expireCodeRan = false;
    private static bool completed = false, sendingBack = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildNumArray();
        //adjust pitch of all audio sources to match game speed
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) {
            audioSource.pitch = Time.timeScale;
        }
        completed = false;
        sendingBack = false;
        for (int i = 0; i < objects; i++) {
            for (int j=0; j<50; j++){ //Attempt up to 50 spawn locations for each object
                Vector3 spawnPoint = new Vector3(Random.Range(-8.8f + radius, 8.8f - radius), Random.Range(-5f + radius, 5f - radius));
                if (!Physics2D.OverlapCircle(spawnPoint, radius)) {
                    GameObject c = Instantiate(circlePrefab, spawnPoint, Quaternion.identity);
                    c.GetComponent<theCircle>().setData(radius*1.4f, nums[i]);
                    radius *= 0.85f; //CHANGE THIS
                    break;
                }
            }
        }
        winTxt = theWinTxt;
    }
    private void buildNumArray() {
        for (int i = 0; i < objects; i++) {
            nums.Add(i + 1);
        }
        if (gameSpeed.currentDifficulty == 1) {
            nums.Reverse();
        }
        if (gameSpeed.currentDifficulty == 2) {
            nums = nums.OrderBy(x => Random.Range(0, int.MaxValue)).ToList();
        }
    }

    private IEnumerator sendBackAsync()
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(0.8f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
    public static void win() {
        winTxt.text = "you winned";
        winTxt.enabled = true;
        barTimer.halt = true;
        dispatch.failed = false;
        completed = true;
    }

    public static void lose() {
        winTxt.text = "you lossed :(";
        winTxt.color = Color.red;
        winTxt.enabled = true;
        barTimer.halt = true;
        dispatch.failed = true;
        completed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (barTimer.expired&&!expireCodeRan&&!completed) { //might be resource intensive idk
            lose();
            winTxt.text = "no time";
            expireCodeRan = true;
            dispatch.failed = true;
            completed = true;
        }
        if (completed&&!sendingBack) { StartCoroutine(sendBackAsync()); sendingBack = true; }
    }
}
