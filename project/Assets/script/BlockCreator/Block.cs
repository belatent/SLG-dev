using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public GameObject blockPerfab;
    public coordinate coord { get; set; }
    public bool isPath { get; set; }
    public string blockType;
    string blockName;
    string describe;
    //blockEvent eventList;

    public void setCoord(int x, int y, float X, float Y)
    {
        coord = new coordinate(x, y, X, Y);

    }

    public class coordinate
    {
        /// <summary>
        /// 数组横坐标
        /// </summary>
        public int x { get; private set; }
        /// <summary>
        /// 数组纵坐标
        /// </summary>
        public int y { get; private set; }
        /// <summary>
        /// 实际横坐标
        /// </summary>
        public float X { get; private set; }
        /// <summary>
        /// 实际纵坐标
        /// </summary>
        public float Y { get; private set; }

        public coordinate(int x, int y, float X, float Y )
        {
            this.x = x;
            this.y = y;
            this.X = X;
            this.Y = Y;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    
}
