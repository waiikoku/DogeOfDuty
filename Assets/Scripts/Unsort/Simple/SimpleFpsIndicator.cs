using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dod
{
    public class SimpleFpsIndicator : MonoBehaviour
    {
        private int fps;
        private int count;
        private float timer;
        private const float t = 1f;

        [SerializeField] private TextMeshProUGUI display;

        public int deltaFps;
        private void Update()
        {
            count++;
            timer += Time.deltaTime;
            deltaFps = (int)Math.Round(1f / Time.unscaledDeltaTime);
            if (t < timer)
            {
                timer = 0;
                fps = count;
                count = 0;
            }
        }

        private void LateUpdate()
        {
            display.text = deltaFps.ToString();
        }
    }
}
