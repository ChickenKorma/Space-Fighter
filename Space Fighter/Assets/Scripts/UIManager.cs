using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Private variables set in inspector
    [SerializeField] private Text velocityText;

    [SerializeField] private Slider throttleSlider;

    [SerializeField] private Image crossHairs;

    void Update()
    {
        // Update cockpit UI
        velocityText.text = "Velocity: " + FlightController.instance.velocity.ToString("0.00") + "m/s";

        throttleSlider.value = Input.GetAxis("Vertical");

        // Update screen space UI
        crossHairs.rectTransform.anchoredPosition = new Vector2(FlightController.instance.mouseRelative.x, FlightController.instance.mouseRelative.y);
    }
}
