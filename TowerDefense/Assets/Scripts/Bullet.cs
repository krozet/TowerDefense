﻿using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 70f;
    public GameObject impactEffect;
    private Transform target;


    public void Seek(Transform _target) {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget() {
        GameObject effectInstance = (GameObject) Instantiate(impactEffect, this.transform.position, this.transform.rotation);

        Destroy(target.gameObject);
        Destroy(effectInstance, 2f);
        Destroy(gameObject);
    }
}
