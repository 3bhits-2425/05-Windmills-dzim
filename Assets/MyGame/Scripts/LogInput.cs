using UnityEngine;
using TMPro;

public class LogInput : MonoBehaviour
{
    [SerializeField] private TMP_Text keyInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyInput.text = "Space pressed";
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            keyInput.text = "";
        }
    }
}
