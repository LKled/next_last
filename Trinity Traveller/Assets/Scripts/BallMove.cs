using UnityEngine;

public class BallMove : MonoBehaviour
{
    public Transform ballRayOrigin;
    public Transform cameraTarget;
    public Transform camera0;
    public Camera currentCamera;
    public float energyConsumptionSpeed;
    public float energyRecoverySpeed;
    public EnergySystem energySystem;
    public float maxVelocity;
    public float rotationY;
    public float sensitivityX;
    public float sensitivityY;

    private float accumulation;
    private float cameraDistance;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float deltaRotationX;
    private Vector3 force;
    private bool isAccumulating;
    private bool isFollowing;
    private bool isJumpEnabled;
    private RaycastHit raycastHit;
    private Vector3 rayOriginPosition;
    private Quaternion rayOriginRotation;
    private new Rigidbody rigidbody;
    private float rotationX;

    private void Start()
    {
        cameraDistance = Vector3.Distance(cameraTarget.position, transform.position);
        isJumpEnabled = true;
        rigidbody = GetComponent<Rigidbody>();
        rotationX = 20f;
    }

    private void FixedUpdate()
    {
        //移动控制
        force = new Vector3();
        if (Input.GetKey(KeyCode.A))
        {
            force -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            force += transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            force -= transform.forward;
        }
        if (Input.GetKey(KeyCode.W))
        {
            force += transform.forward;
        }
        //归纳受力
        rigidbody.AddForce(force.normalized * 10f);
        //速度修正
        if (maxVelocity < new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).magnitude)
        {
            rigidbody.velocity = maxVelocity * new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).normalized + new
                Vector3(0f, rigidbody.velocity.y, 0f);
        }
        //镜头跟踪
        isFollowing = true;
        if (Physics.Raycast(new Ray(ballRayOrigin.position, cameraTarget.position - transform.position), out raycastHit))
        {
            if (cameraDistance > Vector3.Distance(raycastHit.point, transform.position))
            {
                isFollowing = false;
            }
        }
        if (isFollowing)
        {
            camera0.position = Vector3.Lerp(camera0.position, cameraTarget.position, Time.deltaTime * 4f);
            camera0.rotation = Quaternion.Lerp(camera0.rotation, cameraTarget.rotation, Time.deltaTime * 4f);
        }
        else
        {
            camera0.position = raycastHit.point;
            camera0.rotation = cameraTarget.rotation;
        }
    }

    private void Update()
    {
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
        //特殊能力
        if (isJumpEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                accumulation = 0f;
                currentCamera.fieldOfView = 60f;
                isAccumulating = true;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                if (isAccumulating)
                {
                    accumulation += Time.deltaTime * 2f;
                    currentCamera.fieldOfView += Time.deltaTime * 6f;
                    if (currentCamera.fieldOfView > 90f)
                    {
                        currentCamera.fieldOfView = 90f;
                    }
                    energySystem.currentEnergy -= energyConsumptionSpeed * Time.deltaTime;
                    if (energySystem.currentEnergy < 0f)
                    {
                        energySystem.currentEnergy = 0f;
                        isAccumulating = false;
                        isJumpEnabled = false;
                        rigidbody.velocity += new Vector3(0f, accumulation, 0f);
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {

                if (isAccumulating)
                {
                    isAccumulating = false;
                    isJumpEnabled = false;
                    rigidbody.velocity += new Vector3(0f, accumulation, 0f);
                }
            }
        }
        //恢复能量
        if (!isAccumulating)
        {
            currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, 60f, Time.deltaTime * 6f);
            if (energySystem.currentEnergy < energySystem.maxEnergy)
            {
                energySystem.currentEnergy += energyRecoverySpeed * Time.deltaTime;
                if (energySystem.currentEnergy > energySystem.maxEnergy)
                {
                    energySystem.currentEnergy = energySystem.maxEnergy;
                }
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
