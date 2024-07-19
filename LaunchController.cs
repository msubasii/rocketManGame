using UnityEngine;
using DG.Tweening;

public class LaunchController : MonoBehaviour
{

    public Animator animator;
    private bool isPulling = false;
    private bool hasThrown = false;
    private Vector3 mouseStartPosition;
    private float pullAmount;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CamSwitch cameraSwitcher;
    [SerializeField] private RocketManController controller;

    private void Start()
    {
        cameraSwitcher = GameObject.Find("Main Camera").GetComponent<CamSwitch>();
    }

    void Update()
    {

         if (!hasThrown) // hasThrown bayrağını kontrol et 
         {
        if (Input.GetMouseButtonDown(0))
        {
            isPulling = true;
            mouseStartPosition = Input.mousePosition;
            animator.SetBool("pull", isPulling);
        }

        if (Input.GetMouseButton(0) && isPulling)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 difference = currentMousePosition - mouseStartPosition;

            pullAmount = Mathf.Clamp(-difference.x / Screen.width, 0f, 1f);
            animator.Play("New State 0", 0, pullAmount);
        }

        if (Input.GetMouseButtonUp(0))
            {
                isPulling = false;
                animator.Play("New State 1", 0, 1 - pullAmount);
                Throw(); // Atışı burada çağır
                hasThrown = true; // Bayrağı güncelle
            }
        }
        
    }

    public void Throw()
    {

        cameraSwitcher.transitionStarted = true;
        rb.gameObject.transform.parent = null;
        rb.isKinematic = false;

        float forwardForce = 50f * pullAmount;
        float upwardForce = 15f * pullAmount;

        rb.AddForce(new Vector3(0, upwardForce, forwardForce), ForceMode.Impulse);
        controller.StartRotation();
        
        print("fırladı");

    }




}
 