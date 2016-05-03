using UnityEngine;
using System.Collections;

public class Camear : MonoBehaviour {
    Vector3[] Pos = new Vector3[2];
    int point = 0;
    public GameObject player;
    public GameObject background;
    public GameObject singleBlock;

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

            if(Physics.Raycast(ray, out hit) && (hit.collider.tag.Equals("path"))) { 

                print(hit.collider.name);

                Pos[0] = new Vector3(hit.collider.transform.position.x, player.transform.position.y, 0);
                Pos[1] = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, 0);

                point = 0;
                move();
            }

        }
    }

    void findPassableBlocks()
    {
        float length = CreateBlocks.length;
        float width = CreateBlocks.width;
        float bkLength = background.GetComponent<BoxCollider>().size.x;
        float bkWidth = background.GetComponent<BoxCollider>().size.y;
        float blockLength = singleBlock.GetComponent<BoxCollider>().size.x;
        float blockWidth = singleBlock.GetComponent<BoxCollider>().size.y;

        Vector3[,] paths = new Vector3[(int)length, (int)width];

        int currLen = 0;
        int currWid = 0;

        Vector3 position;
        for (int i = (int)-length / 2 - 1; i < length / 2; i++)
        {
            for (int j = (int)-width / 2 - 1; j < width / 2; j++)
            {
                position = new Vector3(i * blockLength + (float)blockLength / 2, j * blockWidth + (float)blockWidth / 2, 0);

                RaycastHit hit;
                Physics.Raycast(position, Vector3.back, out hit, 1);

                if (hit.collider.tag.Equals("path"))
                    paths[currLen, currWid] = position;
                else
                    paths[currLen, currWid] = new Vector3(0,0,0);
                currWid++;
            }
            currLen++;
        }
    }
}
