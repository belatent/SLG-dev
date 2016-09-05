using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public class attribute {
        int level;
        public int movingRange { get; set; }
        int atk;
        int def;
        int speed;
        int magicAtk;
        int magicDef;
        float avo;
        int luck;

        public attribute()
        {
            movingRange = 4;
        }
    }

    public attribute attr { get; set; }

    // Use this for initialization
    void Start()
    {
        attr = new attribute();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
