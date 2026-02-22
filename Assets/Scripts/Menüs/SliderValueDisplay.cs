using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider; // Referenz auf den Slider
    [SerializeField] private Text valueText; // Referenz auf das Text-UI-Element

    private void Start()
    {
        // Initialisiere den Text mit dem aktuellen Slider-Wert
        UpdateValueText(slider.value);

        // Füge einen Listener hinzu, um Änderungen am Slider-Wert zu überwachen
        slider.onValueChanged.AddListener(UpdateValueText);
    }

    // Methode zum Aktualisieren des Textes
    private void UpdateValueText(float value)
    {
        valueText.text = value.ToString("F2"); // Formatiere den Wert mit zwei Dezimalstellen
    }
}