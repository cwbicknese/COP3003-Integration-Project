using UnityEngine;
using UnityEngine.UI; //this is needed to use Unity's Text class

//this class writes the amount of damage dealt from an attack onto the screen
public class DamageNumbers : MonoBehaviour
{
    public Text dmgText; //defines the Text component in Unity
    public float dmgAmount; //amount of damage that will be written
    public int dmgCounter; //counter that determines the duration the text is displayed

    // Update is called once per frame
    void Update()
    {
        dmgText.text = "Dmg: " + dmgAmount;

        // timer for how long damage is shown
        if (dmgCounter > 0)
        {
            dmgCounter--;
        }
        else
        {
            dmgAmount = 0;
        }
    }
}
