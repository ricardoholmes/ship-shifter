using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalCanvas : MonoBehaviour
{
    private Vector3 offset;
    private Transform target;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        offset = transform.position - transform.parent.position;

        //Vector2 originalSize = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        target = transform.parent;
        transform.SetParent(null);
        //transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = originalSize;
    }

    private void Update()
    {
        if (target == null || !target.GetComponent<SpriteRenderer>().enabled)
        {
            Destroy(gameObject);
        }
        transform.position = target.position + offset;
    }
}
