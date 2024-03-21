using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dod
{
    public class SimpleGun : MonoBehaviour
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private Rigidbody bullet;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float fireRate;
        private float fireInterval;

        [SerializeField] private bool autoFire = false;

        [Header("Light Flicker")]
        [SerializeField] private GameObject flash;
        [SerializeField] private float flashDuration;

        [Header("Pointer")]
        [SerializeField] private bool pointToCursor;
        [SerializeField] private Transform aimTarget;
        private Vector3 aimDirection;

        public Action OnFire;

        private void Start()
        {
            UpdateCamera();
            UpdateFirerate();

            if (autoFire)
            {
                StartCoroutine(Fire());
            }
        }

        private void Update()
        {
            CursorWithWorld();
        }

        private void UpdateFirerate()
        {
            fireInterval = 1f / fireRate;
        }

        private void UpdateCamera(Camera camera = null)
        {
            if (cam != null && cam == camera) return;
            if(camera == null)
            {
                cam = Camera.main;
            }
            else
            {
                cam = camera;
            }
        }

        private IEnumerator Fire()
        {
            while (true)
            {
                if(flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                Spawn();
                flash.SetActive(true);
                flashCoroutine = StartCoroutine(Flash());
                OnFire?.Invoke();
                yield return new WaitForSeconds(fireInterval);
            }
        }
        private Coroutine flashCoroutine;
        private IEnumerator Flash()
        {
            yield return new WaitForSeconds(flashDuration);
            flash.SetActive(false);
        }

        private void Spawn()
        {
            /*
            Quaternion targetRot;
            if (pointToCursor)
            {
                LookPointer();
                targetRot = newRot;
            }
            else
            {
                targetRot = muzzle.rotation;
            }
            */
            Rigidbody rb = Instantiate(bullet, muzzle.position, muzzle.rotation);
            /*
            aimDirection = aimTarget.position - muzzle.position;
            aimDirection.y = 0;
            rb.AddForce(bulletSpeed * aimDirection, ForceMode.Impulse);
            */
            rb.AddRelativeForce(Vector3.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(rb.gameObject, 3f);
        }

        [SerializeField] private Camera cam;
        Vector2 mousePosition;
        private readonly Vector2 center = new Vector2(0.5f, 0.5f);
        private Vector2 viewport;
        private Vector2 point;
        private float rot;
        private Quaternion newRot;
        private void LookPointer()
        {
            mousePosition = Input.mousePosition;
            viewport = cam.ScreenToViewportPoint(mousePosition);
            point = viewport - center;
            rot = Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            newRot = Quaternion.Euler(0.0f, rot, 0.0f);
        }
        private Vector3 dir;
        private Vector2 mouse2D;
        private Vector2 muzzle2D;
        private Vector3 CursorWithWorld()
        {
            mousePosition = Input.mousePosition;
            mouse2D = cam.ScreenToViewportPoint(mousePosition);
            mouse2D.x = Mathf.Clamp(mouse2D.x, 0, 1);
            mouse2D.y = Mathf.Clamp(mouse2D.y, 0, 1);
            muzzle2D = cam.WorldToViewportPoint(muzzle.position);
            Vector2 direction = mouse2D - muzzle2D;
            dir = new Vector3(direction.x,0,direction.y);
            return dir;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(muzzle.position, muzzle.position + dir * 5);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mouse2D,0.1f);
            Gizmos.DrawLine(mouse2D, muzzle2D);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(muzzle2D,0.1f);
        }
    }
}
