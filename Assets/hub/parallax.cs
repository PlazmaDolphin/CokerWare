using System;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public float baseVel = -3f;
    public SpriteRenderer sprite;
    private Vector3 initPos;
    private float size;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        size = sprite.bounds.size.x / 3.0f;
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x+(baseVel*Time.deltaTime), initPos.y, initPos.z);
        if (Math.Abs(transform.position.x-initPos.x) > size) {
            transform.position = initPos;
        }
    }
}
