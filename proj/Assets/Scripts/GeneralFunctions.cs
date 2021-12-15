using UnityEngine;

// this class consists of different functions that act as the attacks that a player or enemy can do
// this class uses the bool able to determine whether the object is able to perform actions
// the bool able functionality will be in the same class as the attacks because the attacks will always utilize the bool able
// this script will be attached as a component to the player and enemies who can perform attacks

public class GeneralFunctions : MonoBehaviour
{
    //variable initialization for "able" functionality
    public bool able = true;
    public int ableCounter = 0;

    //declaration of game objects
    public GameObject projectile;
    public GameObject ice;
    public Transform shotPoint;

    void Update()
    {
        //when able is false, set it back to true after the delay
        if (!able)
        {
            if (ableCounter > 0) //checks delay before changing able back to true
            {
                ableCounter--;
            }
            else //player is able to move again, able set back to true
            {
                able = true;
                ableCounter = 0;
            }
        }
    }

    //this function sets able to false for an amount of time determined by the argument given.
    //this will mainly be used to create endlag to certain actions.
    public void setAbleFalse(int delay)
    {
        ableCounter = delay;
        able = false;
    }

    //below are the different attacks that can be performed

    //fireball
    //this function shoots an object forward
    public void shootProjectile(float projectileDamage, float force, int endlag, bool fromEnemy)
    {
        // to create a projectile and apply velocity to it, referenced https://www.youtube.com/watch?v=RnEO3MRPr5Y
        if (gameObject.GetComponent<GeneralFunctions>().able)
        {
            setAbleFalse(endlag);  //toggles able off for a duration according to the endlag parameter
            GameObject createdProjectile = Instantiate(projectile, shotPoint.position, shotPoint.rotation); //creates the projectile
            createdProjectile.GetComponent<Fireball>().dmg = projectileDamage; //sets damage
            createdProjectile.GetComponent<Fireball>().belongsToEnemy = fromEnemy; //uses the boolean parameter fromEnemy, should be false if called from PlayerMovement, true from the enemy classes
            createdProjectile.GetComponent<Rigidbody>().velocity = shotPoint.transform.forward * force; //applies force to make it move (the object must have a rigidbody for this to work)
        }
    }

    //ice
    //this function surrounds the player in ice by instantiating the ice object
    public void castIce(float iceDmg, int endlag)
    {
        if (gameObject.GetComponent<GeneralFunctions>().able)
        {
            gameObject.GetComponent<GeneralFunctions>().setAbleFalse(endlag); //toggles able off
            GameObject createdIce = Instantiate(ice, shotPoint.position, shotPoint.rotation); //creates ice object
            createdIce.GetComponent<IceSpell>().dmg = iceDmg; //sets damage
        }
    }
}
