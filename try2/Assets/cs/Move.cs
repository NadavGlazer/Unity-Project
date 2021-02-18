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
    public Transform Player;
    public GameObject Camera;
    public static bool IsGrounded;
    public static bool SwipeLeft;
    public static bool SwipeRight;
    public static bool SecondJump;
    public static bool FirstJump;
    public static bool IsSlideDown;
    bool tap;
    bool detUp;
    bool detLe;
    bool detRi;
    bool detDo;
    float left;
    float right;
    float frameAmount;
    float distanceFromGround;
    float playerZ;
    float movePositionPerFrameLR;
    float firstJumpForce;
    float secondJumpForce;
    float slideDownForce;
    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    int pixelAmountForSwipe;
    int frameAmountBetwinMovement;
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
            if (!SwipeRight && !SwipeLeft)
            {
                float temp = (float)System.Math.Round(Player.position.x);
                Player.position = new Vector3(temp, Player.position.y, playerZ);
            }
            frameAmount++;
            SwipeCheckInPhone();

            IsSlideDown = (detDo || (Input.GetKey(KeyCode.DownArrow)));
            IsGrounded = isOnGround();
            if (IsGrounded)
            {
                FirstJump = SecondJump = false;
            }
            CheckingIfAbleToMoveThePlayer();
            SettingPositionBasedOnSwipes();
            Camera.transform.position = new Vector3(Player.position.x, Player.position.y + 1.8f, Camera.transform.position.z);
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
        if (SwipeLeft)
        {
            SwipeRight = false;
            //if posiball moving to the left way
            Player.position -= new Vector3(movePositionPerFrameLR, 0, 0);
            if (Player.position.x + 1 <= left)
            {
                SwipeLeft = false;
                System.Math.Round(Player.position.x);
                detLe = false;
            }
        }
        if (SwipeRight)
        {
            SwipeLeft = false;
            //if posiball moving to the right way
            Player.position += new Vector3(movePositionPerFrameLR, 0, 0);
            if (Player.position.x - 1 >= right)
            {
                SwipeRight = false;
                System.Math.Round(Player.position.x);
                detRi = false;
            }
        }
    }
    //checking swipes data
    void CheckingIfAbleToMoveThePlayer()
    {
        if (frameAmount >= frameAmountBetwinMovement && (detUp || (Input.GetKey(KeyCode.UpArrow))))
        {
            if (!FirstJump)
            {
                Player.position = new Vector3(Player.position.x, Player.position.y + (float)0.1, Player.position.z);
                GetComponent<Rigidbody>().velocity = new Vector3(0, firstJumpForce, 0);
                frameAmount = 0;
                detUp = false;
                FirstJump = true;
            }
            else if (FirstJump && !SecondJump && frameAmount >= frameAmountBetwinMovement)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, secondJumpForce, 0);
                frameAmount = 0;
                detUp = false;
                SecondJump = true;
            }
        }
        if ((frameAmount >= frameAmountBetwinMovement && (detDo || (Input.GetKey(KeyCode.DownArrow)))))
        {
            if (!IsGrounded)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, slideDownForce, 0);
                frameAmount = 0;
            }
            detDo = false;
        }
        if (frameAmount >= frameAmountBetwinMovement && ((!SwipeRight && detRi) || (Input.GetKey(KeyCode.RightArrow) && !SwipeRight)))
        {
            if ((Player.position.x < 1 && !SwipeLeft) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                SwipeRight = true;
                right = Player.position.x;
                System.Math.Round(right);
                frameAmount = 0;
            }
        }
        if (frameAmount >= frameAmountBetwinMovement && ((!SwipeLeft && detLe) || (Input.GetKey(KeyCode.LeftArrow) && !SwipeLeft)))
        {
            if ((Player.position.x > -1 && !SwipeRight) && System.Math.Abs(Player.position.x - System.Math.Round(Player.position.x)) < 0.1)
            {
                SwipeLeft = true;
                left = Player.position.x;
                System.Math.Round(left);
                frameAmount = 0;
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

        tap = SwipeLeft = SwipeRight = detUp = detRi = detLe = detDo = FirstJump = SecondJump = IsSlideDown = false;
        left = right = frameAmount = 0;
        Player.position = new Vector3((float)0, (float)0.05, (float)-6);
        distanceFromGround = 0.1f;
        IsGrounded = true;
        playerZ = -6f;
        pixelAmountForSwipe = 100;
        movePositionPerFrameLR = 0.07f;
        frameAmountBetwinMovement = 1;
        firstJumpForce = 3.3f;
        secondJumpForce = 1.9f;
        slideDownForce = -4f;
    }
}
