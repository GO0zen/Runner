using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public Obstacle transformTemplate;

   

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;
    }


    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        groundHeight = transform.position.y + (collider.size.y / 2);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;


        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        GameObject transformator = Instantiate(transformTemplate.gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        BoxCollider2D trCollider = transformator.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.7f;
        maxY += groundHeight;
        float minY = 1;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2; 
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);


        GroundFall fall = go.GetComponent<GroundFall>();
        if (fall != null)
        {
            Destroy(fall);
            fall = null;
        }

        if (Random.Range(0,3) == 0)
        {
            fall = go.AddComponent<GroundFall>();
            fall.fallSpeed = Random.Range(1.0f, 3.0f);
        }


        int obstacleBoxNum = Random.Range(0, 4);
        for (int i=0; i<obstacleBoxNum; i++)
        {
            GameObject box = Instantiate(boxTemplate.gameObject);
            float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            float x = Random.Range(left, right);
            Vector2 boxPos = new Vector2(x, y);
            box.transform.position = boxPos;

            if (fall != null)
            {
                Obstacle o = box.GetComponent<Obstacle>();
                fall.obstacles.Add(o);
            }
        }

        //int chance = Random.Range(0, 2);
        //if(chance == 1)
        //{
        //    GameObject Transformator = Instantiate(transformTemplate.gameObject);
        //    float y = goGround.groundHeight;
        //    float halfWidth = goCollider.size.x / 2 - 1;
        //    float left = go.transform.position.x - halfWidth;
        //    float right = go.transform.position.x + halfWidth;
        //    float x = Random.Range(left - 2, right - 2);
        //    Vector2 transformPos = new Vector2(x, y);
        //    Transformator.transform.position = transformPos;
        //}

        int obstacleTransformNum = Random.Range(0, 2);
        Debug.Log(obstacleTransformNum);
        for(int i = 0; i < obstacleTransformNum; i++)
        {
            //float y = goGround.groundHeight;
            float halfWidth = goCollider.size.x / 2 - 1;
            //float transformHalfWidth = trCollider.size.x / 2 - 1;
            //float trLeft = trCollider.transform.position.x - transformHalfWidth;
            //float trRight = trCollider.transform.position.x + transformHalfWidth;
            float left = go.transform.position.x - halfWidth;
            float right = go.transform.position.x + halfWidth;
            
            
            float x = Random.Range(left, right);
            
            
            Vector2 transformPos = new Vector2(x, goGround.groundHeight);
            transformator.transform.position = transformPos;

            if (fall != null)
            {
                Obstacle trans = transformator.GetComponent<Obstacle>();
                fall.obstacles.Add(trans);
            }
        }
        

    }

}
