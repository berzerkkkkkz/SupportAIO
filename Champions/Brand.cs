using System.Linq;
using EnsoulSharp;

using Extensions = SupportAIO.Common.Extensions;
using Color = System.Drawing.Color;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;
using Spell = EnsoulSharp.SDK.Spell;
using Champion = SupportAIO.Common.Champion;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;

namespace SupportAIO.Champions
{
    class Brand : Champion
    {
        private int language;

        internal Brand()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"];
            bool useW = RootMenu["combo"]["usew"];
            bool useE = RootMenu["combo"]["usee"];
            bool useR = RootMenu["combo"]["user"];

            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {
                return;
            }
            switch (RootMenu["combo"]["combomode"].GetValue<MenuList>().Index)
            {
                case 1:

                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }
                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].GetValue<MenuList>().Index)
                            {
                                case 0:
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            var pred = W.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                W.Cast(pred.CastPosition, true);
                            }
                        }
                    }
                    break;
                case 0:
                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            var pred = W.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                W.Cast(pred.CastPosition, true);
                            }
                        }
                    }
                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].GetValue<MenuList>().Index)
                            {
                                case 0:
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    break;
                case 3:
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            var pred = W.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                W.Cast(pred.CastPosition, true);
                            }
                        }
                    }
                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }

                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].GetValue<MenuList>().Index)
                            {
                                case 0:
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                        pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    break;
                case 2:
                    if (target.IsValidTarget(W.Range) && useW)
                    {
                        if (target != null)
                        {
                            var pred = W.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                W.Cast(pred.CastPosition, true);
                            }
                        }
                    }

                    if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            switch (RootMenu["combo"]["qmode"].GetValue<MenuList>().Index)
                            {
                                case 0:
                                    var pred = Q.GetPrediction(target);
                                    if (pred.Hitchance >= HitChance.High)
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    }
                                    break;
                                case 1:
                                    if (target.HasBuff("brandablaze"))
                                    {
                                         pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (target.IsValidTarget(E.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.CastOnUnit(target);
                        }
                    }


                    break;

            }
            if (useR)
            {
                switch (RootMenu["combo"]["rmode"].GetValue<MenuList>().Index)
                {
                    case 0:
                        if (target != null)
                        {
                            if (!RootMenu["combo"]["minion"])
                            {
                                if (target.IsValidTarget(R.Range) && target.CountEnemyHeroesInRange(750) >=
                                    RootMenu["combo"]["bounce"].GetValue<MenuSlider>().Value && target.HealthPercent <=
                                    RootMenu["combo"]["hp"].GetValue<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                            if (RootMenu["combo"]["minion"])
                            {
                                if (target.IsValidTarget(R.Range) &&
                                    (target.CountEnemyHeroesInRange(750) +
                                     GameObjects.EnemyMinions.Count(h => h.IsValidTarget(750, false,
                                         target.Position)) >=
                                     RootMenu["combo"]["bounce"].GetValue<MenuSlider>().Value) && target.HealthPercent <=
                                    RootMenu["combo"]["hp"].GetValue<MenuSlider>().Value)
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                        }
                        break;
                    case 1:
                        if (target != null)
                        {
                            if (!RootMenu["combo"]["minion"])
                            {
                                if (target.IsValidTarget(R.Range) && target.CountEnemyHeroesInRange(750) >=
                                    RootMenu["combo"]["bounce"].GetValue<MenuSlider>().Value && target.Health <=
                                    Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.E) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                            if (RootMenu["combo"]["minion"])
                            {
                                if (target.IsValidTarget(R.Range) &&
                                    (target.CountEnemyHeroesInRange(750) +
                                     GameObjects.EnemyMinions.Count(h => h.IsValidTarget(750, false,
                                         target.Position)) >=
                                     RootMenu["combo"]["bounce"].GetValue<MenuSlider>().Value) && target.Health <=
                                    Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.E) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.CastOnUnit(target);
                                }
                            }
                        }
                        break;
                }
            }
        }


        protected override void SemiR()
        {
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
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.Aquamarine);
            }
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Yellow);
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


        protected override void Killsteal()
        {
            if (Q.IsReady() &&
                RootMenu["ks"]["ksq"])
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health && bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast(bestTarget);
                }
            }
            if (W.IsReady() &&
                RootMenu["ks"]["ksw"])
            {
                var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health && bestTarget.IsValidTarget(W.Range))
                {
                    W.Cast(bestTarget);
                }
            }
            if (E.IsReady() &&
                RootMenu["ks"]["kse"])
            {
                var bestTarget = Extensions.GetBestKillableHero(E, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.E) >= bestTarget.Health && bestTarget.IsValidTarget(E.Range))
                {
                    E.CastOnUnit(bestTarget);
                }
            }
            if (R.IsReady() &&
                RootMenu["ks"]["ksr"])
            {
                var bestTarget = Extensions.GetBestKillableHero(R, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.R) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(R.Range))
                {
                    if (bestTarget.CountEnemyHeroesInRange(750) >=
                        RootMenu["ks"]["bounce"].GetValue<MenuSlider>().Value)
                    {
                        R.CastOnUnit(bestTarget);
                    }
                }
            }
        }

        protected override void Harass()
        {
            if (Player.ManaPercent >= RootMenu["harass"]["mana"].GetValue<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"];
                bool useW = RootMenu["harass"]["usew"];
                bool useE = RootMenu["harass"]["usee"];

                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                switch (RootMenu["harass"]["harassmode"].GetValue<MenuList>().Index)
                {
                    case 1:

                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }
                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].GetValue<MenuList>().Index)
                                {
                                    case 0:
                                        var pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            pred = Q.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                Q.Cast(pred.CastPosition, true);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        break;
                    case 0:
                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].GetValue<MenuList>().Index)
                                {
                                    case 0:
                                        var pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            pred = Q.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                Q.Cast(pred.CastPosition, true);
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        break;
                    case 3:
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }
                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }

                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].GetValue<MenuList>().Index)
                                {
                                    case 0:
                                        var pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            pred = Q.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                Q.Cast(pred.CastPosition, true);
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        break;
                    case 2:
                        if (target.IsValidTarget(W.Range) && useW)
                        {

                            if (target != null)
                            {
                                W.Cast(target);
                            }
                        }

                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                switch (RootMenu["harass"]["qmode"].GetValue<MenuList>().Index)
                                {
                                    case 0:
                                        var pred = Q.GetPrediction(target);
                                        if (pred.Hitchance >= HitChance.High)
                                        {
                                            Q.Cast(pred.CastPosition, true);
                                        }
                                        break;
                                    case 1:
                                        if (target.HasBuff("brandablaze"))
                                        {
                                            pred = Q.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                Q.Cast(pred.CastPosition, true);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        if (target.IsValidTarget(E.Range) && useE)
                        {

                            if (target != null)
                            {
                                E.CastOnUnit(target);
                            }
                        }


                        break;

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
                    ComboMenu.Add(new MenuList("combomode", "连招模式", new[] { "E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E" }));
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuList("qmode", "Q 模式", new[] { "总是", "仅当可晕" }));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuList("rmode", "R 模式", new[] { "低血量", "可击杀" }));

                    ComboMenu.Add(new MenuSlider("bounce", "R命中人数>=", 1, 1, 5));
                    ComboMenu.Add(new MenuBool("minion", "^- 计算弹射小兵"));

                    ComboMenu.Add(new MenuSlider("hp", "使用R当目标血量% <= (低血量模式)", 50, 1, 100));
                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                }
                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuSlider("mana", "蓝量控制", 50, 1, 100));

                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuList("qmode", "Q 模式", new[] { "总是", "仅当可晕" }));
                    HarassMenu.Add(new MenuBool("usew", "使用 W"));
                    HarassMenu.Add(new MenuBool("usee", "使用 E"));
                    HarassMenu.Add(new MenuList("harassmode", "骚扰模式", new[] { "E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E" }));

                }
                RootMenu.Add(HarassMenu);
                KillstealMenu = new Menu("ks", "抢人头");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "使用Q"));
                    KillstealMenu.Add(new MenuBool("ksw", "使用W"));
                    KillstealMenu.Add(new MenuBool("kse", "使用E"));
                    KillstealMenu.Add(new MenuBool("ksr", "使用R"));
                    KillstealMenu.Add(new MenuSlider("bounce", "^- 仅当可弹射敌人>=", 1, 1, 5));

                }
                RootMenu.Add(KillstealMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                }
                RootMenu.Add(DrawMenu);
          
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuList("combomode", "Combo Mode", new[] { "E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E" }));
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuList("qmode", "Q Mode", new[] { "Always", "Only Stun" }));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuList("rmode", "R Mode", new[] { "If X Health", "Only if Killable" }));

                    ComboMenu.Add(new MenuSlider("bounce", "Use R if Bounces On X Targets", 1, 1, 5));
                    ComboMenu.Add(new MenuBool("minion", "^- Include Minions for Bounce"));

                    ComboMenu.Add(new MenuSlider("hp", "If Health <= ( If X Health Mode)", 50, 1, 100));
                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                }
                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));

                    HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                    HarassMenu.Add(new MenuList("qmode", "Q Mode", new[] { "Always", "Only Stun" }));
                    HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    HarassMenu.Add(new MenuList("harassmode", "Harass Mode", new[] { "E-Q-W", "E-W-Q", "W-E-Q", "W-Q-E" }));

                }
                RootMenu.Add(HarassMenu);
                KillstealMenu = new Menu("ks", "Killsteal");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "Killseal with Q"));
                    KillstealMenu.Add(new MenuBool("ksw", "Killseal with W"));
                    KillstealMenu.Add(new MenuBool("kse", "Killseal with E"));
                    KillstealMenu.Add(new MenuBool("ksr", "Killseal with R"));
                    KillstealMenu.Add(new MenuSlider("bounce", "^- Only if Bounces on X Enemies", 1, 1, 5));

                }
                RootMenu.Add(KillstealMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                }
                RootMenu.Add(DrawMenu);
            }
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 900);
            E = new Spell(SpellSlot.E, 625);
            R = new Spell(SpellSlot.R, 750);
            Q.SetSkillshot(0.25f, 75f, 1600, true, SkillshotType.Line);
            W.SetSkillshot(0.85f, 200f, 3200, false, SkillshotType.Circle);
        }
    }
}
