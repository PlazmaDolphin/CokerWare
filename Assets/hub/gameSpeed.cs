using System.Collections.Generic;
using UnityEngine;

public class gameSpeed : MonoBehaviour
{
    //List of speeds
    public Transform needlePivot;
    private static List<float> speeds = new List<float> {1.0f, 1.2f, 1.5f, 1.75f, 2.0f, 2.3f};
    public static List<int> speedLvls = new List<int> {3, 6, 9, 12, 15};
    public static int currentSpeedIndex = 0;
    public static float currentSpeed { get { return speeds[currentSpeedIndex]; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSpeed();
    }

    // Update is called once per frame
    void UpdateSpeed()
    {
        //rotate needle
        needlePivot.rotation = Quaternion.Euler(0, 0, -255*50*speeds[currentSpeedIndex]/160 +30);
        Time.timeScale = speeds[currentSpeedIndex];
        //scale audio pitch
        foreach (AudioSource audioSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
        {
            audioSource.pitch = speeds[currentSpeedIndex];
        }
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