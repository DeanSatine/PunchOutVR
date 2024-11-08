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
    bool isPlayer = false;

    private void Start()
    {
        healthBarSlider = GetComponent<Slider>();   
    }
    public void Initialize(int maxHealth, bool isPlayer) // made an initialize method so we can assign max health for enemies and player.
    {
        this.isPlayer = isPlayer;
        this.maxHealth = maxHealth;
        UpdateHealthBar(maxHealth);
    }
    private void Update()
    {
        if (isPlayer) transform.LookAt(Player.instance.transform.position);
        UpdateHealthBar(Player.instance.currentHealth);// DEBUGGING.
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
