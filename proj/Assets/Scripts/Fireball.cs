using UnityEngine;

// This class is for fireballs, both from the player and from Enemy2
// It uses the bool belongsToEnemy so that it applies damage only to the player or to enemies depending on what class it came from

public class Fireball : MonoBehaviour
{
    private int duration = 600; // this is private so that the variable is only accessible within this class
    public float dmg;           // this is public so that other classes may access this variable
    public bool belongsToEnemy;

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
        //collision with environment/wall
        if (other.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject); //destroys self
        }
    
        if (belongsToEnemy) //if the projectile was from an enemy
        {
            //collision with player
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerMovement>().takeDamage(dmg); // calls takeDamage() from PlayerMovement. PlayerMovement has no function takeDamage(), so it finds it in its superclass CharacterStats.                                                             
                Destroy(gameObject); //destroys self
            }
            //ice reflects it
            if (other.gameObject.CompareTag("ice"))
            {
                gameObject.GetComponent<Rigidbody>().velocity *= -1; //inverts the velocity so that it shoots back at the enemy
                belongsToEnemy = false; //sets belongsToEnemy to false so that it can hurt the enemy
                duration = 600; //resets the duration so that it doesn't disappear too quickly
            }
        }
        else //the projectile was not from an enemy, from the player
        {
            //collision with enemy
            if (other.gameObject.CompareTag("enemy"))
            {
                if (gameObject != null) //prevents error of trying to access object that has already been destroyed
                {
                    other.gameObject.GetComponent<EnemyParent>().takeDamage(dmg); // calls takeDamage() from EnemyParent. EnemyParent has no function takeDamage(), so it finds it in its superclass CharacterStats.                                                             
                    Destroy(gameObject); //destroys self
                }
            }
        }
    }
}
