using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Rigidbody rb;
    Renderer renderer;

    public Vector3 direction;
    public float speed;

    private float timer = 0f;
    private float destroyTime = 5f;

    private float initialAlpha;
    private bool fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        initialAlpha = renderer.material.color.a;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= destroyTime - 1f) {
            timer = 0f;
            fadeOut = true;
        }
        if (fadeOut) {
            fadeBulletOut();
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    public void fadeBulletOut() {
        float t = Mathf.Clamp01(timer / 1f);
        float newAlpha = Mathf.Lerp(initialAlpha, 0f, t);
        Color newColor = renderer.material.color;
        newColor.a = newAlpha;
        renderer.material.color = newColor;
    }
}
