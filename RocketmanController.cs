using System;
using UnityEngine;
using DG.Tweening;

public class RocketManController : MonoBehaviour
{
    public Animator animator;
    private bool isPulling = false;
    private float startTime;
    private float holdTime;
    private float maxHoldTime = 0.2f;
    private float normalizedOpenTime;
    private float normalizedCloseTime;
    public bool isLaunched;
    public bool hasLaunched = false;
    private Tween rotateTween;
    private bool openWingsOnce;
    private float lastMouseX;
    public float movementForce = 0.3f;

    [SerializeField] private Rigidbody rb;
    public float bounceForce = 10f; // Zıplama kuvveti
    private Vector3 normalGravity = new Vector3(0, -9.81f, 0);
    public Vector3 slowGravity = new Vector3(0, 0, 0);

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        lastMouseX = Input.mousePosition.x;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            isPulling = true;
            startTime = Time.time;
            animator.SetBool("pull", isPulling);
            lastMouseX = Input.mousePosition.x;
        }

        if (hasLaunched)
        {
            if (Input.GetMouseButton(0) && isPulling)
            {
                StopRotation();
                holdTime = Time.time - startTime;
                normalizedOpenTime = Mathf.Clamp01(holdTime / maxHoldTime);
                animator.Play("openWings", 0, normalizedOpenTime);
                Physics.gravity = slowGravity;

                float mouseX = Input.mousePosition.x;
                float deltaX = mouseX - lastMouseX;
                if (Mathf.Abs(deltaX) > 0.1f)
                {
                    float targetZRotation = 0;
                    float targetYRotation = 0;
                    Vector3 forceDirection = Vector3.zero;
                    if (deltaX > 0)
                    {
                        // Sağa dönme
                        targetYRotation = -30;
                        targetZRotation = -45;
                        forceDirection = Vector3.right;
                    }
                    else
                    {
                        // Sola dönme
                        targetYRotation = 30;
                        targetZRotation = 45;
                        forceDirection = Vector3.left;
                    }

                    rb.transform.DORotate(new Vector3(35, targetYRotation, targetZRotation), 1f, RotateMode.Fast);
                    rb.AddForce(forceDirection * movementForce, ForceMode.Impulse);
                    lastMouseX = mouseX;
                }

                if (!openWingsOnce)
                {
                    StopRotation();
                    openWingsOnce = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && isPulling)
            {
                isPulling = false;
                animator.SetBool("pull", isPulling);
                holdTime = Time.time - startTime;
                normalizedCloseTime = Mathf.Clamp01(holdTime / maxHoldTime);
                animator.Play("closeWings", 0, 1 - normalizedCloseTime);
                Physics.gravity = normalGravity;

                if (openWingsOnce)
                {
                    StartRotation();
                    openWingsOnce = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cube"))
        {
            Vector3 bounce = Vector3.up * bounceForce;
            rb.AddForce(bounce, ForceMode.Impulse);
            
        }
        else if (collision.gameObject.CompareTag("cylinder"))
        {
            Vector3 bounce = Vector3.up * bounceForce * 2;
            rb.AddForce(bounce, ForceMode.Impulse);
            
        }
    }

    public void StartRotation()
    {
        print("dönüyor");
        isLaunched = true;
        hasLaunched = true;
        if (isLaunched)
        {
            if (rotateTween != null)
            {
                rotateTween.Pause();
            }
            rotateTween = rb.gameObject.transform.DORotate(new Vector3(360, 0, 0), 0.6f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental).SetRelative();
        }
    }

    public void StopRotation()
    {
        rotateTween.Pause();
        rotateTween = transform.DORotate(new Vector3(35, 0, 0), 0.2f, RotateMode.Fast);
    }
}

