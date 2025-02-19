using UnityEngine;

public class WindmillRotationConstantSpeed : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Speed of rotation

    private void Update()
    {
        // Die Rotation läuft immer weiter mit der Geschwindigkeit von rotationSpeed
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    // Methode zum Setzen der Rotationsgeschwindigkeit, die vom WindmillDynamicSpeed-Skript aufgerufen wird
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed; // Setze die Rotationsgeschwindigkeit auf den Wert, der festgelegt wurde
    }
}
