using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Transform playerCam;

    [SerializeField] private float speed = 5f;
    private Vector3 velocity;
    public float gravity = -9.81f;
    
    private float cameraPitch = 0f;
    public float mouseSensitivity = 2f;

    public bool husKey = false;

    [SerializeField] private TextMeshProUGUI messageText;
    private float messageTimer = 0f;

    void Start()
    {
        Camera playerCam = GetComponentInChildren<Camera>();
        if (playerCam != null)
        {
            this.playerCam = playerCam.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        if(playerCam != null)
        {
            playerCam.localEulerAngles = Vector3.right * cameraPitch;
        }

        float x = 0f;
        float z = 0f;

        if(Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x = 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) z = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) z = -1f;
        }

        Vector3 moveDir = transform.right * x + transform.forward * z;
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            husKey = true;
            ShowMessage("鍵を入手した");
            Destroy(other.gameObject);
        }

        if(other.CompareTag("Goal"))
        {
            if(husKey)
            {
                //ShowMessage("クリア！");
                SceneManager.LoadScene("GameClear");
            }
            else
            {
                ShowMessage("鍵がかかっていて開かない");
            }
        }

        if(other.CompareTag("Enemy"))
        {
            //ShowMessage("ゲームオーバー");
            SceneManager.LoadScene("GameOver");
        }
    }

    private void ShowMessage(string message)
    {
        if(messageText != null)
        {
            messageText.text = message;
            messageTimer = 2f;
        }
    }
}
