using UnityEngine;

public class hoverGlow : MonoBehaviour
{
    public MonoBehaviour glowScript;
    public bool cut = false;
    public MeshRenderer thisWire, cutVersion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        glowScript.enabled = false;
    }

    // Update is called once per frame
    void OnMouseEnter(){
        glowScript.enabled = true;
    }
    void OnMouseDown(){
        cut = true;
    }
    public void cutSelf(){
        thisWire.enabled = false;
        cutVersion.enabled = true;
    }
    void OnMouseExit(){
        glowScript.enabled = false;
    }
}
