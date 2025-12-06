using UnityEngine;

public class RandomFlicker : MonoBehaviour
{
    public Light myLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    public float minTime = 0.05f;
    public float maxTime = 0.3f;

    private float timer;

    void Start()
    {
        if (myLight == null)
            myLight = GetComponent<Light>();

        timer = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            myLight.intensity = Random.Range(minIntensity, maxIntensity);

            timer = Random.Range(minTime, maxTime);
        }
    }
}
