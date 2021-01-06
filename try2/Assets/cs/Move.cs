using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Debug = UnityEngine.Debug;
using UnityEngine;
using System;

public class Move : MonoBehaviour
{
    public bool tap, detUp, detLe, detRi, detDo;
    public float left, right, lim;
    public Transform Player;
    public Vector2 startTouchPosition, endTouchPosition;
    public int counter;
    public float distanceFromGround;
    public static bool isGrounded, swipeLeft, swipeRight, secondJump, firstJump, isSlideDown;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        if (!CollCheck.HasLost && Time.timeScale == 1f)
        {
            Player.position = new Vector3(Player.position.x, Player.position.y, (float)-6);
            if (!swipeRight && !swipeLeft)
            {
                float temp = (float)System.Math.Round(Player.position.x);
                Player.position = new Vector3(temp, Player.position.y, (float)-6);
            }
            lim++;
            SwipeCheckInPhone();

            isSlideDown = (detDo || (Input.GetKey(KeyCode.DownArrow)));
            isGrounded = isOnGround();
            if (isGrounded)
            {
                firstJump = secondJump = false;
            }
            SwipeCheck();
            Movement();
        }
    }
    // detecting swipes in phone and its direction
    void SwipeCheckInPhone()
    {
        //checking touches in phone
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouchPosition = Input.mousePosition;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouchPosition = endTouchPosition = Vector2.zero;
            }
        }
        //checking touches in computer

        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouchPosition = endTouchPosition = Vector2.zero;
        }

        //Calculate distance
        endTouchPosition = Vector2.zero;
        if (startTouchPosition != Vector2.zero)
        {
            //for moblie
            if (Input.touches.Length != 0)
            {
                endTouchPosition = Input.touches[0].position - startTouchPosition;
            }
            //for computer
            else if (Input.GetMouseButton(0))
            {
                endTouchPosition = (Vector2)Input.mousePosition - startTouchPosition;
            }
        }
        //confirms that the swipe is not a mistake
        if (endTouchPosition.magnitude > 100)
        {
            float tempX = endTouchPosition.x;
            float tempY = endTouchPosition.y;
            detRi = detUp = detLe = detRi = false;
            if (Math.Abs(tempX) > Math.Abs(tempY))
            {
                //left or right
                if (tempX > 0)
                {
                    detRi = true;
                }
                else
                {
                    detLe = true;
                }
            }
            else
            {
                //up or down
                if (tempY > 0)
                {
                    detUp = true;
                }
                else
                {
                    detDo = true;
                }
            }
            //restarting the start touch
            startTouchPosition = endTouchPosition = Vector2.zero;
        }
    }
    //moving the character left and right by the data we got from the player
    void Movement()
    {
        if (swipeLeft)
        {
            swipeRight = false;
            //if posiball moving to the left way
            Player.position -= new Vector3((float)0.07, 0, 0);
            if (Player.position.x + 1 <= left)
            {
                swipeLeft = false;
                System.Math.Round(Player.position.x);
                detLe = false;
            }
        }
        if (swipeRight)
        {
            swipeLeft = false;
            //if posiball moving to the right way
            Player.position += new Vector3((float)0.07, 0, 0);
            if (Player.position.x - 1 >= right)
            {
                swipeRight = false;

                System.Math.Round(Player.position.x);
                detRi = false;
            }
        }
    }
    //checking swipes data
    void SwipeCheck()
    {
        if (lim >= 5 && (detUp || (Input.GetKey(KeyCode.UpArrow))))
        {
            if (!firstJump)
            {
                Player.position = new Vector3(Player.position.x, Player.position.y + (float)0.1, Player.position.z);
                GetComponent<Rigidbody>().velocity = new Vector3(0, (float)3.5, 0);
                lim = 0;
                detUp = false;
                firstJump = true;
                //counter = 50;
            }
            else if (firstJump && !secondJump && lim >= 5)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, (float)3, 0);
                lim = 0;
                detUp = false;
                secondJump = true;
            }

        }
        if ((lim >= 5 && (detDo || (Input.GetKey(KeyCode.DownArrow)))))
        {
            if (!isGrounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, (float)-3.5, 0);
                lim = 0;
            }
            detDo = false;

        }
        if (lim >= 5 && ((!swipeRight && detRi) || (Input.GetKey(KeyCode.RightArrow) && !swipeRight)))
        {
            if ((Player.position.x < 1 && !swipeLeft) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                swipeRight = true;
                right = Player.position.x;
                System.Math.Round(right);
                lim = 0;
            }
        }
        if (lim >= 5 && ((!swipeLeft && detLe) || (Input.GetKey(KeyCode.LeftArrow) && !swipeLeft)))
        {
            if ((Player.position.x > -1 && !swipeRight) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                swipeLeft = true;
                left = Player.position.x;
                System.Math.Round(left);
                lim = 0;
            }
        }
    }
    //checking if the player is not jumping
    bool isOnGround()
    {
        return (Physics.Raycast(Player.position, Vector3.down, distanceFromGround));
        //if (counter <= 0)
        //{
        //    return (Physics.Raycast(Player.position, Vector3.down, distanceFromGround));
        //}
        //else
        //{
        //    counter--;
        //    return false;
        //}
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        Physics.gravity = new Vector3(0, -8f, 0);
        var rigidBody = GetComponentsInChildren<Rigidbody>(true);
        foreach (var component in rigidBody)
        {
            if (component.name == "Cube")
                component.useGravity = true;
        }

        tap = swipeLeft = swipeRight = detUp = detRi = detLe = detDo = firstJump = secondJump = isSlideDown = false;
        left = right = lim = 0;
        Player.position = new Vector3((float)0, (float)0.05, (float)-6);
        counter = 0;
        distanceFromGround = 0.1f;
        isGrounded = true;
    }
}
