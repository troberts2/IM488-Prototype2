using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QTESystem : MonoBehaviour
{
    private DefaultInputActions playerInput;
    private InputAction fishing;
    public GameObject Player;

    public bool onFishableLand = false;

    public float mashDelay = .5f;
    public GameObject text;
    public GameObject FishingPromt;
    public int mashQuota = 8;
    public int mashCount = 0;

    [SerializeField]
    private TextMeshProUGUI QTEStatus;

    float mash = 5;
    bool started = false;

    private void Awake()
    {
        playerInput = new DefaultInputActions();
        fishing = playerInput.Fishing.Fish;
    }

    private void OnEnable()
    {
        fishing.Enable();
    }

    private void OnDisable()
    {
        fishing.Disable();    
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (fishing.IsPressed() && started == false && onFishableLand == true)
        {
            mashCount = 0;
            mash = 5;
            started = true;
            FishingPromt.SetActive(false);
            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.GetComponent<Dashing>().enabled = false;
        }

        if (started == true)
        {
            QTEStatus.text = "Reel in your catch by mashing the X button.";
            text.SetActive(true);
            mash -= Time.deltaTime;

            if(fishing.WasPerformedThisFrame())
            {
                mashCount++;
            }
            ///else if (fishing.)
            ///{
            ///    pressed = false;
            ///}
            if (mash <= 0 && mashCount < mashQuota)
            {
                QTEStatus.text = "Fish Fled. Press X to Try Again";
                started = false;
            }
            else if (mash <= 0 && mashCount >= mashQuota)
            {
                QTEStatus.text = "Fish Caught?";
                FindObjectOfType<LevelLoader>().LoadNextLevel();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onFishableLand = false;
        }
        if (collision.gameObject.CompareTag("Fishable"))
        {
            onFishableLand = true;
            
        }
    }
}
