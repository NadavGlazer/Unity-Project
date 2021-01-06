using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator animator;
    public bool left, right, isGrounded, Fjump, SJump, hasLost, first;
    public int frameCount;
    public static bool booliann, slideDown;
    float timeSince;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasLost)
        {
            //updating the boolians
            left = Move.swipeLeft;
            right = Move.swipeRight;
            isGrounded = Move.isGrounded;
            Fjump = Move.firstJump;
            SJump = Move.secondJump;
            hasLost = CollCheck.HasLost;

            if (first)
            {
                slideDown = isGrounded && Move.isSlideDown;
            }
            else
            {
                slideDown = !Fjump && isGrounded && !right && !left;
            }

            if (slideDown && (first || Move.isSlideDown))
            {
                first = false;
                timeSince = Time.time + 1.5f;
            }

            if (slideDown && timeSince <= Time.time)
            {
                slideDown = false;
                first = true;
            }

            CheckThings();
        }
        if (hasLost)
        {
            right = left = SJump = Fjump = slideDown = false;
            if (!booliann && frameCount < 200)
            {
                animator.SetBool("HasLost", true);
                frameCount++;
            }
            if (frameCount >= 200)
            {
                animator.SetBool("HasLost", false);
                booliann = true;
            }
        }
    }
    //animating the player
    void CheckThings()
    {
        if (left)
        {
            animator.SetBool("SlideLeft", true);
        }
        else
        {
            animator.SetBool("SlideLeft", false);
        }
        if (right)
        {
            animator.SetBool("SlideRight", true);
        }
        else
        {
            animator.SetBool("SlideRight", false);
        }
        if (Fjump)
        {
            animator.SetBool("FJump", true);
        }
        else
        {
            animator.SetBool("FJump", false);
        }
        if (SJump)
        {
            animator.SetBool("SJump", true);
        }
        else
        {
            animator.SetBool("SJump", false);
        }
        if (isGrounded && (!right && !left) && !slideDown)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
        if (slideDown)
        {

            animator.SetBool("isSlideDown", true);
        }
        else
        {
            animator.SetBool("isSlideDown", false);
            first = true;
        }
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        animator = GetComponent<Animator>();
        booliann = slideDown = false;
        first = true;
        frameCount = 0;
        timeSince = 0;
    }
}
