using UnityEngine;
using System.Collections;

public class helperMethods {

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
}
