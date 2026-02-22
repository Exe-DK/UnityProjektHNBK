using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [SerializeField] public Slider sensitivityXSlider;
    [SerializeField] public Slider sensitivityYSlider;
    //[SerializeField] public Text sensitivityXValueText;
    //[SerializeField] public Text sensitivityYValueText;
    [SerializeField] public MouseLook mouseLook;


    // Start is called before the first frame update
    private void Start()
    {
        // Lade die gespeicherten Sensitivity-Werte aus den PlayerPrefs
        float savedSensitivityX = PlayerPrefs.GetFloat("SensitivityX", 1f);
        float savedSensitivityY = PlayerPrefs.GetFloat("SensitivityY", 1f);

        // Slider mit den aktuellen Werten aus dem Mouselook-Skript initialisieren
        sensitivityXSlider.value = savedSensitivityX;
        sensitivityYSlider.value = savedSensitivityY;
        mouseLook.SensitivityX = savedSensitivityX;
        mouseLook.SensitivityY = savedSensitivityY;

        // Initialisiere die Textwerte, !Slider verlieren dadurch die Funktion ;/!
        //sensitivityXValueText.text = sensitivityXSlider.value.ToString("F2");
        //sensitivityYValueText.text = sensitivityYSlider.value.ToString("F2");

        //Füge die Listener hinzu, um die Änderungen der Slider zu überwachen
        sensitivityXSlider.onValueChanged.AddListener(OnSensitivityXChanged);
        sensitivityYSlider.onValueChanged.AddListener(OnSensitivityYChanged);
    }

    //Mit den Slidern die Mausempfindlichkeit ändern
    public void OnSensitivityXChanged(float value)
    {
        mouseLook.SensitivityX = value;
        PlayerPrefs.SetFloat("SensitivityX", value); //Speichert die SensitivityX in den PlayerPrefs
        PlayerPrefs.Save(); //Speichert die PlayerPrefs dauerhaft
        //sensitivityXValueText.text = value.ToString("F2");
    }

    public void OnSensitivityYChanged(float value)
    {
        mouseLook.SensitivityY = value;
        PlayerPrefs.SetFloat("SensitivityY", value); //Speichert die SensitivityX in den PlayerPrefs
        PlayerPrefs.Save(); //Speichert die PlayerPrefs dauerhaft
        //sensitivityYValueText.text = value.ToString("F2");
    }
}
