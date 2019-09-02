using UnityEngine;

public class TravellerMove : MonoBehaviour
{
    public Animator animator;
    public Transform cameraTarget;
    public Transform camera0;
    public Camera currentCamera;
    public float energyRecoverySpeed;
    public EnergySystem energySystem;
    public float rotationY;
    public float sensitivityX;
    public float sensitivityY;
    public Transform travellerMesh;
    public Transform travellerRayOrigin;

    private float cameraDistance;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float deltaDirection;
    private float deltaRotationX;
    private Vector3 force;
    private bool isFollowing;
    private bool isMoving;
    private RaycastHit raycastHit;
    private Vector3 rayOriginPosition;
    private Quaternion rayOriginRotation;
    private new Rigidbody rigidbody;
    private float rotationX;
    private bool isJumpEnabled;

    private void Start()
    {
        cameraDistance = Vector3.Distance(cameraTarget.position, transform.position);
        rigidbody = GetComponent<Rigidbody>();
        rotationX = 20f;
        rotationY = 0f;
    }

    private void FixedUpdate()
    {
        //移动控制
        isMoving = false;
        deltaDirection = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            deltaDirection = 270f;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            deltaDirection = 90f;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (deltaDirection < 45f)
            {
                deltaDirection = 180f;
            }
            else if (deltaDirection < 180f)
            {
                deltaDirection = 135f;
            }
            else
            {
                deltaDirection = 225f;
            }
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (deltaDirection < 45f)
            {
                deltaDirection = 0f;
            }
            else if (deltaDirection < 180f)
            {
                deltaDirection = 45f;
            }
            else
            {
                deltaDirection = 315f;
            }
            isMoving = true;
        }
        if (isMoving)
        {
            if (isJumpEnabled)
            {
                if (animator.GetInteger("status") != 1)
                {
                    animator.SetInteger("status", 1);
                }
            }
            else if (animator.GetInteger("status") != 0)
            {
                if (animator.GetInteger("status") != 0)
                {
                    animator.SetInteger("status", 0);
                }
            }
            travellerMesh.rotation = Quaternion.Euler(0f, deltaDirection + rotationY, 0f);
            rigidbody.velocity = travellerMesh.forward + new Vector3(0f, rigidbody.velocity.y, 0f);
        }
        else
        {
            if (animator.GetInteger("status") != 0)
            {
                animator.SetInteger("status", 0);
            }
            rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, 0f);
        }
        travellerMesh.position = transform.position;
        //镜头跟踪
        isFollowing = true;
        if (Physics.Raycast(new Ray(travellerRayOrigin.position, cameraTarget.position - transform.position), out raycastHit))
        {
            if (cameraDistance > Vector3.Distance(raycastHit.point, transform.position))
            {
                isFollowing = false;
            }
        }
        if (isFollowing)
        {
            camera0.position = cameraTarget.position;
            camera0.rotation = cameraTarget.rotation;
        }
        else
        {
            camera0.position = raycastHit.point;
            camera0.rotation = cameraTarget.rotation;
        }
    }

    private void Update()
    {
        //跳跃控制
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumpEnabled)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 5f, rigidbody.velocity.z);
            }
        }
        //镜头控制
        deltaRotationX = -Input.GetAxis("Mouse Y") * sensitivityY;
        if (deltaRotationX + rotationX > 80f)
        {
            deltaRotationX = 80f - rotationX;
            rotationX = 80f;
        }
        else if (deltaRotationX + rotationX < 10f)
        {
            deltaRotationX = 10 - rotationX;
            rotationX = 10f;
        }
        else
        {
            rotationX += deltaRotationX;
        }
        rotationY += Input.GetAxis("Mouse X") * sensitivityX;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        cameraTarget.RotateAround(transform.position, transform.right, deltaRotationX);
        //恢复能量
        if (energySystem.currentEnergy < energySystem.maxEnergy)
        {
            energySystem.currentEnergy += energyRecoverySpeed * Time.deltaTime;
            if (energySystem.currentEnergy > energySystem.maxEnergy)
            {
                energySystem.currentEnergy = energySystem.maxEnergy;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.name == "Floor")
        {
            if (!isJumpEnabled)
            {
                isJumpEnabled = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "Floor")
        {
            isJumpEnabled = false;
        }
    }
}
