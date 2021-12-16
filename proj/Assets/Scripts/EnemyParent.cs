using UnityEngine;
using UnityEngine.UI;

// EnemyParent is a sublclass of CharacterStats
// EnemyParent will inherit members from CharacterStats, but CharacterStats will not have members unique to EnemyParent

// EnemyParent is the superclass to Enemy1 and Enemy2
// This class contains anything that all enemies should have or do
// The subclasses Enemy1 and Enemy2 will inherit from EnemyParent, which inherits from CharacterStats, therefore Enemy1 and Enemy2 will also inherit from CharacterStats

// Visibility Inheritence Model:
// Private members can only be accessed by their own class
// Protected members can only be accessed by classes that have an inheritence relation with the class
// Public members can be accessed by any class
// Example: EnemyParent can access public and protected members of CharacterStats because it is a subclass of it, but not private members in CharacterStats
// The Fireball class has no relation to CharacterStats, so it cannot access private or protected members from CharacterStats

public class EnemyParent : CharacterStats // is a subclass of CharacterStats
{
    public int dropRate; // this will be a number that acts as a percentage.
                         // that percentage determines the chance that the enemy drops an item

    public GameObject healthBar;
    public Slider healthSlider;

    private void Start()
    {
        healthSlider.value = 1;
    }
    private void Update()
    {
        healthSlider.value = updateHealth();
    }

    private float updateHealth() //returns a value between 0 and 1 that acts as the percentage of health
    {
        //display healthbar only after the enemy has taken damage
        if (hp < hpMax)
        {
            healthBar.SetActive(true);
        }
        else
        {
            healthBar.SetActive(false);
        }

        //return a decimal value between 1 and 0 to give the health slider a percentage to fill
        if (hp > 0)
        {
            return hp / hpMax;
        }
        else
        {
            return 0; //prevent hp from being negative
        }
    }

    protected override void die()
    {
        if (gameObject != null) //prevents error of trying to access object that has already been destroyed
        {
            Debug.Log("called EnemyParent die()");
            Destroy(gameObject);
        }
    }

}
