﻿using NLog;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Managers;
using Torch.Managers.PatchManager;
using Torch.Server;
using Torch.Session;

namespace ALE_RestartWatchdog
{
    class RestartManager : Manager {

        [Dependency]
        private TorchSessionManager sessionManager;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public RestartManager(ITorchBase torchInstance)
            : base(torchInstance) {
        }

        /// <inheritdoc />
        public override void Attach() {
            base.Attach();

            sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");
        }

        /// <inheritdoc />
        public override void Detach() {
            base.Detach();

            Log.Info("Detached!");
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state) {

            RestartConfig config = RestartPlugin.Instance.Config;

            switch (state) {
                case TorchSessionState.Unloading:

                    Log.Info("Start Unloading!!!");

                    int delayUnloading = config.DelayInSeconds;

                    Log.Info("Wait for " + delayUnloading + " before force Restart!");

                    /* Delay is in Seconds We need Milliseconds so * 1000 */
                    Task.Delay(delayUnloading * 1000).ContinueWith((t) => {
                        CheckRestart(session, TorchSessionState.Unloading);
                    });
                    break;

                case TorchSessionState.Unloaded:

                    Log.Info("Start Unloaded!!!");

                    int delayUnloaded = config.DelayInSecondsAfterUnload;

                    Log.Info("Wait for " + delayUnloaded + " before force Restart!");

                    /* Delay is in Seconds We need Milliseconds so * 1000 */
                    Task.Delay(delayUnloaded * 1000).ContinueWith((t) => {
                        CheckRestart(session, TorchSessionState.Unloaded);
                    });
                    break;
            }
        }

        private void CheckRestart(ITorchSession session, TorchSessionState referenceState) {


            /* State is fine so no need to fix anything */
            if (session.State != referenceState) {
                Log.Info("Shutdown successful no need to interfere after "+ referenceState + "!");
                return;
            }

            Log.Warn("Unloading took too long. Force restart!");

            string exe = Assembly.GetEntryAssembly().Location;

            /* Making sure we found an EXE */
            if (exe == null || exe.Equals("")) {
                Log.Error("Could not find Path to exe file! Aborting Restart!");
                return;
            }

            try {

                var torchConfig = (TorchConfig) session.Torch.Config;
                /* Making sure Logger is done */
                LogManager.Flush();

                Process currentProcess = Process.GetCurrentProcess();

                /** Tell the new Torch instance to wait till the old one is gone */
                torchConfig.WaitForPID = currentProcess.Id.ToString();
                torchConfig.Autostart = true;

                Log.Info("Starting '" + exe + "' with '" + torchConfig.ToString() + "'!");
                Process.Start(exe, torchConfig.ToString());

                Log.Info("Killing current Process!");
                currentProcess.Kill();

            } catch (Exception e) {
                Log.Error("Could not Restart the Server! Tried running '" + exe + "' And Stopping PID " + Process.GetCurrentProcess().Id.ToString(), e);
            }
        }
    }
}