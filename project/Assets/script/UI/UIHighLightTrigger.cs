using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHighLightTrigger : MonoBehaviour
{
    public int pictLimit;

    string currHighLight = null;
    GameObject highlightObj;
    int currentPict = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mouseAbove();
        if (Input.GetMouseButtonDown(0))
        {
            mouseClick();
        }
    }

    void mouseAbove()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hitPlace;//game object been hit
        //从屏幕点击处向内发射射线，若射到碰撞器，开始判定
        if (Physics.Raycast(ray, out hit))
        {
            hitPlace = hit.collider.gameObject;

            print("Hit Object: " + hitPlace.name + "    " + currHighLight);
            if (currHighLight != null && !hitPlace.name.Equals(currHighLight))
            {
                cleanHighLights();
            }
            if (hitPlace.tag.Equals("UI-MainScene") && currentPict < pictLimit)
            {
                
                currHighLight = hitPlace.name;
                switch (hitPlace.name)
                {
                    case "stairs":
                        highlightObj = (GameObject)Resources.Load("perfab/stairs_hl");
                        break;
                    case "managerDoor":
                        highlightObj = (GameObject)Resources.Load("perfab/managerDoor_hl");
                        break;
                    case "missionBoard":
                        highlightObj = (GameObject)Resources.Load("perfab/missionBoard_hl");
                        break;
                    case "maid":
                        highlightObj = (GameObject)Resources.Load("perfab/maid_hl");
                        break;
                    case "guest":
                        highlightObj = (GameObject)Resources.Load("perfab/guest_hl");
                        break;
                    case "waiter":
                        highlightObj = (GameObject)Resources.Load("perfab/waiter_hl");
                        break;
                    default:
                        break;
                }
                highlightObj = (GameObject)Instantiate(highlightObj, hitPlace.transform.position, Quaternion.identity);
                currentPict++;
            }
        }
        else
        {
            cleanHighLights();
        }
    }

    void cleanHighLights()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("UI-MainScene-highlight"))
        {
            DestroyObject(obj);
        }
        currentPict = 0;
        currHighLight = null;
    }

    void mouseClick()
    {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hitPlace;//game object been hit
        //从屏幕点击处向内发射射线，若射到碰撞器，开始判定
        if (Physics.Raycast(ray, out hit))
        {
            hitPlace = hit.collider.gameObject;

            print("Click Object: " + hitPlace.name);
            if (hitPlace.tag.Equals("UI-MainScene"))
            {
                switch (hitPlace.name)
                {
                    case "stairs":
                        SceneBuffer.GetBuffer().sceneName = "moving";
                        SceneManager.LoadScene("loading");
                        break;
                    case "managerDoor":
                       
                        break;
                    case "missionBoard":
                        
                        break;
                    case "maid":
                        
                        break;
                    case "guest":
                       
                        break;
                    case "waiter":
                        
                        break;
                    default:
                        break;
                }
                
            }
        }
    }
}