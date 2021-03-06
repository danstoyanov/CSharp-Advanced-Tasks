﻿using System;
using System.Collections.Generic;
using System.Linq;
using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players;
using CounterStrike.Models.Players.Contracts;

namespace CounterStrike.Models.Maps.MapModels
{
    public class Map : IMap
    {
        public string Start(ICollection<IPlayer> players)
        {
            var terrorists = players
                .Where(p => p.GetType().Name == nameof(Terrorist))
                .ToList();

            var counterTerrorists = players
                .Where(p => p.GetType().Name == nameof(CounterTerrorist))
                .ToList();

            while (terrorists.Any(t => t.IsAlive) &&
                   counterTerrorists.Any(t => t.IsAlive))
            {
                foreach (var t in terrorists
                    .Where(p => p.IsAlive))
                {
                    foreach (var ct in counterTerrorists
                        .Where(p => p.IsAlive))
                    {
                        var bulletsFired = t.Gun.Fire();

                        ct.TakeDamage(bulletsFired);
                    }
                }

                foreach (var ct in counterTerrorists
                    .Where(p => p.IsAlive))
                {
                    foreach (var t in terrorists
                        .Where(p => p.IsAlive))
                    {
                        var bulletsFired = ct.Gun.Fire();

                        t.TakeDamage(bulletsFired);
                    }
                }
            }

            var result = "";

            result = !counterTerrorists.Any(p => p.IsAlive) ?
                     $"Terrorist wins!" :
                     $"Counter Terrorist wins!";

            return result;
        }
    }
}