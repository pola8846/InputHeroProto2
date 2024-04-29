using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGroundChecker
{
    /// <summary>
    /// 땅에 있는지 체크
    /// </summary>
    /// <returns>땅에 있는가?</returns>
    public bool GroundCheck();
}
