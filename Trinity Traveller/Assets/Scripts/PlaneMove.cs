using UnityEngine;

public class PlaneMove : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform camera0;
    public Camera currentCamera;
    public float energyConsumptionSpeed;
    public float energyRecoverySpeed;
    public EnergySystem energySystem;
    public float maxVelocity;
    public Transform planeRayOrigin;
    public Transform planeRotatePivot;
    public float rotationY;
    public float sensitivityX;
    public float sensitivityY;

    private Vector3 cameraOffset;
    private Vector3 gravity;
    private bool isFlying;
    private bool isMoveEnabled;
    private bool isRecovering;
    private Quaternion planeRotation;
    private RaycastHit raycastHit;
    private new Rigidbody rigidbody;
    private float rotationX;

    private void Start()
    {
        cameraOffset = new Vector3(0f, 2.5f, -2f);
        gravity = new Vector3(0f, -6f, 0f); //初始化重力
        isMoveEnabled = false;
        isRecovering = true;
        planeRotation = Quaternion.Euler(0f, 0f, 180f);
        rigidbody = GetComponent<Rigidbody>();
        rotationX = 0f;
    }

    private void FixedUpdate()
    {
        if (isRecovering)
        {
            //能量恢复
            if (energySystem.currentEnergy < energySystem.maxEnergy)
            {
                energySystem.currentEnergy += energyRecoverySpeed * Time.deltaTime;
                if (energySystem.currentEnergy > energySystem.maxEnergy)
                {
                    energySystem.currentEnergy = energySystem.maxEnergy;
                }
            }
            //向下加速
            if (isMoveEnabled)
            {
                rotationX += Time.deltaTime * 80f;
                if (rotationX > 40f)
                {
                    rotationX = 40f;
                }
                rigidbody.AddForce(gravity);
            }
        }
        else
        {
            //向前加速
            rigidbody.AddForce(transform.forward.normalized * 6f);
        }
        //模型回转
        planeRotatePivot.localRotation = Quaternion.Lerp(planeRotatePivot.localRotation, planeRotation, Time.deltaTime * 2f);
        //归纳方向
        if (isMoveEnabled)
        {
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
        //速度修正
        if (maxVelocity < rigidbody.velocity.magnitude)
        {
            rigidbody.velocity = maxVelocity * rigidbody.velocity.normalized;
        }
        //镜头远近
        if (transform.position.y < 5f)
        {
            cameraTarget.localPosition = cameraOffset;
        }
        else
        {
            cameraTarget.localPosition = cameraOffset * ((transform.position.y - 5f) * 0.1f + 1f);
        }
        //镜头跟踪
        camera0.position = Vector3.Lerp(camera0.position, cameraTarget.position, Time.deltaTime * 4f);
        camera0.rotation = Quaternion.Lerp(camera0.rotation, cameraTarget.rotation, Time.deltaTime * 4f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //移动启用
            isMoveEnabled = true;
        }
        if (isMoveEnabled)
        {
            //移动控制
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isFlying = true;
                isRecovering = false;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                if (isFlying)
                {
                    if (energySystem.currentEnergy > 0f)
                    {
                        //方向控制
                        rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
                        if (rotationX > 40f)
                        {
                            rotationX = 40f;
                        }
                        else if (rotationX < -80f)
                        {
                            rotationX = -80f;
                        }
                        rotationY += Input.GetAxis("Mouse X") * sensitivityX;
                        //模型自转
                        planeRotatePivot.Rotate(0f, 0f, -Input.GetAxis("Mouse X") * 0.6f);
                        //能量消耗
                        energySystem.currentEnergy -= energyConsumptionSpeed * Time.deltaTime;
                        if (energySystem.currentEnergy < 0f)
                        {
                            energySystem.currentEnergy = 0f;
                            isFlying = false;
                            isRecovering = true;
                        }
                    }
                    else
                    {
                        if (!isRecovering)
                        {
                            isRecovering = true;
                        }
                    }
                }
            }
            else
            {
                if (!isRecovering)
                {
                    isRecovering = true;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //移动禁用
        if (collision.collider.name == "Floor")
        {
            isMoveEnabled = false;
            isRecovering = true;
            planeRotatePivot.localRotation = planeRotation;
            rigidbody.velocity = new Vector3();
            rotationX = 0f;
            transform.position = new Vector3(transform.position.x, collision.transform.position.y +
                collision.transform.localScale.y * 0.5f + 0.5f, transform.position.z);
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}
