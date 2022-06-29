using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Camera cam;

    [Header("HUD")]
    [SerializeField] private RectTransform HUDCanvas;
    [SerializeField] private Image targetReticle;
    [SerializeField] private Image proxyTargetReticle;
    [SerializeField] private Text targetDistance;
    [SerializeField] private GameObject missileDetector;
    [SerializeField] private Image velocityVector;
    [SerializeField] private Sprite velocity;
    [SerializeField] private Sprite antiVelocity;

    private float screenWidth, screenHeight;

    [Header("Dashboard")]
    [SerializeField] private Text speedText;
    [SerializeField] private Slider throttleSlider;
    [SerializeField] private Text temperatureText;
    [SerializeField] private Slider temperatureSlider;
    [SerializeField] private Slider missileSlider;
    [SerializeField] private Image weaponSelection;

    private float missileReloadTime;

    [Header("Icons")]
    [SerializeField] private Sprite laser;
    [SerializeField] private Sprite missile;

    [Header("Settings")]
    [SerializeField] private float missileDetectionDistance;
    [SerializeField] private float minMissileDetectorSize;
    [SerializeField] private float maxMissileDetectorSize;

    private List<GameObject> missileDetectors = new List<GameObject>();

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

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        missileReloadTime = PlayerSpacecraft.Instance.MissileLauncher.ShootRate;   
    }

    void Update()
    {
        // Screen space HUD
        GameObject target = PlayerSpacecraft.Instance.Target;

        if(target != null)
        {
            targetReticle.gameObject.SetActive(true);

            Vector3 targetPosition = target.transform.position;

            Vector3 targetScreenPosition = cam.WorldToScreenPoint(targetPosition);
            targetReticle.rectTransform.position = targetScreenPosition;

            targetDistance.text = Vector3.Distance(PlayerSpacecraft.Instance.transform.position, targetPosition).ToString("0.0") + "m";
        }
        else
        {
            targetReticle.gameObject.SetActive(false);
        }

        GameObject proxyTarget = PlayerSpacecraft.Instance.ProxyTarget;

        if (proxyTarget != null && proxyTarget != target)
        {
            proxyTargetReticle.enabled = true;

            Vector3 proxyTargetScreenPosition = cam.WorldToScreenPoint(proxyTarget.transform.position);
            proxyTargetReticle.rectTransform.position = proxyTargetScreenPosition;
        }
        else
        {
            proxyTargetReticle.enabled = false;
        }

        foreach(GameObject detector in missileDetectors)
        {
            Destroy(detector);
        }

        missileDetectors = new List<GameObject>();

        foreach(GameObject missile in GameManager.Instance.Missiles)
        {
            float distance = Vector3.Distance(missile.transform.position, PlayerSpacecraft.Instance.transform.position);

            if (distance <= missileDetectionDistance)
            {
                float size = maxMissileDetectorSize * (1 - (distance / missileDetectionDistance));
                size = Mathf.Clamp(size, minMissileDetectorSize, maxMissileDetectorSize);

                Vector3 missileScreenPosition = cam.WorldToScreenPoint(missile.transform.position);
                missileScreenPosition.x = Mathf.Clamp(missileScreenPosition.x, size / 2, screenWidth - (size / 2));
                missileScreenPosition.y = Mathf.Clamp(missileScreenPosition.y, size / 2, screenHeight - (size / 2));

                GameObject detector = Instantiate(missileDetector, HUDCanvas);

                RectTransform detectorRect = detector.GetComponent<RectTransform>();
                detectorRect.sizeDelta = new Vector2(size, size);
                detectorRect.position = missileScreenPosition;

                detector.GetComponentInChildren<Text>().text = distance.ToString("0.0") + "m";

                missileDetectors.Add(detector);
            }
        }

        float speed = PlayerSpacecraft.Instance.Speed;

        Vector3 relativeVelocityPosition;

        if(speed >= 0)
        {
            velocityVector.sprite = velocity;

            relativeVelocityPosition = PlayerSpacecraft.Instance.transform.position + PlayerSpacecraft.Instance.Velocity.normalized;
        }
        else
        {
            velocityVector.sprite = antiVelocity;

            relativeVelocityPosition = PlayerSpacecraft.Instance.transform.position - PlayerSpacecraft.Instance.Velocity.normalized;
        }

        Vector3 velocityScreenPosition = cam.WorldToScreenPoint(relativeVelocityPosition);
        velocityVector.rectTransform.position = velocityScreenPosition;

        // World space dashboard
        speedText.text = "Speed: " + speed.ToString("0.00") + "m/s";
        throttleSlider.value = PlayerSpacecraft.Instance.Throttle;

        float temperature = PlayerSpacecraft.Instance.LaserCannon.Temperature;
        temperatureText.text = "Lasers: " + temperature.ToString("0.00") + "°C";
        temperatureSlider.value = temperature;

        missileSlider.value = Mathf.Clamp((Time.time - PlayerSpacecraft.Instance.MissileLauncher.LastShot) / missileReloadTime, 0, 1);

        switch (PlayerSpacecraft.Instance.CurrentWeapon)
        {
            case Spacecraft.Weapon.laserCannon:
                weaponSelection.sprite = laser;
                break;

            case Spacecraft.Weapon.missileLauncher:
                weaponSelection.sprite = missile;
                break;
        }
    }
}
