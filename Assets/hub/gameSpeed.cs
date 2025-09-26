using System.Collections.Generic;
using UnityEngine;

public class gameSpeed : MonoBehaviour
{
    //List of speeds
    public Transform needlePivot, tacPivot;
    // Table of when Speed and Difficulty changes at Level
    private static List<(float S, int D, int L)> speedsEasy = new() {
        (1.0f, 0, 0), (1.3f, 0, 2), (1.65f, 0, 4), (2f, 0, 6),
        (1f, 1, 8), (1.4f, 1, 11), (1.7f, 1, 15), (2f, 1, 18),
        (1f, 2, 20), (1.25f, 2, 24), (1.5f, 2, 28), (1.75f, 2, 32), (2f, 2, 36), (2.2f, 2, 40), (2.4f, 2, 45), (2.6f, 2, 50), (2.9f, 2, 60), (3.3f, 2, 70), (3.7f, 2, 80), (4.0f, 2, 90), (5f, 2, 100)
        //(1.0f, 0, 0), (1f, 1, 1), (2f, 1, 2), (1f, 2, 3), (2f, 2, 4)
    }; //aint no one getting past level 60
    public static bool hardMode = false;
    private static List<(float S, int D, int L)> speedsHard = new() {
        (1f, 2, 0), (1.2f, 2, 3), (1.4f, 2, 6), (1.6f, 2, 9), (1.8f, 2, 12), (2f, 2, 15), (2.2f, 2, 18), (2.4f, 2, 21), (2.6f, 2, 24), (2.8f, 2, 27), (3f, 2, 30), (3.3f, 2, 35), (3.6f, 2, 40), (3.9f, 2, 45), (4.2f, 2, 50), (4.6f, 2, 60), (5f, 2, 70), (5.5f, 2, 80), (6f, 2, 90), (7f, 2, 100)  
    };
    private static int currentSpeedIndex = 0;
    public static float currentSpeed { get { return hardMode ? speedsHard[currentSpeedIndex].S : speedsEasy[currentSpeedIndex].S; } }
    public static int currentDifficulty { get { return hardMode ? speedsHard[currentSpeedIndex].D : speedsEasy[currentSpeedIndex].D; }}

    public List<(float S, int D, int L)> speeds { get { return hardMode ? speedsHard : speedsEasy; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSpeed();
    }

    // Update is called once per frame
    void UpdateSpeed()
    {
        //rotate needle
        needlePivot.rotation = Quaternion.Euler(0, 0, -255 * 50 * speeds[currentSpeedIndex].S / 160 + 30);
        tacPivot.rotation = Quaternion.Euler(0, 0, -45 - 170f*speeds[currentSpeedIndex].D/speeds[speeds.Count-1].D);
        Time.timeScale = speeds[currentSpeedIndex].S;
        //scale audio pitch
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            audioSource.pitch = speeds[currentSpeedIndex].S;
        }
    }
    //returns -1 if speeding up, 0 if no change, and 1 if difficulty changes
    public int shouldSpeed(int lvl)
    {
        if (!(currentSpeedIndex + 1 == speeds.Count)
                && (lvl >= speeds[currentSpeedIndex + 1].L))
        {
            return speeds[currentSpeedIndex].D != speeds[currentSpeedIndex + 1].D ? 1 : -1;
        }
        return 0;
    }
    public void speedUp()
    {
        if (currentSpeedIndex < speeds.Count - 1)
        {
            currentSpeedIndex++;
            UpdateSpeed();
        }
    }
    public void resetSpeed()
    {
        currentSpeedIndex = 0;
        UpdateSpeed();
    }
}