using System.Linq;
using TMPro;
using UnityEngine;

public class barTimer : MonoBehaviour
{
    public TextMeshPro display;
    public int ticks = 8; //could vary per game
    private static float ticklen = 1f; //varies throughout session
    public static bool expired = false;
    public static bool halt = false; //controlled externally
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        display.text = string.Concat(Enumerable.Repeat("--", ticks));
        expired = false;
        halt = false;
        Invoke("_tick", ticklen);
    }

    private void _tick() {
        if (ticks >= 1 && !halt) {
            ticks--;
            display.text = string.Concat(Enumerable.Repeat("--", ticks));
            Invoke("_tick", ticklen);
        }
        else if (ticks <= 0){
            display.text = "";
            expired = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
