using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}