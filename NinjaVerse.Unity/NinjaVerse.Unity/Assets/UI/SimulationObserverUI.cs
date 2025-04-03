using UnityEngine;
using UnityEngine.UI;
using NinjaVerse.Core;

public class SimulationObserverUI : MonoBehaviour
{
    [Header("UI References")]
    public InputField yearInput;
    public Button observeButton;
    public Text feedbackText;

    private ObserverContext observer;

    void Start()
    {
        observer = new ObserverContext();

        if (observeButton != null)
            observeButton.onClick.AddListener(OnObserveClicked);
    }

    void OnObserveClicked()
    {
        int year = 0;
        if (!int.TryParse(yearInput.text, out year))
        {
            feedbackText.text = "Invalid year.";
            return;
        }

        observer.ObservePath(new ObservedPath(), year);
        int galaxyEstimate = EstimateGalaxyCount(year, observer.CurrentPath);

        feedbackText.text = $"🔭 Observing Year {year}\nEstimated Galaxies: {galaxyEstimate}";
    }

    int EstimateGalaxyCount(int year, ObservedPath path)
    {
        // This is a placeholder — eventually use UniverseSimulator to estimate based on seed and year
        int baseGalaxies = 1 + (year / 1000); // crude heuristic
        return Mathf.Clamp(baseGalaxies, 1, 1000);
    }
}
