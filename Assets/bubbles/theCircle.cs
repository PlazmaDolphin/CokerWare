using TMPro;
using UnityEngine;

public class theCircle : MonoBehaviour
{
    //private bool clicked = false;
    public SpriteRenderer SpriteRenderer;
    public TextMeshPro numText;
    public AudioSource glassBreak, pop, clear;
    private int number;
    private bool clicked = false;
    private static int maxnum = 1;
    private static int progress = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progress = 1; //Reset on each scene load
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setData(float radius, int number) {
        this.transform.localScale = new Vector3(radius, radius, 1f);
        numText.text = number.ToString();
        this.number = number;
        maxnum = Mathf.Max(maxnum, number);
    }

    private void OnMouseDown() {
        if (clicked||barTimer.halt||barTimer.expired) return;
        if (number == progress) {
            progress++;
            clicked = true;
            SpriteRenderer.color = Color.green;
            if (progress > maxnum) {
                SpawnScript.win();
                clear.Play();
            }
            else pop.Play();
        }
        else {
            SpriteRenderer.color = Color.red;
            glassBreak.Play();
            SpawnScript.lose();
        }
    }
}
//You know what kind of people I hate the most? People who don't know how to use the fucking shift key. I mean, seriously, it's not that hard. It's right there. It's not like it's some obscure key that you have to go looking for. It's right there, next to the z. And yet, there are people who just refuse to use it. I mean, what the hell is up with that? It's like they're saying, "Oh, I'm sorry, I'm too lazy to use the shift key, so I'm just going to type everything in lowercase, even though it makes me look like a total moron." And you know what? They do look like total morons. I mean, seriously, how hard is it to use the shift key? It's not hard at all. You just hold it down while you press another key. That's it. It's not rocket science. And yet, there are people who just can't seem to grasp the concept. I mean, what the hell is wrong with them? Are they just too stupid to figure it out? Are they too lazy to bother? I don't know, but it really pisses me off. I mean, it's not like it's a difficult thing to do. It's not like it takes a lot of effort. It's not like it's some kind of major inconvenience. It's just a simple key on the keyboard.
