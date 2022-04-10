using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CinematicBars : MonoBehaviour
{
    [SerializeField] private float r, g, b, a;
    [SerializeField] private float target_size;
    [SerializeField] private float change_size_amount;
    private RectTransform top_bar, bottom_bar;
    private bool is_active;

    private void Awake()
    {
        GameObject go = new GameObject("Top Bar", typeof(Image));
        go.transform.SetParent(transform, false);
        go.GetComponent<Image>().color = new Color(r,g,b, a);
        top_bar = go.GetComponent<RectTransform>();
        top_bar.anchorMin = new Vector2(0, 1);
        top_bar.anchorMax = new Vector2(1, 1);
        top_bar.sizeDelta = new Vector2(0, 0);

        go = new GameObject("Bottom Bar", typeof(Image));
        go.transform.SetParent(transform, false);
        go.GetComponent<Image>().color = new Color(r, g, b, a);
        bottom_bar = go.GetComponent<RectTransform>();
        bottom_bar.anchorMin = new Vector2(0, 0);
        bottom_bar.anchorMax = new Vector2(1, 0);
        bottom_bar.sizeDelta = new Vector2(0, 0);
    }

    private void Update()
    {
        if (is_active)
        {
            Vector2 size_delta = top_bar.sizeDelta;
            size_delta.y += change_size_amount * Time.deltaTime;
            if (change_size_amount > 0)
            {
                if (size_delta.y >= target_size)
                {
                    size_delta.y = target_size;
                    is_active = false;
                }
            }
            else
            {
                if(size_delta.y <= target_size)
                {
                    size_delta.y = target_size;
                    is_active = false;
                }
            }

            top_bar.sizeDelta = size_delta;
            bottom_bar.sizeDelta = size_delta;
        }
    }

    public void Show(float target_size, float time)
    {
        this.target_size = target_size;
        change_size_amount = (target_size - top_bar.sizeDelta.y) / time;
        is_active = true;
    }

    public void Hide(float time)
    {
        target_size = 0f;
        change_size_amount = (target_size - top_bar.sizeDelta.y) / time;
    }
}
