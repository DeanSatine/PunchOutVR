using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Healthbar : MonoBehaviour
{
    public Gradient healthBarGradient;
    Slider healthBarSlider;
    [SerializeField] TextMeshProUGUI healthBarText;
    [SerializeField] Image healthBarFillImage;
    int maxHealth = int.MinValue;

    private void Start()
    {
        healthBarSlider = GetComponent<Slider>();   
    }
    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealthBar(Player.instance.currentHealth);
    }
    private void Update()
    {
        UpdateHealthBar(Player.instance.currentHealth);
    }
    public void UpdateHealthBar(int currentHealth)
    {
        if (maxHealth == int.MinValue) return;
        float percentHealth = (float)currentHealth / maxHealth;
        healthBarSlider.value = percentHealth ;
        healthBarFillImage.color = healthBarGradient.Evaluate(percentHealth);
        healthBarText.text = $"{currentHealth}/{maxHealth}";

    }

}
