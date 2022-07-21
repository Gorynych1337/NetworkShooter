using JetBrains.Annotations;
using UnityEngine;
using Photon.Pun;
public class PlayerControl : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Walk settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Look settings")]
    [SerializeField] private float lookSpeed;
    [SerializeField] private float vertLookLimit;

    [Header("Jump settings")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float voidHeight;

    [Header("Other settings")]
    [SerializeField] private float shootPower;
    
    // Input
    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private float vertLookAngle;
    
    private bool isGrounded;

    //Player objects
    [SerializeField] GameObject cameraObj;
    [SerializeField] GameObject cameraHolderObj;
    
    //Player components
    private Rigidbody rb;
    private PlayerInput input;
    private PhotonView pv;
    private GUIManager guiManager;
    private DeathHandlers ps;

    [CanBeNull] public Rigidbody RB => rb;

    void Awake()
    {
        // server components
        pv = GetComponent<PhotonView>();
        ps = GetComponent<DeathHandlers>();

        if (!pv.IsMine) {
            Destroy(cameraObj);
            Destroy(rb);
            Destroy(GetComponentInChildren<GUIManager>().gameObject);
            return;
        }

        // input ivents
        input = new PlayerInput();

        input.Player.Jump.performed += context => Jump();

        input.Player.Shoot.performed += context => Shoot();

        input.Player.TabMenu.performed += context => guiManager.ShowUI(UIs.TabMenu);
        input.Player.TabMenu.canceled += context => guiManager.ShowUI(UIs.InGame);

        input.Player.Menu.performed += context => Menu();
        input.Menu.ExitMenu.performed += context => ExitMenu();

        input.Menu.QuitApp.performed += context => Application.Quit();

        input.Menu.Respawn.performed += context => pv.RPC("RpcOnCharacterHit", RpcTarget.All, Vector3.zero);

        // player prefab's components
        rb = GetComponent<Rigidbody>();
        
        guiManager = GetComponentInChildren<GUIManager>();
        guiManager.ShowUI(UIs.InGame);
    }

    public override void OnEnable()
    {
        input?.Player.Enable();
    }
    
    public override void OnDisable()
    {
        input?.Player.Disable();
    }

    private void Update()
    {
        if (!pv.IsMine || ps.IsDead)
            return;
        
        if (transform.position.y < voidHeight)
            ps.OnHit(Vector3.down, pv.Controller);

        moveDirection = input.Player.Move.ReadValue<Vector2>();
        Move();

        lookDirection = input.Player.Look.ReadValue<Vector2>();
        Look();
    }

    // Buttons handlers
    private void Move()
    {
        Vector3 _moveDirection = new Vector3(moveDirection.x, 0, moveDirection.y);
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1);
        _moveDirection *= input.Player.Sprint.IsPressed() ? runSpeed : walkSpeed;
        _moveDirection *= Time.deltaTime;

        _moveDirection = transform.TransformDirection(_moveDirection);
        transform.position += _moveDirection;
    }

    private void Look()
    {
        transform.Rotate(0, lookDirection.x * lookSpeed, 0);
        vertLookAngle -= lookDirection.y * lookSpeed;
        CameraVerticalRotating();
    }

    private void CameraVerticalRotating()
    {
        vertLookAngle = Mathf.Clamp(vertLookAngle, -vertLookLimit, vertLookLimit);
        cameraHolderObj.transform.localEulerAngles = new Vector3(vertLookAngle, 0, 0);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower * rb.mass, ForceMode.Impulse);
        }
    }
    
    public void SetGroundState(bool state)
    {
        isGrounded = state;
    }

    private void Shoot()
    {
        Ray ray = new Ray(cameraObj.transform.position, cameraObj.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;
        
        GameObject hitObject = hit.collider.gameObject;
        
        hitObject.GetComponent<IHittable>()?.OnHit(cameraObj.transform.forward * shootPower, pv.Controller);
    }

    private void Menu()
    {
        SwitchInputToMenu();
        
        guiManager.ShowUI(UIs.EscMenu);
    }

    public void SwitchInputToMenu()
    {
        input.Player.Disable();

        input.Menu.Enable();
    }

    private void ExitMenu()
    {
        input.Menu.Disable();

        guiManager.ShowUI(UIs.InGame);

        input.Player.Enable();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(vertLookAngle);
        }
        else if (stream.IsReading)
        {
            vertLookAngle = (float)stream.ReceiveNext();
            CameraVerticalRotating();
        }
    }
}
