using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleLook : MonoBehaviour
    {
        //private Transform m_transform;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Transform[] targets;
        private enum Mode
        {
            Button,
            Input,
            Pointer,
        }
        [SerializeField] private Mode mode = Mode.Button;

        private void Update()
        {
            switch (mode)
            {
                case Mode.Button:
                    LookButton();
                    break;
                case Mode.Input:
                    LookInput();
                    break;
                case Mode.Pointer:
                    LookPointer();
                    break;
                default:
                    break;
            }
        }

        private void FixedUpdate()
        {
            switch (mode)
            {
                case Mode.Button:
                    if (look == 0) return;
                    target.rotation *= Quaternion.Euler(0.0f, keySpeed * Time.deltaTime * look, 0.0f);
                    break;
                case Mode.Input:
                case Mode.Pointer:
                    if (lastRot == rot) return;
                    target.rotation = Quaternion.Euler(0.0f, rot, 0.0f);
                    lastRot = rot;
                    break;
                default:
                    break;
            }
        }

        private void UpdateRotation(Quaternion rotation)
        {
            foreach (Transform target in targets)
            {
                target.rotation = rotation;
            }
        }

        #region Methods
        private const KeyCode turnLeft = KeyCode.Q;
        private const KeyCode turnRight = KeyCode.E;
        private float look;
        private bool[] isPressed = new bool[2];
        [SerializeField] private float keySpeed = 10f;
        private void LookButton()
        {
            if (ButtonDown(turnLeft))
            {
                look = -1;
                isPressed[0] = true;
            }
            if (ButtonDown(turnRight))
            {
                look = 1;
                isPressed[1] = true;
            }

            if (ButtonUp(turnLeft))
            {
                isPressed[0] = false;
                if (isPressed[1] == false)
                {
                    look = 0;
                }
            }
            if (ButtonUp(turnRight))
            {
                isPressed[1] = false;
                if (isPressed[0] == false)
                {
                    look = 0;
                }
            }
        }
        private bool ButtonDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }
        private bool ButtonUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private Vector2 input;
        private Vector2 smoothInput;
        private Vector2 velocityInput;
        [SerializeField] private float smoothTime = 0.3f;
        private void LookInput()
        {
            input.x = Axis(HORIZONTAL);
            input.y = Axis(VERTICAL);
            smoothInput = Vector2.SmoothDamp(smoothInput, input, ref velocityInput, smoothTime);
            if (input == Vector2.zero) return;
            rot = Mathf.Atan2(smoothInput.x, smoothInput.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

        }
        private float Axis(string key)
        {
            return Input.GetAxis(key);
        }
        Vector2 mousePosition;
        private readonly Vector2 center = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 viewport;
        [SerializeField] private Vector2 point;
        private float rot;
        private float lastRot;
        private void LookPointer()
        {
            mousePosition = Input.mousePosition;
            viewport = cam.ScreenToViewportPoint(mousePosition);
            point = viewport - center;
            rot = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
        }
        #endregion
    }
}
