using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text velocityText;
    [SerializeField] private Slider throttleSlider;

    void Update()
    {
        // Update cockpit UI
        velocityText.text = "Velocity: " + PlayerSpacecraft.instance.Velocity.ToString("0.00") + "m/s";

        throttleSlider.value = Input.GetAxis("Vertical");
    }
}
