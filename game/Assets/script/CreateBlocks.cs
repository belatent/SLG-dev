using UnityEngine;
using System.Collections;

public class CreateBlocks : MonoBehaviour {
    BoxCollider backGround;
    float bkWidth;
    float bkLength;

    BoxCollider block;
    float blockWidth;
    float blockLength;

    int length;
    int width;

    public GameObject singleBlock;
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
        length = (int)(bkLength / blockLength);
        width = (int)(bkWidth / blockWidth);
    }

    void createBlocks()
    {
        //print(singleBlock.GetComponents<BoxCollider>().Length);
        //foreach(BoxCollider collider in singleBlock.GetComponents<BoxCollider>())
        //    Destroy(collider);

        //singleBlock.AddComponent<BoxCollider>();
        //singleBlock.GetComponent<BoxCollider>().size.Set(blockLength,blockWidth,(float)0.2);

        for(int i = -length/2; i < length/2; i++)
        {
            for(int j = -width/2; j < width/2; j++)
            {
                singleBlock = (GameObject)Instantiate(singleBlock, new Vector3(i*blockLength + (float)1.28/2, j* blockWidth + (float)1.28/2, 0), Quaternion.identity);
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
