using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public static bool DeathAnimationFinished;
    public static bool SlideDown;
    public Animator animator;
    bool left;
    bool right;
    bool isGrounded;
    bool Fjump;
    bool SJump;
    bool hasLost;
    bool first;
    int frameCount;
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
            left = Move.SwipeLeft;
            right = Move.SwipeRight;
            isGrounded = Move.IsGrounded;
            Fjump = Move.FirstJump;
            SJump = Move.SecondJump;
            hasLost = CollCheck.HasLost;

            if (first)
            {
                SlideDown = isGrounded && Move.IsSlideDown;
            }
            else
            {
                SlideDown = !Fjump && isGrounded && !right && !left;
            }

            if (SlideDown && (first || Move.IsSlideDown))
            {
                first = false;
                timeSince = Time.time + 1.5f;
            }

            if (SlideDown && timeSince <= Time.time)
            {
                SlideDown = false;
                first = true;
            }

            UpdateAnimation();
        }
        if (hasLost)
        {
            right = left = SJump = Fjump = SlideDown = false;
            if (!DeathAnimationFinished && frameCount < 200)
            {
                animator.SetBool("HasLost", true);
                frameCount++;
            }
            if (frameCount >= 200)
            {
                animator.SetBool("HasLost", false);
                DeathAnimationFinished = true;
            }
        }
    }
    //animating the player
    void UpdateAnimation()
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
        if (isGrounded && (!right && !left) && !SlideDown)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }
        if (SlideDown)
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
        DeathAnimationFinished = SlideDown = false;
        first = true;
        frameCount = 0;
        timeSince = 0;
    }
}
