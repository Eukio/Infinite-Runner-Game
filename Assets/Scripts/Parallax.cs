using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer rendender;
    [SerializeField] float xOffset;
    float speed;
    [SerializeField] GameObject player;
    bool moveBackground;
    void Start()
    {
        rendender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
            xOffset += speed/10 * Time.deltaTime;
            rendender.material.mainTextureOffset = new Vector2(xOffset, rendender.material.mainTextureOffset.y);

    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
