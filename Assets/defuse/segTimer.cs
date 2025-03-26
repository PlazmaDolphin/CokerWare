using TMPro;
using UnityEngine;

public class numTimer : MonoBehaviour
{
    public TextMeshPro time;
    public float totalTime = 12f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(tickClock), 0.1f, 0.1f);
    }
    void tickClock(){
        totalTime -= 0.099999f;
        time.text = totalTime.ToString("F1");
        if(totalTime < 0){
            time.text = "Boom";
            CancelInvoke(nameof(tickClock));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
