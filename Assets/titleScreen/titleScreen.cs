using UnityEngine;
using UnityEngine.SceneManagement;

public class titleScreen : MonoBehaviour {
    public void startEasy() {
        gameSpeed.hardMode = false;
        SceneManager.LoadScene(0);
    }
    public void startHard() {
        gameSpeed.hardMode = true;
        SceneManager.LoadScene(0);
    }
    public void quitGame() {
        Application.Quit();
    }
}
