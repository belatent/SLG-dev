using UnityEngine;
using System.Collections;

public class Camear : MonoBehaviour {
    Vector3[] Pos = new Vector3[2];
    int point = 0;
    public GameObject player;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        coordinate();
    }

    void move()
    {
        if (point < 2)
        {
            iTween.MoveTo(player, iTween.Hash("position", Pos[point], "speed", 50f, "caseType", "linear", "Oncomplete", "move", "oncompletetarget", gameObject));
            point++;

        }

    }

    void coordinate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) && !(hit.collider.name.Equals("Objects"))) { 

            print(hit.collider.name);

            Pos[0] = new Vector3(hit.collider.transform.position.x, player.transform.position.y, 0);
            Pos[1] = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0);

            point = 0;
            move();
            }

        }
    }
}
