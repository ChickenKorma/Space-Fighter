using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Camera cam;

    [Header("HUD")]
    [SerializeField] private Image targetReticle;
    [SerializeField] private Text targetDistance;
    [SerializeField] private Image velocityVector;

    [Header("Dashboard")]
    [SerializeField] private Text speedText;
    [SerializeField] private Slider throttleSlider;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Slider temperatureSlider;
    [SerializeField] private Slider missileSlider;

    private float missileReloadTime;

    private void Awake()
    {
        // Create Singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;

        temperatureSlider.maxValue = PlayerSpacecraft.Instance.LaserCannon.MaxOperatingTemperature;

        missileReloadTime = PlayerSpacecraft.Instance.MissileLauncher.ShootRate;
    }

    void Update()
    {
        // Screen space HUD
        GameObject target = PlayerSpacecraft.Instance.Target;

        if (target != null)
        {
            targetReticle.gameObject.SetActive(true);

            Vector3 targetPosition = target.transform.position;

            Vector3 targetScreenPosition = cam.WorldToScreenPoint(targetPosition);
            targetReticle.rectTransform.position = targetScreenPosition;

            targetDistance.text = Vector3.Distance(cam.transform.position, targetPosition).ToString("0.0") + "m";
        }
        else
        {
            targetReticle.gameObject.SetActive(false);
        }

        float speed = PlayerSpacecraft.Instance.Speed;

        if(speed >= 0)
        {
            velocityVector.enabled = true;

            Vector3 relativeVelocityPosition = PlayerSpacecraft.Instance.transform.position + PlayerSpacecraft.Instance.Velocity.normalized;
            Vector3 velocityScreenPosition = cam.WorldToScreenPoint(relativeVelocityPosition);
            velocityVector.rectTransform.position = velocityScreenPosition;
        }
        else
        {
            velocityVector.enabled = false;
        }

        // World space dashboard
        speedText.text = "Speed: " + speed.ToString("0.00") + "m/s";
        throttleSlider.value = PlayerSpacecraft.Instance.Throttle;

        float temperature = PlayerSpacecraft.Instance.LaserCannon.Temperature;
        temperatureText.text = "Lasers: " + temperature.ToString("0.00") + "°C";
        temperatureSlider.value = temperature;

        missileSlider.value = Mathf.Clamp((Time.time - PlayerSpacecraft.Instance.MissileLauncher.LastShot) / missileReloadTime, 0, 1);
    }
}
