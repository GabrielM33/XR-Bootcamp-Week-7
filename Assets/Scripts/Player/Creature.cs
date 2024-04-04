using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public enum Team
    {
        Player = 0,
        Enemy = 1
    }
    
    public Team team;
    public Transform head;
}
