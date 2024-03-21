using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dod
{
    public class SimpleRigidPlayer : MonoBehaviour
    {
        private Transform m_transform;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform cam;
        private Vector3 intend;
        private Vector3 camF;
        private Vector3 camR;

        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        [SerializeField] private bool manualInput;
        private Vector2 input;
        private Vector2 currentInput;
        private Vector2 inputVelocity;

        [SerializeField] private float speed = 1f;
        [SerializeField] private float smoothTime = 0.3f;
        private readonly Vector3 gravity = Physics.gravity;
        [SerializeField] private LayerMask groundLayer;

        private void Awake()
        {
            m_transform = transform;
            cols = new Collider[3];
        }

        private void Start()
        {
            UpdateCamera();
        }
        private void UpdateCamera(Camera camera = null)
        {
            if (cam != null && cam == camera) return;
            if (camera == null)
            {
                cam = Camera.main.transform;
            }
            else
            {
                cam = camera.transform;
            }
        }

        private void Update()
        {
            if (manualInput)
            {
                input.x = Axis(HORIZONTAL);
                input.y = Axis(VERTICAL);
                input.Normalize();
            }
            currentInput = Vector2.SmoothDamp(currentInput, input, ref inputVelocity, smoothTime);
        }

        private void FixedUpdate()
        {
            GroundCheck();
            Move();
        }

        public Vector2 GetInput()
        {
            return input;
        }

        public void SetInput(Vector2 input)
        {
            this.input = input;
        }

        private void Move()
        {
            if (isGround == false) return;
            if (input == Vector2.zero) return;
            camF = cam.forward;
            camR = cam.right;
            camF.y = 0;
            camR.y = 0;
            camF.Normalize();
            camR.Normalize();
            intend = (camF * currentInput.y) + (camR * currentInput.x);
            rb.velocity = speed * intend;
        }
        private Collider[] cols;
        private int detect;
        private bool isGround;
        private const float radius = 0.2f;
        private void GroundCheck()
        {
            detect = Physics.OverlapSphereNonAlloc(m_transform.position, radius, cols, groundLayer);
            isGround = detect > 0;
            if(isGround)
            {

            }
            else
            {
                rb.AddForce(gravity);
            }
        }

        private float Axis(string key)
        {
            return Input.GetAxis(key);
        }
    }
}
