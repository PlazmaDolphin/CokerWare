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