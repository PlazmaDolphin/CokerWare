using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class defuseIt : MonoBehaviour
{
    public TextMeshPro cmdText;
    public hoverGlow redWire, whiteWire, blueWire;
    public SentenceGenerator confusion;
    public VideoPlayer boomVid;
    public numTimer segTimer;
    public const int RED=0, WHITE=1, BLUE=2;
    private int wireInQuestion, cutWireA = -1, cutWireB = -1, stage=1;
    private bool cutSaidWire = false, gameOver = false;
    void Start(){
        genPrompt();
    }
    void Update()
    {
        if (gameOver) return;
        if (segTimer.totalTime <= 0){
            cmdText.text = "BOOM!";
            ExplodeAndDie();
        }
        //Check for cut wires
        if(redWire.cut){
            redWire.cutSelf();
            redWire.cut = false;
            if (cutWireA == -1)
                cutWireA = RED;
            else cutWireB = RED;
        }
        else if(whiteWire.cut){
            whiteWire.cutSelf();
            whiteWire.cut = false;
            if (cutWireA == -1)
                cutWireA = WHITE;
            else cutWireB = WHITE;
        }
        else if(blueWire.cut){
            blueWire.cutSelf();
            blueWire.cut = false;
            if (cutWireA == -1)
                cutWireA = BLUE;
            else cutWireB = BLUE;
        }
        else return;
        //Check for correct wire
        if (cutSaidWire ^ (stage==1 ? cutWireA : cutWireB) != wireInQuestion){
            cmdText.text = stage==1 ? "NICE!" : "DEFUSED";
            // if both wires cut, win
            if (stage==1) Invoke("genPrompt", 2f); //Wait 2 seconds before generating a new prompt
            else{} //bomb defused, win
            stage = 2;
        }
        else{
            cmdText.text = "WRONG!";
            // violently explode
            ExplodeAndDie();
            stage = 2;
        }
    }
    void genPrompt(){
        do{
            wireInQuestion = Random.Range(0, 3);
        } while (wireInQuestion == cutWireA); //Ensure the wire in question is not the one that was cut
        string wire = wireInQuestion==RED ? "red" : wireInQuestion==WHITE ? "white" : "blue";
        int negs = Random.Range(2, 5);
        cutSaidWire = negs%2==0; //Even number of negations -> double negative, cut
        string prompt = confusion.ConstructCommand(negs, wire);
        cmdText.text = prompt;
    }
    void ExplodeAndDie()
    {
        gameOver = true;
        boomVid.enabled = true;
        dispatch.failed = true;

        boomVid.loopPointReached += OnExplosionVideoFinished;
        boomVid.Play();

        loader = SceneManager.LoadSceneAsync(0);
        loader.allowSceneActivation = false;
    }

    AsyncOperation loader;

    void OnExplosionVideoFinished(VideoPlayer vp)
    {
        loader.allowSceneActivation = true;
    }
    void winTheGame(){
        dispatch.failed = false;
        sendBackAsync(1.2f);
    }
    private IEnumerator sendBackAsync(float delay){ 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(delay);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) { yield return null; }
    }
}