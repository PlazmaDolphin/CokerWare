using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using System.Threading.Tasks;
// TODO: Add difficulty modes (0: few nots, 1: many nots, 2: medium nots with game state question ex. cut if this is mile 13)
// TODO: make text appear character by character quickly
public class defuseIt : MonoBehaviour {
    public TextMeshPro cmdText;
    public hoverGlow redWire, whiteWire, blueWire;
    public SentenceGenerator confusion;
    public VideoPlayer boomVid;
    public numTimer segTimer;
    public const int RED = 0, WHITE = 1, BLUE = 2;
    private int wireInQuestion, cutWireA = -1, cutWireB = -1, stage = 1;
    private bool cutSaidWire = false, gameOver = false, canInput = false;
    private string prompt;
    void Start() {
        genPrompt();
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) {
            audioSource.pitch = Time.timeScale;
        }
        if (Application.platform == RuntimePlatform.WebGLPlayer) {
            boomVid.source = VideoSource.Url;
            boomVid.url = Application.absoluteURL + "output_baseline.mp4";
        }
    }
    async Task Update() {
        if (gameOver) return;
        if (segTimer.totalTime <= 0) {
            cmdText.text = "BOOM!";
            ExplodeAndDie();
        }
        //Check for cut wires
        if (redWire.cut) {
            if (!canInput){redWire.cut = false; return;}
            redWire.cutSelf();
            redWire.cut = false;
            if (cutWireA == -1)
                cutWireA = RED;
            else cutWireB = RED;
        }
        else if (whiteWire.cut) {
            if (!canInput){whiteWire.cut = false; return;}
            whiteWire.cutSelf();
            whiteWire.cut = false;
            if (cutWireA == -1)
                cutWireA = WHITE;
            else cutWireB = WHITE;
        }
        else if (blueWire.cut) {
            if (!canInput){blueWire.cut = false; return;}
            blueWire.cutSelf();
            blueWire.cut = false;
            if (cutWireA == -1)
                cutWireA = BLUE;
            else cutWireB = BLUE;
        }
        else return;
        //Check for correct wire
        if (cutSaidWire ^ (stage == 1 ? cutWireA : cutWireB) != wireInQuestion) {
            cmdText.text = stage == 1 ? "NICE!" : "DEFUSED";
            // if both wires cut, win
            if (stage == 1) {
                await revealText(reverse: true);
                genPrompt();
            } //Wait 2 seconds before generating a new prompt
            else { winTheGame(); } //bomb defused, win
            stage = 2;
        }
        else {
            cmdText.text = "WRONG!";
            // violently explode
            ExplodeAndDie();
            stage = 2;
        }
    }
    async Task genPrompt() {
        do {
            wireInQuestion = Random.Range(0, 3);
        } while (wireInQuestion == cutWireA); //Ensure the wire in question is not the one that was cut
        string wire = wireInQuestion == RED ? "red" : wireInQuestion == WHITE ? "white" : "blue";
        int negs = gameSpeed.currentDifficulty == 0 ? Random.Range(2, 4):
                   gameSpeed.currentDifficulty == 1 ? Random.Range(3, 7):
                                                      Random.Range(1, 5);
        cutSaidWire = negs % 2 == 0; //Even number of negations -> double negative, cut
        prompt = confusion.ConstructCommand(negs, wire);
        await revealText();
    }
    async Task revealText(bool reverse = false) {
        canInput = false;
        for (int i = 0; i < prompt.Length; i++) {
            cmdText.text = reverse ? prompt.Substring(0, prompt.Length - i) : prompt.Substring(0, i + 1);
            await Task.Delay((int)(1f/Time.timeScale * (reverse ? 10 : 25)));
        }
        canInput = true;
    }
    void ExplodeAndDie() {
        gameOver = true;
        boomVid.enabled = true;
        dispatch.failed = true;

        boomVid.loopPointReached += OnExplosionVideoFinished;
        boomVid.Play();

        loader = SceneManager.LoadSceneAsync(0);
        loader.allowSceneActivation = false;
    }

    AsyncOperation loader;

    void OnExplosionVideoFinished(VideoPlayer vp) {
        loader.allowSceneActivation = true;
    }
    void winTheGame() {
        gameOver = true;
        Debug.Log("Bomb defused!");
        dispatch.failed = false;
        StartCoroutine(sendBackAsync(1.2f));
    }
    private IEnumerator sendBackAsync(float delay) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
}