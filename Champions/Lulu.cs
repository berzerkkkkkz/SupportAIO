using System.Linq;

using EnsoulSharp;

using SupportAIO.SpellBlocking;
using Extensions = SupportAIO.Common.Extensions;
using Color = System.Drawing.Color;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;
using Spell = EnsoulSharp.SDK.Spell;
using Champion = SupportAIO.Common.Champion;
using System.Windows.Forms;
using static EnsoulSharp.SDK.Gapcloser;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;

namespace SupportAIO.Champions
{
    class Lulu : Champion
    {
        private int language;

        internal Lulu()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        internal override void OnProcessSpellCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs
            args)
        {
            var attack = sender as AIHeroClient;
            var target = args.Target as AIHeroClient;
            if (attack != null && attack.IsAlly )
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["combo"]["wset"]["ally"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>() != 0)
                    .OrderByDescending(
                        x => RootMenu["combo"]["wset"]["ally"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>()))
                {

                    if (target != null && args.SData.Name.Contains("BasicAttack") &&
                        attack.Distance(Player) < E.Range && !attack.IsDead && attack == ally)
                    {
                        W.CastOnUnit(attack);
                    }
                }
            }

        }


        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"];
            bool useW = RootMenu["combo"]["wset"]["usew"];
            bool useR = RootMenu["combo"]["rset"]["user"];
            bool useEQ = RootMenu["combo"]["useeq"];
            if (useEQ)
            {

                if (Q.IsReady())
                {
                    foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (ally != null && !ally.IsMe)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range,
                                    false, ally.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {

                                if (ally.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(ally);
                                }
                            }
                        }
                    }
                    foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (minion != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, minion.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (minion.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                    foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (target != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, target.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (target.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }
                    }
                }


                foreach (var pixs in GameObjects.AllGameObjects)
                {

                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {

                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")) && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }

                                }
                            }
                        }
                    }


                }
            }
            if (E.IsReady())
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range) && target != null)
                {

                    if (target != null)
                    {
                        switch (RootMenu["combo"]["emode"].GetValue<MenuList>().Index)
                        {
                            case 0:
                                E.CastOnUnit(target);
                                break;
                            case 1:
                              
                                if (Player.CountAllyHeroesInRange(E.Range) == 0 ||
                                    target.HealthPercent < 5 && Player.HealthPercent > 20)
                                {
                                    E.CastOnUnit(target);
                                }
                                break;
                            case 2:

                                break;
                        }
                    }


                }
            }
            if (useR)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(R.Range, true)))
                {
                    if (ally != null)
                    {
                        if (ally.HealthPercent <= RootMenu["combo"]["rset"]["hp"].GetValue<MenuSlider>().Value)
                        {
                            if (ally.CountEnemyHeroesInRange(350) >= RootMenu["combo"]["rset"]["hitr"].GetValue<MenuSlider>().Value)
                            {
                                R.CastOnUnit(ally);
                            }
                        }
                    }
                }

            }
            if (useW)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
                if (target.IsValidTarget(W.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var enemy in GameObjects.EnemyHeroes.Where(
                                x => x.Distance(Player) <= W.Range &&
                                     RootMenu["combo"]["wset"]["enemy"][x.CharacterName.ToLower() + "priority"]
                                         .GetValue<MenuSlider>() != 0)
                            .OrderByDescending(
                                x => RootMenu["combo"]["wset"]["enemy"][x.CharacterName.ToLower() + "priority"]
                                   .GetValue<MenuSlider>()))
                        {
                            W.CastOnUnit(enemy);
                        }
                    }


                }
            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget(Q.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var pixs in GameObjects.AllGameObjects)
                        {
                            if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                            {
                                if (pixs.Distance(target) < Player.Distance(target))
                                {
                                    Q.From = pixs.Position;
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                                if (pixs.Distance(target) > Player.Distance(target))
                                {
                                    Q.From = Player.Position;
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["rset"]["semir"].GetValue<MenuKeyBind>().Active)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(x=> x.IsValidTarget(R.Range, true) && !x.IsDead && x != null).OrderBy(x=> x.Health))

                {
                    R.CastOnUnit(ally);
                }
            }
            if (RootMenu["flee"]["fleekey"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                if (W.IsReady())
                {
                    W.Cast();
                }
            }
            if (RootMenu["we"]["key"].GetValue<MenuKeyBind>().Active)
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["we"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>().Value != 0)
                    .OrderByDescending(
                        x => RootMenu["we"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>().Value))
                {
                    W.CastOnUnit(ally);
                    E.CastOnUnit(ally);
                }
            }
        }
 
        protected override void Farming()
        {

        }

        protected override void Drawings()
        {

            if (RootMenu["drawings"]["drawq"])
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"])
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawe"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawpix"])
            {
                foreach (var pix in GameObjects.AllGameObjects)
                {
                    if (pix.Name == "RobotBuddy" && pix.IsValid && pix != null && !pix.IsDead && pix.Team == Player.Team)
                    {
                        Render.Circle.DrawCircle(pix.Position, 60, Color.HotPink);
                    }
                }

            }
            if (RootMenu["drawings"]["pixranges"])
            {
                foreach (var pixs in GameObjects.AllGameObjects)
                {
                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                pix.HasBuff("lulufaerieattackaid"))
                            {
                                Render.Circle.DrawCircle(pixs.Position, Q.Range, Color.GreenYellow);
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                pix.HasBuff("lulufaerieburn"))
                            {
                                Render.Circle.DrawCircle(pixs.Position, Q.Range, Color.GreenYellow);
                            }
                        }

                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                 pix.HasBuff("luluevision")))
                            {

                                Render.Circle.DrawCircle(pixs.Position, Q.Range, Color.GreenYellow);
                            }
                        }
                    }
                }
            }
          
        }

        protected override void Killsteal()
        {
            if (Q.IsReady() &&
                RootMenu["killsteal"]["ksq"])
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    foreach (var pixs in GameObjects.AllGameObjects)
                    {
                        if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                        {
                            if (pixs.Distance(bestTarget) < Player.Distance(bestTarget))
                            {
                                Q.From = pixs.Position;
                                var pred = Q.GetPrediction(bestTarget);
                                if (pred.Hitchance >= HitChance.High)
                                {
                                    Q.Cast(pred.CastPosition, true);
                                }
                            }
                            if (pixs.Distance(bestTarget) > Player.Distance(bestTarget))
                            {
                                Q.From = Player.Position;
                                var pred = Q.GetPrediction(bestTarget);
                                if (pred.Hitchance >= HitChance.High)
                                {
                                    Q.Cast(pred.CastPosition, true);
                                }
                            }
                        }
                    }
                }
            }
            if (E.IsReady() &&
                RootMenu["killsteal"]["kse"])
            {
                var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(bestTarget);
                }
            }
            if (Q.IsReady() && RootMenu["killsteal"]["kseq"])
            {
                var bestTarget = Extensions.GetBestKillableHeroEQ(DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(E.Range + Q.Range) && bestTarget.Distance(Player) > Q.Range)
                {
                    foreach (var ally in GameObjects.AllyHeroes
                        .Where(x => x.IsValidTarget(E.Range, true) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(ally);
                    }
                    foreach (var minion in GameObjects.Minions
                        .Where(x => x.IsValidTarget(E.Range) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(minion);
                    }
                    foreach (var enemy in GameObjects.EnemyHeroes
                        .Where(x => x.IsValidTarget(E.Range) && x != null && x.Distance(Player) < E.Range &&
                                    x.Distance(bestTarget) < Q.Range)
                        .OrderBy(x => x.Distance(bestTarget)))
                    {
                        E.CastOnUnit(enemy);
                    }
                    foreach (var pixs in GameObjects.AllGameObjects)
                    {
                        if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                        {
                            foreach (var pix in GameObjects.AllyHeroes)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(bestTarget) < Q.Range)
                                {
                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(bestTarget);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                            foreach (var pix in GameObjects.EnemyHeroes)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(bestTarget) < Q.Range)
                                {
                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(bestTarget);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                            foreach (var pix in GameObjects.EnemyMinions)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")))
                                {

                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(bestTarget);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void Harass()
        {
            bool useQ = RootMenu["harass"]["useq"];
            bool useE = RootMenu["harass"]["usee"];

            bool useEQ = RootMenu["harass"]["useeq"];
            if (useEQ)
            {

                if (Q.IsReady())
                {
                    foreach (var ally in GameObjects.AllyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (ally != null && !ally.IsMe)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, ally.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (ally.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(ally);
                                }
                            }
                        }
                    }
                    foreach (var minion in GameObjects.EnemyMinions.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (minion != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range,
                                    false, minion.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (minion.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(minion);
                                }
                            }
                        }
                    }
                    foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(E.Range, true)))
                    {
                        if (target != null)
                        {

                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, target.Position));
                            if (enemyInBounceRange != null && enemyInBounceRange.Distance(Player) > Q.Range)
                            {


                                if (target.Distance(enemyInBounceRange) < Q.Range)
                                {
                                    E.CastOnUnit(target);
                                }
                            }
                        }
                    }
                }


                foreach (var pixs in GameObjects.AllGameObjects)
                {

                    if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                    {
                        foreach (var pix in GameObjects.AllyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800, true) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieattackaid") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyHeroes)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    pix.HasBuff("lulufaerieburn") && pix.Distance(enemyInBounceRange) < Q.Range)
                                {

                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }

                                }
                            }
                        }
                        foreach (var pix in GameObjects.EnemyMinions)
                        {
                            var enemyInBounceRange =
                                GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(Q.Range, 
                                    false, pix.Position));
                            if (enemyInBounceRange != null)
                            {
                                if (pix.IsValidTarget(1800) && pix != null && pix.Distance(Player) < 1800 &&
                                    (pix.HasBuff("lulufaerieburn") || pix.HasBuff("lulufaerieattackaid") ||
                                     pix.HasBuff("luluevision")) && pix.Distance(enemyInBounceRange) < Q.Range)
                                {


                                    Q.From = pix.Position;
                                    var pred = Q.GetPrediction(enemyInBounceRange);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }

                                }
                            }
                        }
                    }


                }
            }
            if (useE)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);
                if (target.IsValidTarget(E.Range) && target != null)
                {

                    if (target != null)
                    {
                        E.CastOnUnit(target);
                    }
                }


            }
            if (useQ)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (target.IsValidTarget(Q.Range) && target != null)
                {

                    if (target != null)
                    {
                        foreach (var pixs in GameObjects.AllGameObjects)
                        {
                            if (pixs.Name == "RobotBuddy" && pixs.IsValid && pixs != null && !pixs.IsDead && pixs.Team == Player.Team)
                            {
                                if (pixs.Distance(target) < Player.Distance(target))
                                {
                                    Q.From = pixs.Position;
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                                if (pixs.Distance(target) > Player.Distance(target))
                                {
                                    Q.From = Player.Position;
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
        }


        internal override void OnAction(object sender, OrbwalkerActionArgs e)
        {
            if (e.Type == OrbwalkerType.BeforeAttack)
            {
                if (RootMenu["combo"]["support"])
                {
                    if (Orbwalker.ActiveMode.Equals(OrbwalkerMode.LastHit) ||
                        Orbwalker.ActiveMode.Equals(OrbwalkerMode.LaneClear) ||
                        Orbwalker.ActiveMode.Equals(OrbwalkerMode.Harass))
                    {
                        if (e.Target.Type == GameObjectType.AIMinionClient && GameObjects.AllyHeroes.Where(x => x.Distance(Player) < 1000 && !x.IsMe).Count() > 0)
                        {
                            e.Process = false;
                        }
                    }
                }
            }
        }

        protected override void SetMenu()
        {
            RootMenu = new Menu("root", $"辅助合集{ObjectManager.Player.CharacterName}", true);

            RootMenu.Add(new MenuList("language", "Language(语言选择)", new[] { "中文", "Englsih" }) { Index = 0 });
            RootMenu.Add(new MenuSeparator("1", "Press F5 to reload language(按 F5 确认切换语言)"));
            language = RootMenu.GetValue<MenuList>("language").Index;
            if (language != 1)
            {

                ComboMenu = new Menu("combo", "连招");
                {
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuBool("useeq", "使用EQ延长攻击", false));
                    var WSettings = new Menu("wset", "W 设置");
                    WSettings.Add(new MenuBool("usew", "使用 W"));
                    var EnemySet = new Menu("enemy", "敌人设定");
                    EnemySet.Add(new MenuSeparator("meow", "0 为禁用"));
                    EnemySet.Add(new MenuSeparator("meowmeow", "1 优先最低,5 最高"));
                    foreach (var target in GameObjects.EnemyHeroes)
                    {

                        EnemySet.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " 优先级: ", 1, 0, 5));

                    }
                    var AllySet = new Menu("ally", "队友设定");
                    AllySet.Add(new MenuSeparator("meow", "0 为禁用"));
                    AllySet.Add(new MenuSeparator("meowmeow", "1 优先最低,5 最高"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        AllySet.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " 优先级: ", 1, 0, 5));

                    }
                    ComboMenu.Add(WSettings);
                    WSettings.Add(EnemySet);
                    WSettings.Add(AllySet);

                    ComboMenu.Add(new MenuList("emode", "E 模式", new[] { "总是", "连招逻辑", "从不" }));
                    var RSettings = new Menu("rset", "R 设置");
                    RSettings.Add(new MenuBool("user", "使用 R"));
                    RSettings.Add(new MenuSlider("hitr", "^- 若可击飞敌人 >=", 2, 0, 5));
                    RSettings.Add(new MenuSlider("hp", "^- 若队友血量% <=", 20, 0, 100));
                    RSettings.Add(new MenuBool("autor", "受致命伤害前R"));
                    RSettings.Add(new MenuKeyBind("semir", "半自动R最低血量队友", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(RSettings);
                    ComboMenu.Add(new MenuBool("support", "辅助模式", false));

                }

                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuSlider("mana", "蓝量管理", 30, 0, 100));
                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("usee", "使用 E"));
                    HarassMenu.Add(new MenuBool("useeq", "使用EQ延长攻击"));

                }
                RootMenu.Add(HarassMenu);
                var WE = new Menu("we", "WE 设定");
                WE.Add(new MenuKeyBind("key", "WE连招", Keys.G, KeyBindType.Press));

                WE.Add(new MenuSeparator("meow", "0 为禁用"));
                WE.Add(new MenuSeparator("meowmeow", "1 优先最低,5 最高"));
                foreach (var target in GameObjects.AllyHeroes)
                {

                    WE.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " 优先级: ", 1, 0, 5));

                }
                RootMenu.Add(WE);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                    DrawMenu.Add(new MenuBool("drawpix", "显示 皮克斯位置"));
                    DrawMenu.Add(new MenuBool("pixranges", "显示 与皮克斯距离"));
                }
                RootMenu.Add(DrawMenu);

                EvadeMenu = new Menu("wset", "护盾设置");
                {
                    var First = new Menu("first", "技能列表");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);
                    EvadeMenu.Add(First);


                }
                RootMenu.Add(EvadeMenu);
                KillstealMenu = new Menu("killsteal", "抢人头");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "使用 Q"));
                    KillstealMenu.Add(new MenuBool("kse", "使用 E"));
                    KillstealMenu.Add(new MenuBool("kseq", "使用 EQ"));
                }
                RootMenu.Add(KillstealMenu);
                FarmMenu = new Menu("flee", "逃跑设定");
                {
                    FarmMenu.Add(new MenuKeyBind("fleekey", "逃跑按键", Keys.Z, KeyBindType.Press));

                }
                RootMenu.Add(FarmMenu);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("useeq", "Use E > Q Extended in Combo", false));
                    var WSettings = new Menu("wset", "W Settings");
                    WSettings.Add(new MenuBool("usew", "Use W in Combo"));
                    var EnemySet = new Menu("enemy", "Enemy Settings");
                    EnemySet.Add(new MenuSeparator("meow", "0 - Disabled"));
                    EnemySet.Add(new MenuSeparator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                    foreach (var target in GameObjects.EnemyHeroes)
                    {

                        EnemySet.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " Priority: ", 1, 0, 5));

                    }
                    var AllySet = new Menu("ally", "Ally Settings");
                    AllySet.Add(new MenuSeparator("meow", "0 - Disabled"));
                    AllySet.Add(new MenuSeparator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        AllySet.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " Priority: ", 1, 0, 5));

                    }
                    ComboMenu.Add(WSettings);
                    WSettings.Add(EnemySet);
                    WSettings.Add(AllySet);

                    ComboMenu.Add(new MenuList("emode", "E Mode on Enemy", new[] { "Always", "Logic", "Never" }));
                    var RSettings = new Menu("rset", "R Settings");
                    RSettings.Add(new MenuBool("user", "Use R in Combo"));
                    RSettings.Add(new MenuSlider("hitr", "^- if Knocks Up X Enemies", 2, 0, 5));
                    RSettings.Add(new MenuSlider("hp", "^- if Ally is Lower than X Health", 20, 0, 100));
                    RSettings.Add(new MenuBool("autor", "Auto R if Incoming Damage will Kill"));
                    RSettings.Add(new MenuKeyBind("semir", "Semi-R on Lowest Health Ally", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(RSettings);
                    ComboMenu.Add(new MenuBool("support", "Support Mode", false));

                }

                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 30, 0, 100));
                    HarassMenu.Add(new MenuBool("useq", "Harass with Q"));
                    HarassMenu.Add(new MenuBool("usee", "Harass with E"));
                    HarassMenu.Add(new MenuBool("useeq", "Harass with E > Q Extended"));

                }
                RootMenu.Add(HarassMenu);
                var WE = new Menu("we", "W > E Settings");
                WE.Add(new MenuKeyBind("key", "W > E Key", Keys.Z, KeyBindType.Press));

                WE.Add(new MenuSeparator("meow", "0 - Disabled"));
                WE.Add(new MenuSeparator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                foreach (var target in GameObjects.AllyHeroes)
                {

                    WE.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " Priority: ", 1, 0, 5));

                }
                RootMenu.Add(WE);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                    DrawMenu.Add(new MenuBool("drawpix", "Draw Pix Position"));
                    DrawMenu.Add(new MenuBool("pixranges", "Draw Ranges from Pix"));
 
                }
                RootMenu.Add(DrawMenu);

                EvadeMenu = new Menu("wset", "Shielding");
                {
                    var First = new Menu("first", "Spells Detector");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);
                    EvadeMenu.Add(First);

                }
                RootMenu.Add(EvadeMenu);
                KillstealMenu = new Menu("killsteal", "Killsteal");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                    KillstealMenu.Add(new MenuBool("kse", "Killsteal with E"));
                    KillstealMenu.Add(new MenuBool("kseq", "Killsteal with E > Q"));
                }
                RootMenu.Add(KillstealMenu);
                FarmMenu = new Menu("flee", "Flee");
                {
                    FarmMenu.Add(new MenuKeyBind("fleekey", "Fleey Key", Keys.G, KeyBindType.Press));

                }
                RootMenu.Add(FarmMenu);
            }

            RootMenu.Attach();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {
         

                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    W.CastOnUnit(target);
                }
            
        }


        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 650);
            R = new Spell(SpellSlot.R, 900);
            Q.SetSkillshot(0.25f, 60, 1400, false, SkillshotType.Line, false, HitChance.None);
        }
    }
}
