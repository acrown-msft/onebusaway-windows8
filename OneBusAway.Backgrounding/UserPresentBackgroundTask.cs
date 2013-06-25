﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OneBusAway.Backgrounding
{
    /// <summary>
    /// This task runs when the user becomes present (IE they unlock the device).
    /// </summary>
    public sealed class UserPresentBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Runs the tile updater service.
        /// </summary>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (TileUpdaterService.Instance.CreateIfNeccessary())
            {
                taskInstance.Canceled += (sender, reason) => TileUpdaterService.Instance.Abort();
                var deferral = taskInstance.GetDeferral();
                await TileUpdaterService.Instance.ServiceAborted;
                deferral.Complete();
            }
        }
    }
}
