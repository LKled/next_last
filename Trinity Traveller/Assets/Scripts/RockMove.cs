using UnityEngine;

public class RockMove : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform camera0;
    public Camera currentCamera;
    public float energyConsumptionSpeed;
    public float energyRecoverySpeed;
    public EnergySystem energySystem;
    public bool isRushing;
    public float maxVelocity;
    public Transform rockRayOrigin;
    public float rotationY;
    public float sensitivityX;
    public float sensitivityY;

    private float cameraDistance;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private float currentVelocity;
    private float deltaRotationX;
    private Vector3 force;
    private bool isFollowing;
    private RaycastHit raycastHit;
    private Vector3 rayOriginPosition;
    private Quaternion rayOriginRotation;
    private new Rigidbody rigidbody;
    private float rotationX;

    private void Start()
    {
        cameraDistance = Vector3.Distance(cameraTarget.position, transform.position);
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
        rigidbody.AddForce(force.normalized * 20f);
        //速度修正
        currentVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).magnitude;
        if (isRushing)
        {
            if (currentVelocity > maxVelocity * 2f)
            {
                currentVelocity = maxVelocity * 2f;
                rigidbody.velocity = currentVelocity * new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).normalized +
                    new Vector3(0f, rigidbody.velocity.y, 0f);
            }
        }
        else
        {
            if (maxVelocity < new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).magnitude)
            {
                currentVelocity = maxVelocity;
                rigidbody.velocity = currentVelocity * new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z).normalized +
                    new Vector3(0f, rigidbody.velocity.y, 0f);
            }
        }
        //镜头跟踪
        isFollowing = true;
        if (Physics.Raycast(new Ray(rockRayOrigin.position, cameraTarget.position - transform.position), out raycastHit))
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRushing = true;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (isRushing)
            {
                energySystem.currentEnergy -= energyConsumptionSpeed * Time.deltaTime;
                if (energySystem.currentEnergy < 0f)
                {
                    energySystem.currentEnergy = 0f;
                    isRushing = false;
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isRushing)
            {
                isRushing = false;
            }
        }
        //恢复能量
        if (!isRushing)
        {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (isRushing)
        {
            if (collision.collider.name == "Breakable")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
