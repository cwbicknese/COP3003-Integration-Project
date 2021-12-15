using UnityEngine;

// This class will be for any character that has stats.
// The player and enemies will be subclasses to this so that they can have hp and can take damage
// This class has a virtual function which is overridden in the subclasses PlayerMovement, Enemy1, Enemy2, and EnemyParent
// referenced: https://www.youtube.com/watch?v=e8GmfoaOB4Y&t=245s

public class CharacterStats : MonoBehaviour
{
    //HP
    public float hpMax;
    public float hp;

    //MP
    public float mpMax = 100f;
    public float mp;

    //Atk and Def
    public Stat attack;
    public Stat defense;

    //gold
    public float gold = 0;

    //This is an example of a constructor for demonstration purposes. Instead of creating objects using constructors,
    //I will use Unity's instantiate method so that I can place them into the game world. They do the same thing,
    //but instantiate seems to be preferred for creating objects in Unity because it allows the object to have all of its components like rigidbody and its collision mask
    public CharacterStats() // default constructor sets hp to 100 if no parameters. Then sets hp to hpMax.
    {
        hpMax = 100f;
        hp = hpMax;
    }
    public CharacterStats(float maxHealth) // overloaded using 1 parameter, maxHealth, and sets hpMax to it. Then sets hp to hpMax.
    {
        hpMax = maxHealth;
        hp = hpMax;
    }
    //To create objects using the constructor:
    //  CharacterStats player1 = new CharacterStats();      //creates player1 as an object of CharacterStats class, hpMax and hp are set to 100 by default constructor
    //  player1.mpMax = 100f;                               //sets player1's mpMax to 100
    //  player1.AddComponent<Rigidbody>();                  //adds the Rigidbody component to player1
    //
    //  CharacterStats player2 = new CharacterStats(300f);  //creates player2 as an object of CharacterStats class, hpMax and hp are set to 300 by overloaded constructor
    //  player2.mpMax = 50f;                                //sets player2's mpMax to 50
    
    void Awake()
    {
        hp = hpMax;
        mp = mpMax;
    }

    public void takeDamage(float damage)
    {
        damage -= defense.getValue(); //damage is reduced by the value of the defense stat
        damage = Mathf.Clamp(damage, 0, float.MaxValue); //prevents damage from being negative

        hp -= damage;

        //damage number shown
        GameObject.Find("dmg_text").GetComponent<DamageNumbers>().dmgAmount = damage;
        GameObject.Find("dmg_text").GetComponent<DamageNumbers>().dmgCounter = 120;

        if (hp <= 0)
        {
            die(); // In CharacterStats, die() is virtual, so it will go down to the most derived form of the function
                   // to override it, which will be in Enemy1 or Enemy2.
        }
    }

    // Polymorphism: when die() is called, because it is virtual, it will look for the most derived class that has the function.
    protected virtual void die()
    {
        if (gameObject != null) 
        {
            Debug.Log("called CharacterStats die()");
            Destroy(gameObject);
        }
    }
}
