using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {
    public GameObject MainCamera;
    MapCreator mc;
    RangeCreator rc;
    PathFinder pf;
    UIManager uim;
    public GameObject focusPlayer { get; set; } //player current operating
    public bool focusThis{get;set;}

    bool battleMenuCreated = false;

    // Use this for initialization
    void Start () {
        //setup tools
        focusThis = true;
        mc = (MapCreator)this.gameObject.GetComponent("MapCreator");
        rc = (RangeCreator)this.gameObject.GetComponent("RangeCreator");
        pf = (PathFinder)this.gameObject.GetComponent("PathFinder");
        uim = (UIManager)this.gameObject.GetComponent("UIManager");
        //init
        mc.init();
        rc.init();
        pf.init();
    }
	
	// Update is called once per frame
	void Update () {
        //check click per frame
        checkClick();
	}

    void checkClick()
    {
        if (!focusThis)
            return;
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
                    focusPlayer = hit.collider.gameObject;
                    uim.createBattleMenu(hit.collider.transform.position);
                }else if (hit.collider.tag.Equals("moving range"))
                {
                    pf.setNextPath(hit);
                    rc.destoryBlockByTag("moving range");
                }
            }
        }
    }
}
