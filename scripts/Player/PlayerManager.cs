using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//用playermanager获取player instance，避免find game object的性能问题
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);//如果已经有instance，删除当前instance，保留已有instance
        else
            instance = this;
    }
}
