using UnityEngine;

public class Enemy1 : EnemyParent
{
    public GameObject drop;

    public Transform player;
    public float minDistance = 5f;
    public float maxDistance = 30f;
    public float moveSpeed = 6f; //speed at which the enemy will move when the player is within range of maxDistance
    public float attackSpeed = 6f; //speed that will be added when the player is within range of minDistance
    
    private void FixedUpdate()
    {
        //follow player when in range
        if (Vector3.Distance(transform.position,player.position) <= maxDistance)
        {
            transform.position += transform.forward * (moveSpeed+attackSpeed) * Time.deltaTime;

            if (Vector3.Distance(transform.position,player.position) <= minDistance)
            {
                attackSpeed = 6f;
            }
            else
            {
                transform.LookAt(player); //looks toward player when within range of maxDistance but not minDistance
                attackSpeed = 0f;
            }
        }
    }

    //collide with player and make the player take damage
    void OnTriggerEnter(Collider other)
    {
        //collision with player
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().takeDamage(attack.getValue()); // calls takeDamage() from PlayerMovement. PlayerMovement has no function takeDamage(), so it finds it in its superclass CharacterStats.                                                             
        }
    }

    protected override void die() 
    {
        dropRate = 25;
        if (Random.Range(0, 101) <= dropRate) // Random.Range function using integers excludes its max value, so this produces a random number from 0 to 100 and checks if its <= 25.
        {
            Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
        Debug.Log("called Enemy1 die()");
        Destroy(gameObject);
    }

    // Enemy1 is a subclass of EnemyParent, which is a subclass of CharacterStats.
    // Subclasses inherit all members their superclasses possess, and can add more.
    // The die() function above overrides the protected virtual die() function in CharacterStats because Enemy1 is an EnemyParent class which is a CharacterStats class. 
    // When die() is called in CharacterStats, it will use this variant of die() in Enemy1 because it is the most derived.
}
