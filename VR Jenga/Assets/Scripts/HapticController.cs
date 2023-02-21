using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticController : MonoBehaviour
{

    public XRBaseController leftController;
    public XRBaseController rightController;

    public float defaultAmp = 0.2f;
    public float defualtDur = 0.5f;

    [ContextMenu("Send Haptics")]

    public void SnedHaptic()
    {
        leftController.SendHapticImpulse(defaultAmp, defualtDur);
        rightController.SendHapticImpulse(defaultAmp, defualtDur);
    }
}
