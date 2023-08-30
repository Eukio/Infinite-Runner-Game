using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private bool jump, left, right;
    [SerializeField] float speed; Rigidbody2D rb;
    [SerializeField] Text t;
    [SerializeField] Text lose;
    [SerializeField] Parallax background;
    [SerializeField] Parallax foreground;
    [SerializeField] GameObject RayObject;
    [SerializeField] Text bestTime;
    long startTime;
    long speedStartTime;
    long totalElasped;
    long speedElasped;
    float prevY;
    RaycastHit2D hitWall;
    [SerializeField] Platform platform;
    [SerializeField] Platform startPlatform;
    bool run;
    float worldSpeed;
    bool startParallax;
    float distance;
    long longestTime;
    GameObject p;
    void Start()
    {
        t.text = "";
        bestTime.text = "";
        lose.text = "";
        rb = GetComponent<Rigidbody2D>();
      Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jump = true;
            if (Input.GetKeyDown(KeyCode.A))
            {
                GetComponent<Animator>().SetBool("running", true);

                left = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("running", true);
                right = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
                jump = false;
            if (Input.GetKeyUp(KeyCode.A))
            {
                left = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                right = false;

            }
            if (transform.position.x < -9.7f)
            {
                Vector3 temp = new Vector3(.5f, 0, 0);
                transform.position +=temp;
            }
            if (transform.position.x > -1 && !startParallax)
            {
                foreground.SetSpeed(2.5f);
                background.SetSpeed(2.5f);
                startParallax = true;
               p = Instantiate(platform.gameObject, new Vector3(25f,0f), Quaternion.identity);
                float spriteWidth = p.GetComponent<SpriteRenderer>().sprite.bounds.size.x * p.transform.lossyScale.x;
                p.GetComponent<Platform>().Moving(true);
                startPlatform.SetSpeed(2.5f);
                startPlatform.Moving(true);
                distance = p.GetComponent<Platform>().GetSpriteWidth() + UnityEngine.Random.RandomRange(1f, 3f);
                speedStartTime = DateTime.Now.Ticks;
                startTime = DateTime.Now.Ticks;
                worldSpeed = 2.5f;
                prevY = 0f;



            }
            else if (startParallax)
            {
                t.text = (totalElasped / 1000f).ToString();
                if (13f - p.GetComponent<Platform>().GetRightEdge() > distance)
                {
                    prevY = p.transform.position.y;
                   // Debug.Log(prevY.ToString());
                    SpawnPlatform();
                }
                   

                speedElasped = (DateTime.Now.Ticks - speedStartTime) / 10000;
                totalElasped = (DateTime.Now.Ticks - startTime) / 10000;


            }
            if (speedElasped / 1000f > 3 && startParallax)
            {

                worldSpeed += .2f;
                background.SetSpeed(worldSpeed);
                foreground.SetSpeed(worldSpeed);
                p.GetComponent<Platform>().SetSpeed(worldSpeed);
                RestartTime();
            }
         
        }

        if(transform.position.y < -5f)
        {
            run = false;
            bestTime.text = (longestTime/1000f) + "";
            if (longestTime == 0 || longestTime < totalElasped)
                longestTime = totalElasped;
            bestTime.text = (longestTime / 1000f) + "";

            // lose.text = "You fell and survived for "+(totalElasped/1000)+ " seconds. Your highest time survived is "+(longestTime/1000)+". Press R to restart";
            Reset();
            //StartCoroutine(Reset());
        }
      
       

    }
    private void FixedUpdate()
    {
        hitWall = Physics2D.Raycast(RayObject.transform.position, -Vector2.up);
        Debug.DrawRay(RayObject.transform.position, -Vector2.up * hitWall.distance, Color.red);


        float xChange = 0f;
        float yChange = 0f;
        if (hitWall.distance > .1)
            GetComponent<Animator>().SetBool("jumpMove", true);
        else
            GetComponent<Animator>().SetBool("jumpMove", false);

        if (jump && hitWall.distance <= 0.02)
        {
            rb.AddForce(Vector3.up * 220);
        }
         if (left)
        {
            if(!jump)
            GetComponent<Animator>().SetBool("running", true);
            else
            {
                GetComponent<Animator>().SetBool("jumpMove", true);
                GetComponent<Animator>().SetBool("running", false);
            }
           

            xChange -= speed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (right)
        {
            if (!jump)
                GetComponent<Animator>().SetBool("running", true);
            else
            {
                GetComponent<Animator>().SetBool("jumpMove", true);
                GetComponent<Animator>().SetBool("running", false);
            }
            xChange += speed;
            GetComponent<SpriteRenderer>().flipX = false;
        }
       else  if (!right || !left)
        {
            GetComponent<Animator>().SetBool("running", false);
        }

        if (run == true)
            GetComponent<Transform>().position += new Vector3(xChange * Time.deltaTime, yChange * Time.deltaTime, 0);
    }
   
    public void SpawnPlatform() //Increment parallax speed, change tile speed
    {

        //find 7 - p.right edge > distance 
        //randomly generate size
        //instatate platform that is at 7+size/2
       // Debug.Log(distance);
          //  Debug.Log("Space"+(7f - p.GetComponent<Platform>().GetRightEdge()));
            int scaleX = UnityEngine.Random.Range(3, 5);
        float posY = prevY + UnityEngine.Random.Range(-2.5f, 1.5f);
        p = Instantiate(platform.gameObject, new Vector3(20f, posY), Quaternion.identity);
            p.transform.localScale = new Vector3(scaleX, 1, 1);
        p.GetComponent<Platform>().SetSpeed(worldSpeed);

        if (posY  < 3.5f && posY >= -4.37f)
            p.transform.position = new Vector3(7 + p.GetComponent<Platform>().GetSpriteWidth() / 2, posY);
        else if (posY <= -4f)
        {
            p.transform.position = new Vector3(7 + p.GetComponent<Platform>().GetSpriteWidth() / 2, posY + UnityEngine.Random.Range(1, 1.5f));

        }
        else
            p.transform.position = new Vector3(7 + p.GetComponent<Platform>().GetSpriteWidth() / 2, posY- UnityEngine.Random.Range(5f, 7f));

       

        p.GetComponent<Platform>().Moving(true);
        distance = UnityEngine.Random.RandomRange(13f, 13.5f);

       // p.GetComponent<Platform>().GetSpriteWidth()+ UnityEngine.Random.RandomRange(2f, 3f);
        
    }
    void RestartTime()
    {
        speedStartTime = DateTime.Now.Ticks;
        speedElasped = 0;
    }
    public void Reset()
    {

        RestartTime();
        GetComponent<Animator>().SetBool("running", false);
        worldSpeed = 0;
        background.SetSpeed(worldSpeed);
        foreground.SetSpeed(worldSpeed);
      
        left = false;
        jump = false;
        right= false;
        run = true;
        startParallax= false;
        t.text = "";
        prevY = transform.position.y;
        totalElasped = 0;
        lose.text = "";
        startTime = DateTime.Now.Ticks;
        gameObject.transform.position = new Vector2(-8.94f, 4.72f);
        startPlatform.transform.position = new Vector3(-3.0753f, -1.97f);
        startPlatform.GetComponent<Transform>().localScale = new Vector3(15.74562f, 1.262356f, 1);
        startPlatform.GetComponent<Platform>().Moving(false);
       for (int i = 1; i< GameObject.FindGameObjectsWithTag("Platform").Length; i++){
            Destroy(GameObject.FindGameObjectsWithTag("Platform")[i]);
        }
                   GetComponent<Animator>().SetBool("jumpMove", true);


    }

}
