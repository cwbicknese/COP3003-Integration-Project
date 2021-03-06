using UnityEngine;

// This is for the potions that the player can pick up to increase their Attack stat
// Colliding with one will increase attack by 1 and display a message on the screen saying "Attack Increased!"

public class StatBooster : MonoBehaviour
{
    private GameObject objSplash;
    private SplashText textToWrite;

    private void Start()
    {
        objSplash = GameObject.Find("obj_splash_text_txt");
        textToWrite = objSplash.GetComponent<SplashText>();
    }

    //collision
    void OnTriggerEnter(Collider other)
    {
        //collision with environment/wall
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterStats>().attack
                .incValue(); // no argument is given so it uses the default parameter in incValue() found in the Stat class

            textToWrite.GetComponent<SplashText>()
                .setText("Attack Increased!"); //sets SplashText txt to the string. No argument is passed for maxCount parameter, so it uses the default parameter

            Destroy(gameObject); //destroys self
        }
    }
}
