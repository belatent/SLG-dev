using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject MainCamera;
    public BlockCreater bc;

    // Use this for initialization
    void Start () {
        bc = (BlockCreater)this.gameObject.GetComponent("BlockCreater");
        bc.blocklist = bc.createMap();
	}
	
	// Update is called once per frame
	void Update () {
        checkClick();
	}

    void checkClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //从屏幕点击处向内发射射线，若射到碰撞器，开始判定
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag.Equals("Player"))
                {
                    bc.createRange(hit.collider.gameObject);
                }else if (hit.collider.tag.Equals("path"))
                {

                }
            }
        }
    }
}
