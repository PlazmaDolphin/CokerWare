using UnityEngine;

public class defuseIt : MonoBehaviour
{
    public hoverGlow redWire, whiteWire, blueWire;
    public const int RED=0, WHITE=1, BLUE=2;
    void Update()
    {
        //Check for cut wires
        if(redWire.cut){
            redWire.cutSelf();
        }
        if(whiteWire.cut){
            whiteWire.cutSelf();
        }
        if(blueWire.cut){
            blueWire.cutSelf();
        }
    }
}
