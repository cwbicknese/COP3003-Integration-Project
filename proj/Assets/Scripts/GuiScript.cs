using UnityEngine;
using UnityEngine.UI; //this is needed to use Unity's Text class

//this class writes information on the screen
public class GuiScript : MonoBehaviour
{
    public Text guiText;
    private GameObject playerObj;
    private CharacterStats playerStats;

    private string goldString;
    private string hpString;
    private string mpString;
    private string atkString;
    private string defString;

    //health bar and mp bar displayed in the top left corner
    public Slider healthSlider;
    public Slider mpSlider;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("obj_player");
        playerStats = playerObj.GetComponent<CharacterStats>();
        healthSlider.value = 1; //initialize healthbar at start of game
        mpSlider.value = 1; //initialize mpbar at start of game
    }

    // Update is called once per frame
    void Update()
    {
        //write stats on the screen
        goldString = "Gold: " + playerStats.gold;
        hpString = "HP: " + playerStats.hp + "/" + playerStats.hpMax;
        mpString = "MP: " + playerStats.mp + "/" + playerStats.mpMax;
        atkString = "Atk: " + playerStats.attack.getValue();
        defString = "Def: " + playerStats.defense.getValue();

        guiText.text = hpString + "\n"
            + mpString + "\n"
            + atkString + "\n"
            + defString + "\n"
            + goldString
            ;
        
        healthSlider.value = updateHealth();
        mpSlider.value = updateMp();
    }

    private float updateHealth()
    {
        return playerStats.hp / playerStats.hpMax;
    }
    private float updateMp()
    {
        return playerStats.mp / playerStats.mpMax;
    }

}

// assets downloaded from Unity Asset Store
/* LowlyPoly - Hand Painted Stone Texture
 * LowlyPoly - Fantasy Treasure Pack Lite
 * amusedArt - Stone Monster
 * TeamJoker - Fantasy Monster - Skeleton
 * TS WORK - Fantasy Monster(Wizard) DEMO
 * 
 * 
 * 
 */

