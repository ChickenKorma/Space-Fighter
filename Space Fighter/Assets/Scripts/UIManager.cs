using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text velocityText;

    [SerializeField] private Slider throttleSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocityText.text = "Velocity: " + FlightController.instance.velocity.ToString("0.00") + "m/s";

        throttleSlider.value = Input.GetAxis("Vertical");
    }
}
