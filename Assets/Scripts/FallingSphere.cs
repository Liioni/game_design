using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSphere : MonoBehaviour
{
    private float timer;
    private float gravity = 10f;
    private float fallingVelocity = 0f;
    private Vector3 targetScale;
    private float time;
    private Vector3 initialScale;

    private enum AnimationPhase
    {
        Grow,
        Fall,
        Shrink,
        Spread,
        Stay,
        Disappear
    }

    private AnimationPhase phase;
    public GameObject splashEffect;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        targetScale = new Vector3(1f, 1f, 1f);
        phase = AnimationPhase.Grow;
        time = 2f;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLoop();
    }

    public void Fall() {
        fallingVelocity -= gravity * Time.deltaTime;
        transform.Translate(Vector3.up * fallingVelocity * Time.deltaTime);
    }

    private float QuadraticEaseOut(float t)
    {
        return 1f - Mathf.Pow(1f - t, 2f);
    }

    private float QuadraticEaseIn(float t)
    {
        return t * t;
    }

    public void Resize(Vector3 targetScale, float time, bool grow) {
        float t = Mathf.Clamp01(timer / time);
        if (grow) {
            t = QuadraticEaseOut(t);
        }
        else {
            t = QuadraticEaseIn(t);
        }
        transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        timer += Time.deltaTime;
    }

    public void AnimationLoop() {
        switch (phase)
        {
            case AnimationPhase.Grow:
                time = 3f;
                Resize(targetScale, time, true);
                if (timer >= time)
                {
                    timer = 0f;
                    initialScale = transform.localScale;
                    phase++;
                }
                break;
            case AnimationPhase.Fall:
                Fall();
                if (transform.position.y <= 0.1f)
                {
                    Instantiate(splashEffect, transform.position, Quaternion.identity);
                    SoundManager.Instance.PlaySFX("Sphere Explosion");
                    phase++;
                }
                break;
            case AnimationPhase.Shrink:
                time = 0.1f;
                Resize(new Vector3(0.1f, 0.1f, 0.1f), time, false);
                if (timer >= time)
                {
                    timer = 0f;
                    initialScale = transform.localScale;
                    phase++;
                }
                break;
            case AnimationPhase.Spread:
                time = 1f;
                Resize(new Vector3(5f, 0.1f, 5f), time, true);
                if (timer >= time)
                {
                    timer = 0f;
                    initialScale = transform.localScale;
                    phase++;
                }
                break;
            case AnimationPhase.Stay:
                time = 2f;
                timer += Time.deltaTime;
                if (timer >= time)
                {
                    timer = 0f;
                    phase++;
                }
                break;
            case AnimationPhase.Disappear:
                time = 2f;
                Resize(new Vector3(0f, 0f, 0f), time, false);
                if (timer >= time) {
                    Destroy(gameObject);
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
