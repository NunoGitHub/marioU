using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type : MonoBehaviour
{
    [SerializeField]
    public enum MovementType
    {
        win =100,
        goomba=20,
    }

    public MovementType movementType;


}
