using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {


    public GameObject boostLife;
    public GameObject boostDeath;
    public GameObject boostIncrease;
    public GameObject boostDecrease;
    public GameObject boostInvisible;
    public GameObject boostSlow;
    public int hitsToKill;
    public int points;
    private int numberOfHits;
    public int boostValue;
    
    // Use this for initialization
    void Start () {

      
        numberOfHits = 0;
        System.Random rnd = new System.Random();
        Block[] blocks = GameObject.FindObjectsOfType<Block>();

        foreach (Block item in blocks)
        {
            int tmp = rnd.Next(0, 100);
            
            if (tmp < 50)item.boostValue = 0;
            else item.boostValue = tmp % 6 + 1;
           
        }
        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            numberOfHits++;

            if (numberOfHits == hitsToKill)
            {
                GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];             
                player.SendMessage("addPoints", points);
           
                switch (boostValue)
                {
                    case 1:
                        Instantiate<GameObject>(boostLife, gameObject.transform.position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate<GameObject>(boostDeath, gameObject.transform.position, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate<GameObject>(boostIncrease, gameObject.transform.position, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate<GameObject>(boostDecrease, gameObject.transform.position, Quaternion.identity);
                        break;
                    case 5:
                        Instantiate<GameObject>(boostInvisible, gameObject.transform.position, Quaternion.identity);
                        break;
                    case 6:
                        Instantiate<GameObject>(boostSlow, gameObject.transform.position, Quaternion.identity);
                        break;
                    default:
                        break;
                }
             
                Destroy(this.gameObject);
            }
        }
    }
}
