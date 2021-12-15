using UnityEngine;

public class Enemy2 : EnemyParent
{
    public GameObject drop;

    public Transform player;
    public float maxDistance = 50f;

    private void FixedUpdate()
    {
        //looks at player when in range and shoots fireballs
        if (Vector3.Distance(transform.position, player.position) <= maxDistance)
        {
            transform.LookAt(player); //looks toward player when within range of maxDistance

            if (gameObject.GetComponent<GeneralFunctions>().able) //if able, shoot a fireball
            {
                gameObject.GetComponent<GeneralFunctions>()
                    .shootProjectile(attack.getValue(), 15f, 400, true); 
            }
        }
    }

    protected override void die()
    {
        if (gameObject != null) //prevents error of trying to access object that has already been destroyed
        {
            dropRate = 100;
            if (Random.Range(0, 101) <= dropRate) // Random.Range function using integers excludes its max value, so this produces a random number
                                                  // from 0 to 100 and checks if its <= 25.
            {
                Instantiate(drop, new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity);
            }

            Debug.Log("called Enemy2 die()");
            Destroy(gameObject);
        }
    }
}