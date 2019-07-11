using System.Linq;
using EnsoulSharp;

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
    class Nami : Champion
    {
        private int language;

        internal Nami()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {

            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                Q.Cast(Args.EndPosition);
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



        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"];



            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
            }

            if (RootMenu["combo"]["user"])
            {
                if (Extensions.GetBestEnemyHeroTargetInRange(R.Range) != null)
                {
                    if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value > 1)
                    {
                        R.CastIfWillHit(Extensions.GetBestEnemyHeroTargetInRange(R.Range),
                            RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value - 1);
                    }
                    if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value == 1)
                    {
                        R.Cast(Extensions.GetBestEnemyHeroTargetInRange(R.Range));
                    }
                }

            }
            var targets = Extensions.GetBestEnemyHeroTargetInRange(W.Range);
            switch (RootMenu["combo"]["wmode"].GetValue<MenuList>().Index)
            {

                case 0:
                {
                    return;
                }
                case 1:
                    if (targets.IsValidTarget(W.Range) && targets != null)
                    {

                        if (targets.CountAllyHeroesInRange(690) >= 0)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    break;
                case 2:
                    foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.IsValidTarget(W.Range, true) && x.Distance(Player) < W.Range && !x.IsDead))
                    {
                        foreach (var targeto in GameObjects.EnemyHeroes.Where(
                            x => x.Distance(ally) < W.Range && x != null && !x.IsDead))
                        {


                            if (ally.Distance(targeto) < 690 && ally.IsAlly)
                            {
                                if (ally.CountEnemyHeroesInRange(690) > 0)
                                {
                                    W.CastOnUnit(ally);
                                }
                            }
                        }
                    }
                    break;

            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["semir"].GetValue<MenuKeyBind>().Active)
            {
                if (Extensions.GetBestEnemyHeroTargetInRange(R.Range) != null)
                {
                    if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value > 1)
                    {
                        R.CastIfWillHit(Extensions.GetBestEnemyHeroTargetInRange(R.Range),
                            RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value - 1);
                    }
                    if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value == 1)
                    {
                        R.Cast(Extensions.GetBestEnemyHeroTargetInRange(R.Range));
                    }
                }

            }
            if (RootMenu["misc"]["autoq"])
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(Q.Range)))
                {

                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }

            }
            if (RootMenu["white"]["enable"])
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                        x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsRecalling() &&
                             RootMenu["white"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>().Value != 0 &&
                             x.HealthPercent <= RootMenu["white"][x.CharacterName.ToLower() + "hp"].GetValue<MenuSlider>().Value)
                    .OrderByDescending(x => RootMenu["white"][x.CharacterName.ToLower() + "priority"].GetValue<MenuSlider>().Value)
                    .Where(x => x.Distance(Player) < W.Range).OrderBy(x => x.Health))
                {


                    if (ally != null && !ally.IsDead)
                    {
                        W.Cast(ally);
                    }
                }

            }

        }







        protected override void Farming()
        {

        }

        internal override void OnProcessSpellCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            var attack = sender as AIHeroClient;
            var target = args.Target as AIHeroClient;
            if (attack != null && attack.IsAlly && !attack.IsMinion)
            {
                if (RootMenu["ewhite"][attack.CharacterName.ToLower()] && RootMenu["combo"]["usee"])
                {
                    if (target != null && args.SData.Name.Contains("BasicAttack") && attack.Distance(Player) < E.Range && !attack.IsDead)
                    {
                        E.Cast(attack);
                    }
                }
            }

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

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {

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
                    ComboMenu.Add(new MenuList("wmode", "W弹射模式:",
                        new[] { "不使用", "队友和我之间", "队友和目标之间" }));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- 可击中>=", 2, 1, 5));
                    ComboMenu.Add(new MenuSlider("allyr", "^- 附近友军>=", 1, 1, 4));
                    ComboMenu.Add(new MenuKeyBind("semir", "半自动R", Keys.T, KeyBindType.Press));

                    ComboMenu.Add(new MenuBool("support", "辅助模式"));

                }

                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("misc", "自动");
                {
                    HarassMenu.Add(new MenuBool("autoq", "自动Q被控目标"));


                }

                RootMenu.Add(HarassMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("white", "W 设定");
                {
                    FarmMenu.Add(new MenuBool("enable", "自动W"));
                    FarmMenu.Add(new MenuSeparator("meow", "优先级 0为关闭"));
                    FarmMenu.Add(new MenuSeparator("meowmeow", "1 最低, 5 最高"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " 优先级: ", 1, 0, 5));
                        FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "hp", "^- 血量%: ", 50, 0,
                            100));
                    }

                }
                RootMenu.Add(FarmMenu);
                KillstealMenu = new Menu("ewhite", "E 白名单");
                {

                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        KillstealMenu.Add(new MenuBool(target.CharacterName.ToLower(), "启用E: " + target.CharacterName));


                    }
                }
                RootMenu.Add(KillstealMenu);

                
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuList("wmode", "W On Target Mode:",
                        new[] { "Never", "If Bounces to Ally / Me", "Bounce from Ally to Target" }));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X", 2, 1, 5));
                    ComboMenu.Add(new MenuSlider("allyr", "^- if X Nearby Allies", 1, 1, 4));
                    ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", Keys.T, KeyBindType.Press));

                    ComboMenu.Add(new MenuBool("support", "Support Mode"));

                }

                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("misc", "Misc.");
                {
                    HarassMenu.Add(new MenuBool("autoq", "Auto Q on CC"));


                }

                RootMenu.Add(HarassMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("white", "W Settings");
                {
                    FarmMenu.Add(new MenuBool("enable", "Use Auto W"));
                    FarmMenu.Add(new MenuSeparator("meow", "Priority 0 - Disabled"));
                    FarmMenu.Add(new MenuSeparator("meowmeow", "1 - Lowest, 5 - Biggest Priority"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "priority", target.CharacterName + " Priority: ", 1, 0, 5));
                        FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "hp", "^- Health Percent: ", 50, 0,
                            100));
                    }

                }
                RootMenu.Add(FarmMenu);
                KillstealMenu = new Menu("ewhite", "E WhiteList");
                {

                    foreach (var target in GameObjects.AllyHeroes)
                    {

                        KillstealMenu.Add(new MenuBool(target.CharacterName.ToLower(), "Enable: " + target.CharacterName));


                    }
                }
                RootMenu.Add(KillstealMenu);
            }
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 725);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 1000);
            Q.SetSkillshot(1f, 110, float.MaxValue, false, SkillshotType.Circle);
            R.SetSkillshot(0.5f, 180, 850, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}
