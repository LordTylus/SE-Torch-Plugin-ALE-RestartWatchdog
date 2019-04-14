using Torch;

namespace ALE_RestartWatchdog {
    
    public class RestartConfig : ViewModel
    {
        private int _delayInSeconds = 60 * 2; //2 Minutes
        private int _delayInSecondsAfterUnload = 60 * 1; //1 Minutes

        public int DelayInSeconds { get => _delayInSeconds; set => SetValue(ref _delayInSeconds, value); }

        public int DelayInSecondsAfterUnload { get => _delayInSecondsAfterUnload; set => SetValue(ref _delayInSecondsAfterUnload, value); }
    }
}
