using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosRev = new Vector3(-0.35f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotRev = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        bool isRev = player.flipX;

        if (isLeft) //근접
        {
            transform.localRotation = isRev ? leftRotRev : leftRot;
            spriter.flipY = isRev;
            spriter.sortingOrder = isRev ? 4 : 6;
        }
        else //원거리
        {
            transform.localPosition = isRev ? rightPosRev : rightPos;
            spriter.flipX = isRev;
            spriter.sortingOrder = isRev ? 6 : 4;
        }
    }
}
