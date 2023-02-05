using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaKeeper : MonoBehaviour
{
    public BoxCollider2D battelArea;
    public Collider2D col;
    public bool touchWall;
    public bool touchRight;
    private void Start() {
        if(battelArea == null)
        {
            battelArea = GameObject.Find("Battle Area").GetComponent<BoxCollider2D>();
        }
    }
    void Update()
    {
        touchWall = false;
        if (col.bounds.center.x < battelArea.bounds.min.x)
        {
            touchWall = true;
            touchRight = false;
            transform.position += new Vector3(battelArea.bounds.min.x - col.bounds.center.x, 0);
            PlayMakerFSM.BroadcastEvent("DCBOSS TOUCH AIR WALL");
            PlayMakerFSM.BroadcastEvent("DCBOSS TOUCH AIR WALL LEFT");
        }
        if (col.bounds.center.x > battelArea.bounds.max.x)
        {
            touchWall = true;
            touchRight = true;
            transform.position -= new Vector3(col.bounds.center.x - battelArea.bounds.max.x, 0);
            PlayMakerFSM.BroadcastEvent("DCBOSS TOUCH AIR WALL");
            PlayMakerFSM.BroadcastEvent("DCBOSS TOUCH AIR WALL RIGHT");
        }
        if(col.bounds.center.y < battelArea.bounds.min.y)
        {
            transform.position = new Vector3(0, battelArea.bounds.min.y - col.bounds.center.y);
        }
    }
}
