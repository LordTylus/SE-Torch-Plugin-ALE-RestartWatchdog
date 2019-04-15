using Torch;

namespace ALE_RestartWatchdog {
    
    public class RestartConfig : ViewModel
    {
        private int _delayInSeconds = 60 * 2; //2 Minutes

        public int DelayInSeconds { get => _delayInSeconds; set => SetValue(ref _delayInSeconds, value); }
    }
}
