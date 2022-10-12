using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextAnimation : MonoBehaviour
{
    [SerializeField]
    public TMP_Text score;
    public float destroyTime = 2.0f;
    public float scaleIntensity = 0.01f;
    Vector3 randomizeIntensity = new Vector3(0.3f, 0, 0);
    Vector3 offset = new Vector3(0, 1, 0);
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        this.transform.localScale += Vector3.one * scaleIntensity;
    }
    public void setRandomizedPosition()
    {
        transform.position += offset;
        transform.position += new Vector3(Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
        Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
        Random.Range(-randomizeIntensity.z, randomizeIntensity.z));
    }
}
