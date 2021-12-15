using UnityEngine;
using UnityEngine.UI; //this is needed to use Unity's Text class

public class SplashText : MonoBehaviour
{
    public Text splashText;
    public string txt = ""; //initialize to no text
    private int count = 0; //counter that indicates how long the text will be on the screen

    // Update is called once per frame
    void Update()
    {
        splashText.text = txt; //sets the text component of this class so that it uses txt as its string

        // timer for how long text is shown
        if (count > 0) //checks if the string says something
        {
            count--;
        }
        else //when count reaches 0, set the string to nothing and reset the count
        {
            txt = "";
            count = 0;
        }
    }

    public void setText(string newString, int maxCount = 300)
    {
        txt = newString;
        count = maxCount;
    }
}