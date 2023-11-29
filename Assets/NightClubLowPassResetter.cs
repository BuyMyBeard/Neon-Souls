using UnityEngine;

public class NightClubLowPassResetter : MonoBehaviour, IRechargeable
{
    [SerializeField] AudioLowPassFilter lowPassFilter;
    [SerializeField] float val;
    // Start is called before the first frame update
    void Awake()
    {
        lowPassFilter.cutoffFrequency = val;
    }
    public void Recharge(RechargeType rechargeType = RechargeType.Respawn)
    {
        lowPassFilter.cutoffFrequency = val;
    }
}
