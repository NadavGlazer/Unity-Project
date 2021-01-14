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
    bool tap, detUp, detLe, detRi, detDo;
    float left, right, FrameAmount, distanceFromGround;
    public Transform Player;
    Vector2 startTouchPosition, endTouchPosition;
    public static bool isGrounded, swipeLeft, swipeRight, secondJump, firstJump, isSlideDown;
    float playerZ, movePositionPerFrameLR, firstJumpForce, secondJumpForce, slideDownForce;
    int pixelAmountForSwipe, frameAmountBetwinMovement;
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
            Player.position = new Vector3(Player.position.x, Player.position.y, playerZ);
            if (!swipeRight && !swipeLeft)
            {
                float temp = (float)System.Math.Round(Player.position.x);
                Player.position = new Vector3(temp, Player.position.y, playerZ);
            }
            FrameAmount++;
            SwipeCheckInPhone();

            isSlideDown = (detDo || (Input.GetKey(KeyCode.DownArrow)));
            isGrounded = isOnGround();
            if (isGrounded)
            {
                firstJump = secondJump = false;
            }
            CheckingIfAbleToMoveThePlayer();
            SettingPositionBasedOnSwipes();
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
        if (endTouchPosition.magnitude > pixelAmountForSwipe)
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
    void SettingPositionBasedOnSwipes()
    {
        if (swipeLeft)
        {
            swipeRight = false;
            //if posiball moving to the left way
            Player.position -= new Vector3(movePositionPerFrameLR, 0, 0);
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
            Player.position += new Vector3(movePositionPerFrameLR, 0, 0);
            if (Player.position.x - 1 >= right)
            {
                swipeRight = false;
                System.Math.Round(Player.position.x);
                detRi = false;
            }
        }
    }
    //checking swipes data
    void CheckingIfAbleToMoveThePlayer()
    {
        if (FrameAmount >= frameAmountBetwinMovement && (detUp || (Input.GetKey(KeyCode.UpArrow))))
        {
            if (!firstJump)
            {
                Player.position = new Vector3(Player.position.x, Player.position.y + (float)0.1, Player.position.z);
                GetComponent<Rigidbody>().velocity = new Vector3(0, firstJumpForce, 0);
                FrameAmount = 0;
                detUp = false;
                firstJump = true;
            }
            else if (firstJump && !secondJump && FrameAmount >= frameAmountBetwinMovement)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, secondJumpForce, 0);
                FrameAmount = 0;
                detUp = false;
                secondJump = true;
            }
        }
        if ((FrameAmount >= frameAmountBetwinMovement && (detDo || (Input.GetKey(KeyCode.DownArrow)))))
        {
            if (!isGrounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, slideDownForce, 0);
                FrameAmount = 0;
            }
            detDo = false;
        }
        if (FrameAmount >= frameAmountBetwinMovement && ((!swipeRight && detRi) || (Input.GetKey(KeyCode.RightArrow) && !swipeRight)))
        {
            if ((Player.position.x < 1 && !swipeLeft) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                swipeRight = true;
                right = Player.position.x;
                System.Math.Round(right);
                FrameAmount = 0;
            }
        }
        if (FrameAmount >= frameAmountBetwinMovement && ((!swipeLeft && detLe) || (Input.GetKey(KeyCode.LeftArrow) && !swipeLeft)))
        {
            if ((Player.position.x > -1 && !swipeRight) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                swipeLeft = true;
                left = Player.position.x;
                System.Math.Round(left);
                FrameAmount = 0;
            }
        }
    }
    //checking if the player is not jumping
    bool isOnGround()
    {
        return (Physics.Raycast(Player.position, Vector3.down, distanceFromGround));
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
        left = right = FrameAmount = 0;
        Player.position = new Vector3((float)0, (float)0.05, (float)-6);
        distanceFromGround = 0.1f;
        isGrounded = true;
        playerZ = -6f;
        pixelAmountForSwipe = 100;
        movePositionPerFrameLR = 0.07f;
        frameAmountBetwinMovement = 5;
        firstJumpForce = 3.5f;
        secondJumpForce = 3f;
        slideDownForce = -3.5f;
    }
}
