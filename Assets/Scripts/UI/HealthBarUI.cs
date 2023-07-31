using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasHealthGameObject;
    [SerializeField] private Image healthBarImage;
    
    private IHasHealth hasHealth;
    void Start()
    {
        hasHealth = hasHealthGameObject.GetComponent<IHasHealth>();
        if (hasHealth == null) {
            Debug.LogError("Game Object " + hasHealthGameObject + " does not implement IHasHealth.");
        }

        hasHealth.OnHealthChanged += HasHealth_OnHealthChanged;

        healthBarImage.fillAmount = 1f;

        Hide();
    }

    private void HasHealth_OnHealthChanged(object sender, IHasHealth.OnHealthChangedEventArgs e) {
        healthBarImage.fillAmount = e.healthNormalized;

        if (e.healthNormalized == 0f || e.healthNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
