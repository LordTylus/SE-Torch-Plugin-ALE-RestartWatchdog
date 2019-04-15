using NLog;
using System;
using System.IO;
using Torch;
using Torch.API;
using Torch.API.Plugins;

namespace ALE_RestartWatchdog {

    public class RestartPlugin : TorchPluginBase
    {

        public static RestartPlugin Instance { get; private set; }
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private Persistent<RestartConfig> _config;
        public RestartConfig Config => _config?.Data;

        public void Save() => _config.Save();

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            var configFile = Path.Combine(StoragePath, "RestartWatchdog.cfg");

            try {

                _config = Persistent<RestartConfig>.Load(configFile);

            } catch (Exception e) {
                Log.Warn(e);
            }

            if (_config?.Data == null) {

                Log.Info("Create Default Config, because none was found!");

                _config = new Persistent<RestartConfig>(configFile, new RestartConfig());
                _config.Save();
            }

            var pgmr = new RestartManager(torch);
            torch.Managers.AddManager(pgmr);

            Instance = this;
        }
    }
}