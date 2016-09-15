using UnityEngine;
using System.Collections;

public class HelperMethods : MonoBehaviour
{

    /// <summary>
    /// 转换float为int，有小数进1
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public int convertFloattoInt(float num)
    {
        if (num % (int)num > 0)
            return (int)num + 1;
        else
            return (int)num;
    }

    public bool checkRangeExist()
    {
        foreach (GameObject gameObj in FindObjectsOfType<GameObject>())
        {
            if (gameObj.tag.Equals("moving range"))
                return true;
        }
        return false;
    }
}
