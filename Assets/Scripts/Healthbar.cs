using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Healthbar : MonoBehaviour
{
    public Gradient healthBarGradient;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] TextMeshProUGUI healthBarText;
    [SerializeField] Image healthBarFillImage;
    public float hitEffectDecayRate = 0.003f;
    Coroutine hitEffectDecayCoroutine;
    public Color whiteish;

    [SerializeField] Slider hitEffectSlider; // secondary slider that shows how much damage you took in an instance.
    [SerializeField] Image hitEffectFillImage;


    int maxHealth = int.MinValue;
    bool isEnemy = false;
    Transform playerCameraTransform;
    Enemy_Controller enemy;


    public void Initialize(int maxHealth, bool isEnemy) // made an initialize method so we can assign max health for enemies and player.
    {
        this.isEnemy = isEnemy;
        if (isEnemy)
        {
            enemy = GetComponentInParent<Enemy_Controller>(); // get enemy
            playerCameraTransform = Camera.main.transform;    // get camera transform (for .LookAt())
        }
        this.maxHealth = maxHealth;
        UpdateHealthBar(maxHealth, maxHealth);
    }
    private void Update()
    {
        if (isEnemy) transform.LookAt(playerCameraTransform.position);
    }




    public void UpdateHealthBar(int preDamageHealth, int postDamageHealth)
    {
        if (maxHealth == int.MinValue) return;
        float percentHealthPreDamage = (float)preDamageHealth / maxHealth;
        float percentHealthPostDamage = (float)postDamageHealth / maxHealth;

        // set hit effect slider and colour to show how much of hp bar was taken that hit.
        hitEffectSlider.value = percentHealthPreDamage;
        hitEffectFillImage.color = healthBarGradient.Evaluate(percentHealthPreDamage);

        healthBarSlider.value = percentHealthPostDamage;
        healthBarFillImage.color = healthBarGradient.Evaluate(percentHealthPostDamage);
        healthBarText.text = $"{postDamageHealth}/{maxHealth}";

        if (preDamageHealth == postDamageHealth) return;

        if (hitEffectDecayCoroutine != null) StopCoroutine(hitEffectDecayCoroutine);
        StartCoroutine(DecayHitEffect(percentHealthPreDamage - percentHealthPostDamage));

    }

    IEnumerator DecayHitEffect(float differenceOfPercent)
    {
        float initialDifference = differenceOfPercent;

        while (differenceOfPercent >= 0)
        {
            yield return null;

            hitEffectSlider.value -= hitEffectDecayRate;
            differenceOfPercent -= hitEffectDecayRate;


            // get base colour from the gradient
            Color gradientColor = healthBarGradient.Evaluate(hitEffectSlider.value);

            // lerp white and the gradient color based on remaining difference
            hitEffectFillImage.color = Color.Lerp(gradientColor.Subtract(0.5f, 0.5f, 0.5f,0), whiteish, differenceOfPercent / initialDifference);
        }



    }
}
