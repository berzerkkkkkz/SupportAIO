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
    class Soraka : Champion
    {
        private int language;

        internal Soraka()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {
       
                if (target != null && Args.EndPosition.Distance(Player) < E.Range)
                {
                    E.Cast(Args.EndPosition);
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
   
            bool useE = RootMenu["combo"]["usee"];
        
            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (target.IsValidTarget(E.Range) && useE)
            {

                if (target != null)
                {
                    var pred = E.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        E.Cast(pred.CastPosition, true);
                    }
                }
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

        }

        protected override void SemiR()
        {
            if (RootMenu["heal"]["autor"])
            {
                foreach (var ally in GameObjects.AllyHeroes.Where(
                    x => x.IsAlly && !x.IsRecalling() &&
                         x.HealthPercent <=
                         RootMenu["heal"]["rhealth"].GetValue<MenuSlider>().Value && x.CountEnemyHeroesInRange(3000) > 0 &&
                         !RootMenu["black"][x.CharacterName.ToLower()]))
                {
                    if (ally != null && !ally.IsDead)
                    {
                        R.Cast();
                    }
                }

            }
            if (RootMenu["heal"]["autow"])
            {



                switch (RootMenu["heal"]["mode"].GetValue<MenuList>().Index)
                {
                    case 0:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent >=
                                     RootMenu["heal"]["me"].GetValue<MenuSlider>().Value && x.HealthPercent <=
                                     RootMenu["heal"]["ally"].GetValue<MenuSlider>().Value &&
                                     RootMenu["white"][x.CharacterName.ToLower()])
                            .OrderByDescending(x => x.TotalAttackDamage))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 1:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent >=
                                     RootMenu["heal"]["me"].GetValue<MenuSlider>().Value && x.HealthPercent <=
                                     RootMenu["heal"]["ally"].GetValue<MenuSlider>().Value &&
                                     RootMenu["white"][x.CharacterName.ToLower()])
                            .OrderByDescending(x => x.TotalMagicalDamage))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 2:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent >=
                                     RootMenu["heal"]["me"].GetValue<MenuSlider>().Value && x.HealthPercent <=
                                     RootMenu["heal"]["ally"].GetValue<MenuSlider>().Value &&
                                     RootMenu["white"][x.CharacterName.ToLower()])
                            .OrderBy(x => x.Health))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 3:

                        foreach (var ally in GameObjects.AllyHeroes.Where(
                                x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                     Player.HealthPercent >=
                                     RootMenu["heal"]["me"].GetValue<MenuSlider>().Value && x.HealthPercent <=
                                     RootMenu["heal"]["ally"].GetValue<MenuSlider>().Value &&
                                     RootMenu["white"][x.CharacterName.ToLower()])
                            .OrderByDescending(x => x.MaxHealth))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;
                    case 4:
                        foreach (var ally in GameObjects.AllyHeroes.Where(
                            x => x.Distance(Player) <= W.Range && x.IsAlly && !x.IsMe && !x.IsRecalling() &&
                                 Player.HealthPercent >=
                                 RootMenu["heal"]["me"].GetValue<MenuSlider>().Value && x.HealthPercent <=
                                 RootMenu["white"][x.CharacterName.ToLower() + "hp"].GetValue<MenuSlider>().Value &&
                                 RootMenu["white"][x.CharacterName.ToLower()]))
                        {
                            if (ally != null && !ally.IsDead)
                            {
                                W.CastOnUnit(ally);
                            }
                        }
                        break;



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
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.Yellow);
            }
      

        }

        protected override void Killsteal()
        {
            
        }

        protected override void Harass()
        {
            if (RootMenu["harass"]["toggle"].GetValue<MenuKeyBind>().Active)
            {
                bool useQ = RootMenu["harass"]["useq"];

                bool useE = RootMenu["harass"]["usee"];

                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                if (target.IsValidTarget(E.Range) && useE)
                {

                    if (target != null)
                    {
                        var pred = E.GetPrediction(target);
                        if (pred.Hitchance >= HitChance.High)
                        {
                            E.Cast(pred.CastPosition, true);
                        }
                    }
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
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                }
                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuKeyBind("toggle", "自动骚扰", Keys.T, KeyBindType.Toggle));
                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("usee", "使用 E"));

                }

                RootMenu.Add(ComboMenu);
                RootMenu.Add(HarassMenu); KillstealMenu = new Menu("misc", "反突进");
                {
                    KillstealMenu.Add(new MenuBool("antigapq", "反突进 E"));


                }

                RootMenu.Add(KillstealMenu);
                WhiteList = new Menu("heal", "W设定");
                {
                    WhiteList.Add(new MenuBool("autow", "启用W"));
                    WhiteList.Add(new MenuList("mode", "W优先级", new[] { "AD优先", "AP优先", "低血量百分比优先", "低血量数值优先", "白名单优先" }));
                    WhiteList.Add(new MenuSlider("ally", "队友血量 %<=", 50));
                    WhiteList.Add(new MenuSlider("me", "禁用W若自身血量 %<=", 30));
                    WhiteList.Add(new MenuBool("autor", "启用R"));
                    WhiteList.Add(new MenuSlider("rhealth", "队友血量 %<=", 20));
                }
                RootMenu.Add(WhiteList);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("white", "W 白名单");
                {
                    FarmMenu.Add(new MenuSeparator("meow", "血量百分比模式下，仅W白名单内的队友"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        if (!target.IsMe)
                        {
                            FarmMenu.Add(new MenuBool(target.CharacterName.ToLower(), "启用: " + target.CharacterName));
                            FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "hp", "^- 血量百分比: ", 100, 0, 100));
                        }
                    }
                }
                RootMenu.Add(FarmMenu);
                WhiteList = new Menu("black", "R 黑名单");
                {
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "不R: " + target.CharacterName, false));
                    }
                }
                RootMenu.Add(WhiteList);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                }
                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuKeyBind("toggle", "Toggle Key", Keys.T, KeyBindType.Toggle));
                    HarassMenu.Add(new MenuBool("useq", "Use Q in Harass"));
                    HarassMenu.Add(new MenuBool("usee", "Use E in Harass"));

                }

                RootMenu.Add(ComboMenu);
                RootMenu.Add(HarassMenu); KillstealMenu = new Menu("misc", "Misc.");
                {
                    KillstealMenu.Add(new MenuBool("antigapq", "Anti-Gap E"));


                }

                RootMenu.Add(KillstealMenu);
                WhiteList = new Menu("heal", "Healing");
                {
                    WhiteList.Add(new MenuBool("autow", "Enable W Healing"));
                    WhiteList.Add(new MenuList("mode", "Healing Priority", new[] { "Most AD", "Most AP", "Least Health", "Least Health (Squishies)", "Whitelist" }));
                    WhiteList.Add(new MenuSlider("ally", "Ally Health Percent <=", 50));
                    WhiteList.Add(new MenuSlider("me", "Don't W if my Health <=", 30));
                    WhiteList.Add(new MenuBool("autor", "Enable R Healing"));
                    WhiteList.Add(new MenuBool("semi", "^- Semi Manual  R", false));
                    WhiteList.Add(new MenuSlider("rhealth", "Ally R Health <=", 20));
                }
                RootMenu.Add(WhiteList);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("toggle", "Draw Toggle"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("white", "W White List");
                {
                    FarmMenu.Add(new MenuSeparator("meow", "Health Percent only works if Whitelist Mode"));
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        if (!target.IsMe)
                        {
                            FarmMenu.Add(new MenuBool(target.CharacterName.ToLower(), "Enable: " + target.CharacterName));
                            FarmMenu.Add(new MenuSlider(target.CharacterName.ToLower() + "hp", "^- Health Percent: ", 100, 0, 100));
                        }
                    }
                }
                RootMenu.Add(FarmMenu);
                WhiteList = new Menu("black", "R Black List");
                {
                    foreach (var target in GameObjects.AllyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "Block: " + target.CharacterName, false));
                    }
                }
                RootMenu.Add(WhiteList);
            }
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 800);
            W = new Spell(SpellSlot.W, 550);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.5f, 80, 1750f, false, SkillshotType.Circle, false, HitChance.None);
            E.SetSkillshot(0.5f, 50, 1750f, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}
