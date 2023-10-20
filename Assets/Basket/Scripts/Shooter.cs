using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float interpolFramesCount = 25f;
    [SerializeField] Transform minHeight;
    [SerializeField] Transform maxHeight;
    [SerializeField] Transform handPos;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject basket;
    [SerializeField] Animator anim;
    [SerializeField] Vector2 strenghtRange;
    bool isShooting;
    string currentAnimation = null;
    float elaspedFrames;
    ShootingPhase phase;

    public void SetIsShooting(bool state)
    {
        isShooting = state;
        phase = ShootingPhase.prejump;
    }

    void Update()
    {
        if (isShooting)
        {
            float interpol = elaspedFrames / interpolFramesCount;
            elaspedFrames = (elaspedFrames + 1) % (interpolFramesCount + 1);
            if(phase == ShootingPhase.fall)
            {
                PlayThisAnim("Fall");
                transform.position = Vector3.Lerp(maxHeight.position, minHeight.position, interpol);
                if(interpol == 1)
                {
                    isShooting = false;
                }
            }
            if(phase == ShootingPhase.jump)
            {
                PlayThisAnim("Jump");
                transform.position = Vector3.Lerp(minHeight.position, maxHeight.position, interpol);
                if (interpol == 1)
                {
                    phase = ShootingPhase.fall;
                    Shoot();
                }
            }
            if (phase == ShootingPhase.prejump)
            {
                PlayThisAnim("Prejump");
                if(interpol == 1)
                {
                    phase = ShootingPhase.jump;
                }
            }
        }
        else
        {
            phase = ShootingPhase.idle;
            PlayThisAnim("Idle");
        }
    }

    void PlayThisAnim(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            anim.Play(animation);
        }
    }

    void Shoot()
    {
        GameObject ball = Instantiate(ballPrefab, handPos.transform.position, Quaternion.identity);
        Vector3 dir = (basket.transform.position - transform.position).normalized;
        float strenght = Random.Range(strenghtRange.x, strenghtRange.y);
        ball.GetComponent<Rigidbody2D>().AddForce(dir * strenght);
    }

    enum ShootingPhase
    {
        idle,
        prejump,
        jump,
        fall
    }
}
