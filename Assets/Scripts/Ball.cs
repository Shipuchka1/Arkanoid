using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    private bool ballIsActive;
    private Vector3 ballPosition;
    private Vector2 ballInitialForce;
    public GameObject playerObject;
    private Rigidbody2D rb;
    private float timeHorizontal;
    private const float velocityModule = 5.0f;
    public bool visible;
    public float flipFlopTimer;
    public SpriteRenderer spr;
    public AudioClip hitSound;
    // Use this for initialization
    void Start () {

        visible = true;
        timeHorizontal = -1.0f;
        rb = GetComponent<Rigidbody2D>();
        ballInitialForce = new Vector2(100.0f, 300.0f);
        ballIsActive = false;
        ballPosition = transform.position;
        spr = GetComponent<SpriteRenderer>();
       


    }

    // Update is called once per frame
    void Update() {
   
        if(!visible)
        {
            if(flipFlopTimer>0)
            {
                flipFlopTimer -= Time.deltaTime;
            }
            else 
            {
                if(spr.enabled)
                {
                    spr.enabled = false;
                    flipFlopTimer = 0.8f;
                }
                else
                {
                    spr.enabled = true;
                    flipFlopTimer = 0.8f;
                }
            }
        }
        // проверка нажатия на пробел
        if (Input.GetButtonDown("Jump") == true)
        {
            // проверка состояния
            if (!ballIsActive)
            {
                rb.isKinematic = false;
                rb.velocity = new Vector2(0.0f,0.0f);
                // применим силу
                rb.AddForce(ballInitialForce);
                
                // зададим активное состояние
                ballIsActive = !ballIsActive;
            }
        }
        if (ballIsActive)
        {
            rb.velocity += rb.velocity*Time.deltaTime*0.04f;
            if (Math.Abs(rb.velocity.y) < 0.1f)
            { 

                if (timeHorizontal < 0)
                    timeHorizontal = Time.realtimeSinceStartup;
                else if (Time.realtimeSinceStartup - timeHorizontal > 5)
                {
                    rb.AddForce(new Vector2(0, 10f));
                }
            }
             else timeHorizontal = -1.0f;
    
        }
       
        if (!ballIsActive && playerObject != null)
            {
                // задаем новую позицию шарика
                ballPosition.x = playerObject.transform.position.x;

                // устанавливаем позицию шара
                transform.position = ballPosition;
            }

        if (ballIsActive && transform.position.y < -6)
        {
            ballIsActive = !ballIsActive;
            ballPosition.x = playerObject.transform.position.x+1.0f;
            ballPosition.y = -4.0f;
            transform.position = ballPosition;

            rb.isKinematic = true;
            spr.enabled = true;

            //  playerObject.SendMessage("TakeLife");
            playerObject.SendMessage("Damage");
        }

    }

    public void ResetVelocity()
    {
        
        float factor = velocityModule / rb.velocity.magnitude;
        rb.velocity *= factor;
       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ballIsActive)
        {
            GetComponent<AudioSource>().PlayOneShot(hitSound);
        }
    }
}
