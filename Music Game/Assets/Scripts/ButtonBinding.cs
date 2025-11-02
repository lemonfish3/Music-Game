using UnityEngine;
using UnityEngine.UI;

public class ButtonBinding : MonoBehaviour
{

    [SerializeField] private Button quit;
    [SerializeField] private Button start;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BindButton(quit, () => GameManager.instance?.Quit());
        BindButton(start, () => GameManager.instance?.LoadLevel1());
    }
    
    private void BindButton(Button button, System.Action action)
    {
        if (button == null)
        {
            Debug.LogWarning("Button reference is missing!");
            return;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => action?.Invoke());
    }

}
