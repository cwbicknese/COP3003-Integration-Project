using UnityEngine;

// This class is for the ice spell the player can cast
// It will make a block of ice on top of the player that can damage enemies and reflect fireballs

public class IceSpell : MonoBehaviour
{
    private int duration = 100; //how long the ice spell will last
    private bool hurt = false; //checks whether it can hurt an enemy, so that it hurts the enemy once instead of every frame
    public float dmg; //damage number that gets passed to this class

    // Update is called once per frame
    void Update()
    {
        //automatic self destruction after a certain amount of time
        duration--;
        if (duration <= 0)
        {
            Destroy(gameObject); //destroys self
        }
    }

    //collision
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision detected");
        //collision with enemy
        if (other.gameObject.CompareTag("enemy"))
        {
            Debug.Log(dmg);
            if (!hurt) //this ensures that it only hits once instead of every frame
            {
                dmg /= 2; //ice is half as strong as the attack value passed to it from PlayerMovement when it is created
                dmg += other.gameObject.GetComponent<CharacterStats>().defense.getValue(); //adds foe's defense so that it pierces defense
                other.gameObject.GetComponent<EnemyParent>().takeDamage(dmg);
                hurt = true;
            }
        }
    }
}
