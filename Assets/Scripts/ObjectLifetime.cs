using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    private float elapsedTime;
    public float life_span = 1.5f;
    public bool destroyGameObject = true;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(life_span < elapsedTime){
            if(destroyGameObject) {
                Destroy(gameObject);
            } else {
                Destroy(this);
            }
        }
    }

    public float timeLeft(){
        return life_span - elapsedTime;
    }
}
