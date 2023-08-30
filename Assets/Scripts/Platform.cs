using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    //Randomize heights // locations depending on the player's location
    //delete when left side, spawn on right
    int randomSize;
    bool movement;
    float leftEdge;
    float rightEdge;
    float spriteWidth;
    float speed;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.lossyScale.x;
        rightEdge = transform.position.x + (spriteWidth / 2);
        leftEdge = transform.position.x - (spriteWidth / 2);
        if (transform.position.x < -20f && gameObject.name != "startPlatform")
        {
            Destroy(gameObject);
        }
        else if(transform.position.x < -20f && gameObject.name == "startPlatform")
            movement= false;
        if (movement)
        {
            GetComponent<Rigidbody2D>().velocity = -transform.right * speed;

        }
        else
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    public void Moving(bool movement)
    {
        this.movement = movement;
    }
    public float GetRightEdge()
    {
return rightEdge;
    }
    public float GetLeftEdge()
    {
        return leftEdge;
    }
    public float GetSpriteWidth()
    {
        return spriteWidth;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
