using UnityEngine;
using UnityEngine.UI;

public class SimulationSetupUI : MonoBehaviour
{
    [Header("UI References")]
    public Text outputText;

    public InputField seedInput;
    public InputField yearsInput;
    public Dropdown endTypeDropdown;
    public Button simulateButton;

    private void Start()
    {
        if (simulateButton != null)
            simulateButton.onClick.AddListener(OnSimulateClicked);

        if (endTypeDropdown != null)
        {
            endTypeDropdown.ClearOptions();
            endTypeDropdown.AddOptions(new System.Collections.Generic.List<string> {
                "Big Freeze", "Big Crunch", "Heat Death", "Looped"
            });
        }
    }

    private void OnSimulateClicked()
    {
        int seed = 42;
        int years = 1000;

        int.TryParse(seedInput.text, out seed);
        int.TryParse(yearsInput.text, out years);

        SimulationManager sim = SimulationManager.Instance;
        if (sim != null)
        {
            sim.RootSeed = seed;
            sim.UniverseYears = years;

            // Optionally log end type if you hook it into simulator later
            string endType = endTypeDropdown.options[endTypeDropdown.value].text;
            //Debug.Log("Selected End Type: " + endType);

            sim.RunSimulation();

            if (outputText != null)
            {
                //outputText.text = "Simulation complete.\\n"
                //    + $"Seed: {seed}\\nYears: {years}\\nEnd Type: {endType}";
                outputText.text = sim.GetSimulationSummary();
            }

            
        }
    }


}
