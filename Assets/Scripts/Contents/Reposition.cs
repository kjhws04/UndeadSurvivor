using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager._instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 40);
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);
                break;
            case "Enemy": //플레이어의 이동에 의해 Area에서 enemy가 벋어났을 경우
                if (col.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3));
                    transform.Translate(rand + dist * 2);
                }
                break;
            default:
                break;
        }
    }
}
