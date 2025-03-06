using TMPro;
using UnityEngine;

public class scoreCount : MonoBehaviour
{
    public TextMeshPro text;
    public static int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = score.ToString();
    }
}
