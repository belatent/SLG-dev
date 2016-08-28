using UnityEngine;
using System.Collections;

public class battleMenu : MonoBehaviour {
    GameObject MainCamera;
    RangeCreator rc;
    PathFinder pf;
    GameObject gmObj;
    GameManager gm;
    GameObject focusPlayer;

    // Use this for initialization
    void Start () {
        init();
    }
	
	// Update is called once per frame
	void Update () {
        checkClick();
    }

    public void init()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gmObj = GameObject.FindGameObjectWithTag("GM");
        gm = (GameManager)gmObj.GetComponent("GameManager");
        focusPlayer = gm.focusPlayer;
        rc = (RangeCreator)gmObj.GetComponent("RangeCreator");
        rc.init();
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

                if (hit.collider.tag.Equals("UI-Move"))
                {
                    rc.createRange(focusPlayer);
                }
                else if (hit.collider.tag.Equals("UI-Attack"))
                {

                }
                else if (hit.collider.tag.Equals("UI-Magic"))
                {

                }
                else if (hit.collider.tag.Equals("UI-Skill"))
                {

                }
                else if (hit.collider.tag.Equals("UI-Item"))
                {

                }
                else if (hit.collider.tag.Equals("UI-Pass"))
                {

                }
                DestroyObject(this.gameObject);
                gm.focusThis = true;
            }
        }
    }
}
