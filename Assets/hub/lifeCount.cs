using UnityEngine;

public class lifeCount : MonoBehaviour
{
    private static int lives=4;
    public SpriteRenderer life1;
    public SpriteRenderer life2;
    public SpriteRenderer life3;
    public SpriteRenderer life4;
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
        life1.enabled = (lives >= 1);
        life2.enabled = (lives >= 2);
        life3.enabled = (lives >= 3);
        life4.enabled = (lives >= 4);
    }
}
