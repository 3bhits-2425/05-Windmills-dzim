using UnityEngine;
using UnityEngine.UI;

public class WindmillDynamicSpeed : MonoBehaviour
{
    [SerializeField] private Light lampLight;
    [SerializeField] private float maxLightIntensity = 1f;
    [SerializeField] private Slider speedSlider;
    public float maxRotationSpeed = 255f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 30f;
    private float currentSpeed = 0f;
    private bool isLocked = false;

    // Wir fügen hier eine Referenz zum WindmillRotationConstantSpeed-Skript hinzu
    [SerializeField] private WindmillRotationConstantSpeed windmillRotationScript;

    private void Update()
    {
        if (!isLocked)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                currentSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                currentSpeed -= deceleration * Time.deltaTime;
            }

            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxRotationSpeed);

            if (speedSlider != null)
            {
                speedSlider.value = Mathf.Round(currentSpeed);
            }
        }

        // Windmill keeps rotating at the current speed, even when locked
        transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);

        if (lampLight != null)
        {
            lampLight.intensity = Mathf.Lerp(0f, maxLightIntensity, currentSpeed / maxRotationSpeed);
        }

        // Wenn das Windrad gesperrt ist, setzen wir die Geschwindigkeit des Rotors
        if (isLocked && windmillRotationScript != null)
        {
            windmillRotationScript.SetRotationSpeed(currentSpeed);
        }
    }

    public float GetNormalizedSpeed()
    {
        return currentSpeed / maxRotationSpeed;
    }

    public void LockWindmillSpeed()
    {
        isLocked = true; // Sperre die Windmühle, damit keine Benutzerinteraktionen mehr möglich sind.

        // Blockiere die Möglichkeit, den Speed über den Slider zu verändern
        if (speedSlider != null)
        {
            speedSlider.interactable = false;
        }

        Debug.Log($"Windmühle {gameObject.name} gesperrt. Geschwindigkeit eingefroren bei {currentSpeed}");
    }


    public void UnlockWindmillSpeed()
    {
        isLocked = false;
        if (speedSlider != null)
        {
            speedSlider.interactable = true;
        }
        Debug.Log("Windmill speed unlocked.");
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0f, maxRotationSpeed);
        if (speedSlider != null)
        {
            speedSlider.value = Mathf.Round(currentSpeed);
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
