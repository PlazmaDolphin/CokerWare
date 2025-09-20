using UnityEngine;
using UnityEngine.UI;

public class lifeCount : MonoBehaviour
{
    private static int lives=4;
    public Image life1, life2, life3, life4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void loseLife() {
        lives--;
        //Animation stuff later?
    }
    public void gainLife() {
        lives++;
        //Animation stuff later?
    }
    public void resetLives() {
        lives = 4;
    }
    public bool isAlive() {
        return lives > 0;
    }

    // Update is called once per frame
    void Update()
    {
        life1.enabled = lives >= 1;
        life2.enabled = lives >= 2;
        life3.enabled = lives >= 3;
        life4.enabled = lives >= 4;
    }
}
