using Torch;

namespace ALE_RestartWatchdog {
    
    public class RestartConfig : ViewModel
    {
        private bool _enabled = true;
        private int _delayInSeconds = 60 * 2; //2 Minutes

        public bool Enabled { get => _enabled; set => SetValue(ref _enabled, value); }

        public int DelayInSeconds { get => _delayInSeconds; set => SetValue(ref _delayInSeconds, value); }
    }
}
