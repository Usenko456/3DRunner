using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // Для роботи з UI


public class CollectibleManager : MonoBehaviour
{
    // Швидкість обертання монетки
    public float rotationSpeed = 50f;

    // Посилання на UI Text, який показує кількість монеток
    // Вставити через інспектор або знайти за шляхом в Start()
    public TextMeshPro coinText;
    // Лічильник зібраних монеток (статична, щоб зберігати загальне значення)
    public static int coinsCollected = 0;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coinsCollected++;
            UpdateCoinText();
            Destroy(gameObject);
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
