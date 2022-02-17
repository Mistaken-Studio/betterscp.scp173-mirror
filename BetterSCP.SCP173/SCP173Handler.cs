// -----------------------------------------------------------------------
// <copyright file="SCP173Handler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Mirror;
using Mistaken.API.Diagnostics;
using UnityEngine;

namespace Mistaken.BetterSCP.SCP173
{
    internal class SCP173Handler : Module
    {
        public SCP173Handler(PluginHandler p)
            : base(p)
        {
        }

        public override string Name => nameof(SCP173Handler);

        public override void OnEnable()
        {
            Exiled.Events.Handlers.Scp173.PlacingTantrum += this.Scp173_PlacingTantrum;
            Exiled.Events.Handlers.Server.RestartingRound += this.Server_RestartingRound;
        }

        public override void OnDisable()
        {
            Exiled.Events.Handlers.Scp173.PlacingTantrum -= this.Scp173_PlacingTantrum;
            Exiled.Events.Handlers.Server.RestartingRound -= this.Server_RestartingRound;
        }

        private readonly Queue<GameObject> scp173Tantrums = new Queue<GameObject>();

        private void Scp173_PlacingTantrum(Exiled.Events.EventArgs.PlacingTantrumEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            if (this.scp173Tantrums.Count >= PluginHandler.Instance.Config.TantrumLimit)
                NetworkServer.Destroy(this.scp173Tantrums.Dequeue());

            this.scp173Tantrums.Enqueue(ev.GameObject);
        }

        private void Server_RestartingRound()
        {
            this.scp173Tantrums.Clear();
        }
    }
}
