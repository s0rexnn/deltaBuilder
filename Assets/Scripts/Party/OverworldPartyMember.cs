using System.Collections.Generic;
using UnityEngine;

public class OverworldPartyMember : MonoBehaviour
{
    private enum AxisLock { None, Horizontal, Vertical }

    [SerializeField] private int followDelay = 10;
    [SerializeField] private Animator anim;
    [SerializeField] private AxisLock axisLock = AxisLock.None;

    private readonly List<Vector3> posList = new();
    private readonly List<Vector2> dirList = new();
    private readonly List<float> spdList = new();

    private OverworldPlayerBehaviour player;
    private Vector2 lastDir = Vector2.down;
    private bool isMoving;

    private void Awake()
    {
        player = FindFirstObjectByType<OverworldPlayerBehaviour>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        FollowPlayer();
        HandleAnimations();
        HandleAxisLock();
    }

    private void FollowPlayer()
    {     
        if (player.CurrentSpeed > 0.01f)
        {
            posList.Add(player.transform.position);
            dirList.Add(player.InputDirection);
            spdList.Add(player.CurrentSpeed);
            lastDir = player.InputDirection;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (posList.Count > followDelay && isMoving)
        {
            Vector3 targetPos = posList[0];
            transform.position = targetPos;

            posList.RemoveAt(0);
            dirList.RemoveAt(0);
            spdList.RemoveAt(0);
        }
    }

     private void HandleAxisLock()
    {
        if (dirList.Count == 0)
        {
           axisLock = AxisLock.None;
           return;
        }

        Vector2 dir = dirList[0];
        
        bool hasX = Mathf.Abs(dir.x) > 0f;
        bool hasY = Mathf.Abs(dir.y) > 0f;

        if (!hasX && !hasY)
        {
            axisLock = AxisLock.None;
            return;
        }

        if (axisLock == AxisLock.None)
            axisLock = hasX ? AxisLock.Horizontal : AxisLock.Vertical;

        if (axisLock == AxisLock.Horizontal && !hasX)
            axisLock = AxisLock.None;

        if (axisLock == AxisLock.Vertical && !hasY)
            axisLock = AxisLock.None;
    }

     private void HandleAnimations()
     {
        if (player == null || anim == null) return;
        Vector2 dir = dirList.Count > 0 ? dirList[0] : lastDir;

        if (dirList.Count > 0)
            lastDir = dir;

       anim.SetBool("isMoving", isMoving);
       anim.SetFloat("moveX", dir.x);
       anim.SetFloat("moveY", dir.y);
 

       if (axisLock == AxisLock.Horizontal)
          anim.SetFloat("moveY", 0f);
       else if (axisLock == AxisLock.Vertical)
          anim.SetFloat("moveX", 0f);


      float spd = player.CurrentSpeed;
      if (spd <= 4f) anim.speed = 1f;
      else if (spd <= 6f) anim.speed = 1.3f;
      else anim.speed = 1.7f;
    } 
}
