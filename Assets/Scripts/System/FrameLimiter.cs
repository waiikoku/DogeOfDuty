using UnityEngine;

namespace Dod
{
    public class FrameLimiter : MonoBehaviour
    {
        private const int ZERO = 0;
        private const int POTATO = 15;
        private const int MOVIE = 24;
        private const int NORMAL = 30;
        private const int HIGH = 60;

        private enum Target
        {
            Low = POTATO,
            Movie = MOVIE,
            Normal = NORMAL,
            Gaming = HIGH,
            Unlimit = ZERO,
        }
        [SerializeField] private Target currentTarget = Target.Normal;

        private void Awake()
        {
            QualitySettings.vSyncCount = currentTarget == Target.Unlimit ? 0 : 1;
            Application.targetFrameRate = (int)currentTarget;
        }
    }
}
