using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour moveBehaviour;

    private IMovable movable;

    void Start()
    {
        if (moveBehaviour != null)
        {
            movable = moveBehaviour as IMovable;
        }
        else
        {
            Debug.LogError("MoveBehaviour not found");
        }
    }

    void Update()
    {
        movable?.Move();
    }
}
