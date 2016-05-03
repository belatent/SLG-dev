using UnityEngine;
using System.Collections;

public class CreateBlocks : MonoBehaviour {
    BoxCollider backGround;
    float bkWidth;
    float bkLength;

    BoxCollider block;
    float blockWidth;
    float blockLength;

    public static float length;
    public static float width;

    public GameObject singleBlock;
    public GameObject singlePath;
	// Use this for initialization
	void Start () {
        setUp();
        print("block nums\t"+length+"\t"+width);
        print("block size\t\t" + blockLength + "\t" + blockWidth);
        print("background size\t" + bkLength + "\t" + bkWidth);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void getInfo()
    {
        //get background size
        backGround = this.gameObject.GetComponent<BoxCollider>();
        bkLength = backGround.size.x;
        bkWidth = backGround.size.y;
        //get block size
        block = singleBlock.GetComponent<BoxCollider>();
        blockLength = block.size.x;
        blockWidth = block.size.y;
    }

    void setUpBlocksLW()
    {
        length = bkLength / blockLength;
        width = bkWidth / blockWidth;
    }

    void createBlocks()
    {
        Vector3 position;
        for (int i = (int)-length/2 -1; i < length/2; i++)
        {
            for(int j = (int)-width/2 -1; j < width/2; j++)
            {
                position = new Vector3(i * blockLength + (float)blockLength / 2, j * blockWidth + (float)blockWidth / 2, 0);
                
                RaycastHit hit;

                if (Physics.Raycast(position,Vector3.forward, out hit,1))
                {
                    print(hit.collider.name);

                    if (hit.collider.name.Equals("Objects"))
                        singleBlock = (GameObject)Instantiate(singleBlock, position, Quaternion.identity);
                    else
                        singlePath = (GameObject)Instantiate(singlePath, position, Quaternion.identity);
                }
                else
                    singlePath = (GameObject)Instantiate(singlePath, position, Quaternion.identity);
            }
        }
    }


    void setUp()
    {
        getInfo();
        setUpBlocksLW();
        createBlocks();
    }

}
