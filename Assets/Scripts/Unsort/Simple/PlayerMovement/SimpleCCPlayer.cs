using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleCCPlayer : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cam;
        private Vector3 intend;
        private Vector3 camF;
        private Vector3 camR;

        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        private Vector2 input;
        private Vector2 currentInput;
        private Vector2 inputVelocity;

        [SerializeField] private float speed = 1f;
        [SerializeField] private float smoothTime = 0.3f;

        public float currentSpeed;

        private void Update()
        {
            input.x = Axis(HORIZONTAL);
            input.y = Axis(VERTICAL);
            input.Normalize();
            currentInput = Vector2.SmoothDamp(currentInput, input, ref inputVelocity, smoothTime);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (input == Vector2.zero) return;
            camF = cam.forward;
            camR = cam.right;
            camF.y = 0;
            camR.y = 0;
            camF.Normalize();
            camR.Normalize();
            intend = (camF * currentInput.y) + (camR * currentInput.x);
            controller.Move(speed * Time.fixedDeltaTime * intend);

            currentSpeed = controller.velocity.magnitude;
        }

        private float Axis(string key)
        {
            return Input.GetAxis(key);
        }
    }
}
