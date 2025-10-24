using UnityEngine;
using TMPro;

public class RatingUI : MonoBehaviour
{
    public TextMeshProUGUI ratingText;
    private string currentRating = "";

    [Header("Colors")]
    public Color perfectColor = Color.green;
    public Color goodColor = Color.yellow;
    public Color missColor = Color.red;
    public Color idleColor = Color.white;

    void Update()
    {
        if (HitNoteManager.instance == null || ratingText == null)
            return;

        string newRating = HitNoteManager.instance.currentRating;

        if (newRating != currentRating)
        {
            currentRating = newRating;
            ratingText.text = currentRating;
            ratingText.color = GetColorForRating(currentRating);
        }
    }

    Color GetColorForRating(string rating)
    {
        switch (rating)
        {
            case "Perfect": return perfectColor;
            case "Good": return goodColor;
            case "Miss": return missColor;
            default: return idleColor;
        }
    }
}
