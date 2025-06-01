using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��playermanager��ȡplayer instance������find game object����������
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public Player player;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);//����Ѿ���instance��ɾ����ǰinstance����������instance
        else
            instance = this;
    }
}
