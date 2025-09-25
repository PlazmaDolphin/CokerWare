using System.Collections.Generic;
using UnityEngine;

public class gameSpeed : MonoBehaviour
{
    //List of speeds
    public Transform needlePivot, tacPivot;
    // Table of when Speed and Difficulty changes at Level
    private static List<(float S, int D, int L)> speeds = new() {
        (1.0f, 0, 0), (1.3f, 0, 3), (1.65f, 0, 6), (2f, 0, 8),
        (1f, 1, 10), (1.4f, 1, 14), (1.8f, 1, 17), (2.2f, 1, 20),
        (1f, 2, 24), (1.5f, 2, 27), (2f, 2, 30), (2.5f, 2, 35), (2.8f, 2, 40), (3.0f, 2, 43), (3.2f, 2, 46), (3.4f, 2, 49), (3.6f, 2, 52), (3.8f, 2, 55), (4.0f, 2, 60)
        //(1.0f, 0, 0), (1f, 1, 1), (2f, 1, 2), (1f, 2, 3), (2f, 2, 4)
    }; //aint no one getting past level 60
    private static int currentSpeedIndex = 0;
    public static float currentSpeed { get { return speeds[currentSpeedIndex].S; } }
    public static int currentDifficulty { get { return speeds[currentSpeedIndex].D; }}
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