using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 


public class CollectibleManager : MonoBehaviour
{
    float rotationSpeed = 220f;
    public GameObject pickupEffect;
    private Rigidbody rb;
    AudioManager audioManager;
    public TextMeshProUGUI coinText;
    public static int coinsCollected = 0;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        coinsCollected = 0;
    }
    // Giving coin constant rotation
    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }
    //Method for audio,effect and refresh coin count text when pick up it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupEffect != null)
            {
                GameObject effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f); // delete effect after 1s
            }
            audioManager.PlaySFX(audioManager.Coincollected);
            coinsCollected++;
            UpdateCoinText();
            Destroy(gameObject);//delete coin from scene
        }
    }
    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Монет зібрано: " + coinsCollected.ToString();
        }
    }
}
