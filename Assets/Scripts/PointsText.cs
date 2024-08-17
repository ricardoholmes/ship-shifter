using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// text that shows when gaining points
public class PointsText : MonoBehaviour
{
    public float timeVisible = 0.5f;
    public float riseSpeed = 3;
    private float timeShown = 0;

    private TextMeshProUGUI text;
    private Color color;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        color = text.color;
    }

    private void Update()
    {
        timeShown += Time.deltaTime;
        if (timeShown >= timeVisible)
        {
            Destroy(gameObject);
            return;
        }

        // percent complete
        float p = timeShown / timeVisible;
        color.a = 1 - p;

        transform.position += riseSpeed * Time.deltaTime * Vector3.up;
    }
}
