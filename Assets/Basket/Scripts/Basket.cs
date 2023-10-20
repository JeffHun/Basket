using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] Transform minHeight;
    [SerializeField] Transform maxHeight;
    [SerializeField] float interpolFramesCount = 1000f;
    Transform temporaryHeight;
    float elaspedFrames;
    public bool isGameEnd;

    void Update()
    {
        if(!isGameEnd)
        {
            float interpol = elaspedFrames / interpolFramesCount;
            elaspedFrames = (elaspedFrames + 1) % (interpolFramesCount + 1);
            transform.position = Vector3.Lerp(minHeight.position, maxHeight.position, interpol);
            if(interpol == 1)
            {
                temporaryHeight = maxHeight;
                maxHeight = minHeight;
                minHeight = temporaryHeight;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        GameManager.Instance.IncreasePoint();
    }
}