using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WindmillGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windmills;
    [SerializeField] private Slider[] windmillSliders;
    [SerializeField] private GameObject colorTarget;

    private List<Button> lockButtons = new List<Button>();
    private List<WindmillDynamicSpeed> windmillScripts = new List<WindmillDynamicSpeed>();
    private bool[] locked;
    private int currentIndex = 0;

    private void Awake()
    {
        locked = new bool[windmills.Length];
        FindWindmillComponents();
    }

    private void FindWindmillComponents()
    {
        foreach (GameObject windmill in windmills)
        {
            WindmillDynamicSpeed windmillScript = windmill.GetComponentInChildren<WindmillDynamicSpeed>();
            if (windmillScript != null)
            {
                windmillScripts.Add(windmillScript);
            }
            else
            {
                Debug.LogError($"WindmillDynamicSpeed nicht gefunden auf {windmill.name}");
            }

            Button button = windmill.GetComponentInChildren<Canvas>()?.GetComponentInChildren<Button>();
            if (button != null)
            {
                lockButtons.Add(button);
                Debug.Log($"Button für {windmill.name} gefunden: {button.name}");
            }
            else
            {
                Debug.LogError($"Kein Button gefunden in {windmill.name} > Canvas");
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < lockButtons.Count; i++)
        {
            int index = i;
            lockButtons[i].onClick.AddListener(delegate { LockWindmill(index); });
            lockButtons[i].interactable = (i == currentIndex);
            Debug.Log($"Button {i} registriert - Interactable: {lockButtons[i].interactable}");
        }

        EnableCurrentWindmill();
    }

    private void EnableCurrentWindmill()
    {
        for (int i = 0; i < windmillScripts.Count; i++)
        {
            windmillScripts[i].enabled = (i == currentIndex);
        }

        Debug.Log($"Windmühle {currentIndex} aktiviert.");
    }

    private void LockWindmill(int index)
    {
        Debug.Log($"LockWindmill aufgerufen für Index {index}");

        if (index == currentIndex && !locked[index])
        {
            locked[index] = true;
            Debug.Log($"Windmühle {index} gesperrt");
            UpdateColor();
            lockButtons[index].interactable = false;

            // Nach dem Sperren Slider-Wert und Geschwindigkeit einfrieren
            if (windmillSliders[index] != null)
            {
                windmillSliders[index].interactable = false;
            }

            // Sperre die Geschwindigkeit der Windmühle
            windmillScripts[index].LockWindmillSpeed();

            // Synchronisiere die Geschwindigkeit mit dem Rotations-Skript
            if (windmillScripts[index] != null)
            {
                float currentSpeed = windmillScripts[index].GetCurrentSpeed();
                WindmillRotationConstantSpeed rotationScript = windmills[index].GetComponentInChildren<WindmillRotationConstantSpeed>();
                if (rotationScript != null)
                {
                    rotationScript.SetRotationSpeed(currentSpeed);  // Setze die Geschwindigkeit auf die aktuelle Windmühlengeschwindigkeit
                }
            }

            currentIndex++;
            Debug.Log($"Neuer currentIndex: {currentIndex}");

            if (currentIndex < lockButtons.Count)
            {
                lockButtons[currentIndex].interactable = true;
                Debug.Log($"Button {currentIndex} ist jetzt klickbar.");
            }

            EnableCurrentWindmill();
        }
    }


    private void UpdateColor()
    {
        float r = currentIndex >= 0 ? windmillScripts[0].GetNormalizedSpeed() : 0;
        float g = currentIndex >= 1 ? windmillScripts[1].GetNormalizedSpeed() : 0;
        float b = currentIndex >= 2 ? windmillScripts[2].GetNormalizedSpeed() : 0;

        Color newColor = new Color(r, g, b);
        colorTarget.GetComponent<Renderer>().material.color = newColor;
    }
}
