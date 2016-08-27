using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {
    public GameObject MainCamera;
    BlockCreator bc;
    PathFinder pf;
    GameObject focusPlayer; //player current operating

    bool rangeCreated = false;

    // Use this for initialization
    void Start () {
        //setup tools
        bc = (BlockCreator)this.gameObject.GetComponent("BlockCreator");
        pf = (PathFinder)this.gameObject.GetComponent("PathFinder");
        //create map
        bc.blocklist = bc.createMap();
        pf.getBlockList();
    }
	
	// Update is called once per frame
	void Update () {
        //check click per frame
        checkClick();
	}

    void checkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            GameObject hitPlace;//game object been hit
            //从屏幕点击处向内发射射线，若射到碰撞器，开始判定
            if (Physics.Raycast(ray, out hit))
            {
                hitPlace = hit.collider.gameObject;
                print("Hit Object: " + hit.collider.name);

                if (hit.collider.tag.Equals("Player"))
                {
                    if (!rangeCreated)
                    {
                        bc.createRange(hit.collider.gameObject);
                        rangeCreated = true;
                    }
                }else if (hit.collider.tag.Equals("moving range"))
                {
                    pf.setNextPath(hit);
                    bc.destoryBlockByTag("moving range");
                    rangeCreated = false;
                }
            }
        }
    }
}
