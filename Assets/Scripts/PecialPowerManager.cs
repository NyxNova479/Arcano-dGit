using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class SpecialPowerManager : MonoBehaviour
{
    public SpecialPowerType currentPower;

    private bool powerUsed = false;

    [SerializeField] private WingsWalls wings;
    [SerializeField] private EnergyExplosion energyExplosion;
    [SerializeField] private Revive revive;

    [SerializeField] private TextMeshProUGUI powerText;
    private void Update()
    {
        if (!powerUsed && Input.GetKeyDown(KeyCode.S))
        {
            ActivatePower();
        }
    }

    public void ChooseRandomPower()
    {
        int random = Random.Range(0, System.Enum.GetValues(typeof(SpecialPowerType)).Length);
        currentPower = (SpecialPowerType)random;
        powerUsed = false;

        Debug.Log("Pouvoir actuel : " + currentPower);
        powerText.text = currentPower.ToString();
    }

    private void ActivatePower()
    {
        powerUsed = true;

        switch (currentPower)
        {
            case SpecialPowerType.WingsWalls:
                wings.Activate();
                break;

            case SpecialPowerType.EnergyExplosion:
                energyExplosion.Activate();
                break;

            case SpecialPowerType.Revive:
                revive.Activate();
                break;
        }
    }

    public void ConsumePower()
    {
        powerUsed = true;
    }
}