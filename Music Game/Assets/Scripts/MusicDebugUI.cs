using UnityEngine;
using TMPro;

public class MusicDebugUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI debugText;

    [Header("Update Rate")]
    public float updateInterval = 0.1f;
    private float timeSinceLastUpdate = 0f;


    // Update is called once per frame
    void Update()
    {
        // Check if it's time to update the debug info
        if (Time.time >= timeSinceLastUpdate + updateInterval)
        {
            UpdateDebugInfo();
            timeSinceLastUpdate = Time.time;
        }
    }

    void UpdateDebugInfo()
    {
        MusicManager mm = MusicManager.instance;
        if (mm == null || debugText == null) return;
        
        debugText.text = $"BPM: {mm.BPM}\n" +
                        $"Song Time: {mm.songPosition:F3} sec\n" +
                        $"Beats: {mm.songPositionInBeats:F2}\n" +
                        $"Playing: {mm.isPlaying}\n" +
                        $"Offset: {mm.offsetTime:F3} sec";
    }   
}
