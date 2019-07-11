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
    class Zyra : Champion
    {
        private int language;

        internal Zyra()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
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
            bool useR = RootMenu["combo"]["user"];
            bool useW = RootMenu["combo"]["usew"];
            bool useE = RootMenu["combo"]["usee"];

            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

            if (!target.IsValidTarget())
            {

                return;
            }
            switch (RootMenu["combo"]["mode"].GetValue<MenuList>().Index)
            {
                case 0:
                {
                    if (target.IsValidTarget(W.Range) && useW && (Q.IsReady() || E.IsReady()))
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
                                var pred = Q.GetPrediction(target);
                                if (pred.Hitchance >= HitChance.High)
                                {
                                    if (Q.Cast(pred.CastPosition, true))
                                    {
                                        if (target.IsValidTarget(W.Range) && useW)
                                        {
                                            pred = W.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                W.Cast(pred.CastPosition, true);
                                            }
                                        }
                                    }
                                }                                                                                  
                        }
                    }
                    if (target.IsValidTarget(R.Range) && target != null && useR)
                    {
                        switch (RootMenu["combo"]["rusage"].GetValue<MenuList>().Index)
                        {
                            case 0:
                                if (RootMenu["combo"]["rhit"].GetValue<MenuSlider>().Value <=
                                    target.CountEnemyHeroesInRange(500))
                                {

                                    R.Cast(target);
                                }
                                break;

                            case 1:
                                if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.Cast(target);
                                }
                                break;

                        }
                    }
                    if (target.IsValidTarget(E.Range) && useE)
                    {
                        if (target != null)
                        {
                            if (E.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }

                }
                    break;
                case 1:
                {
                    if (target.IsValidTarget(W.Range) && useW && (Q.IsReady() || E.IsReady()))
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
                            if (E.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }
                        if (target.IsValidTarget(Q.Range) && useQ)
                    {

                        if (target != null)
                        {
                            if (Q.Cast(target))
                            {
                                if (target.IsValidTarget(W.Range) && useW)
                                {
                                    W.Cast(target);
                                }
                            }
                        }
                    }
                    if (target.IsValidTarget(R.Range) && target != null && useR)
                    {
                        switch (RootMenu["combo"]["rusage"].GetValue<MenuList>().Index)
                        {
                            case 0:
                                if (RootMenu["combo"]["rhit"].GetValue<MenuSlider>().Value <=
                                    target.CountEnemyHeroesInRange(500))
                                {

                                    R.Cast(target);
                                }
                                break;

                            case 1:
                                if (target.Health <= Player.GetSpellDamage(target, SpellSlot.Q) +
                                    Player.GetSpellDamage(target, SpellSlot.W) +
                                    Player.GetSpellDamage(target, SpellSlot.R))
                                {
                                    R.Cast(target);
                                }
                                break;

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
                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                if (target.IsValidTarget(R.Range) && target != null)
                {


                    R.Cast(target);


                }

            }
            if (RootMenu["misc"]["autoe"])
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t => (t.HasBuffOfType(BuffType.Charm) || t.HasBuffOfType(BuffType.Stun) ||
                          t.HasBuffOfType(BuffType.Fear) || t.HasBuffOfType(BuffType.Snare) ||
                          t.HasBuffOfType(BuffType.Taunt) || t.HasBuffOfType(BuffType.Knockback) ||
                          t.HasBuffOfType(BuffType.Suppression)) && t.IsValidTarget(E.Range)))
                {

                    E.Cast(target);
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
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["draww"])
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color.Crimson);
            }
            if (RootMenu["drawings"]["drawe"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range, Color.Wheat);
            }
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Crimson);
            }
           
            if (RootMenu["drawings"]["drawseed"])
            {
                foreach (var seed in GameObjects.AllGameObjects)
                {
                    if (seed.Name.Contains("Zyra_Base_W_Seed") && !seed.IsDead && seed.IsValid &&
                        seed.Distance(Player) < 2000 && seed != null)
                    {
                        Render.Circle.DrawCircle(seed.Position, 100, Color.Wheat);
                    }
                }
            }
        }

        protected override void Killsteal()
        {
            
        }

        protected override void Harass()
        {
            if (Player.ManaPercent >= RootMenu["harass"]["mana"].GetValue<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"];
                bool useW = RootMenu["harass"]["usew"];
                bool useE = RootMenu["harass"]["usee"];

                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                if (target.IsValidTarget(W.Range) && useW && (Q.IsReady() || E.IsReady()))
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
                        if (Q.Cast(target))
                        {
                            if (target.IsValidTarget(W.Range) && useW)
                            {
                                W.Cast(target);
                            }
                        }
                    }
                }
                if (target.IsValidTarget(E.Range) && useE)
                {
                    if (target != null)
                    {
                        if (E.Cast(target))
                        {
                            if (target.IsValidTarget(W.Range) && useW)
                            {
                                W.Cast(target);
                            }
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
                    ComboMenu.Add(new MenuList("mode", "连招模式", new[] { "Q > W > R > E > W", "E > W > R > Q > W" }));
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuList("rusage", "R 模式", new[] { "可击中多个英雄", "连招可击杀" }));
                    ComboMenu.Add(new MenuSlider("rhit", "若可击中 >= (可击中多个英雄模式)", 2, 1, 5));
                    ComboMenu.Add(new MenuKeyBind("semir", "半自动R", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                }
                RootMenu.Add(ComboMenu);


                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuSlider("mana", "蓝量控制", 50, 1, 100));
                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("usew", "使用 W"));
                    HarassMenu.Add(new MenuBool("usee", "使用 E"));
                }
                RootMenu.Add(HarassMenu);

                KillstealMenu = new Menu("misc", "自动");
                {
                    KillstealMenu.Add(new MenuBool("autoe", "自动E被控目标"));
                }
                RootMenu.Add(KillstealMenu);

                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                    DrawMenu.Add(new MenuBool("drawseed", "显示种子位置"));
                }
                RootMenu.Add(DrawMenu);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuList("mode", "Combo Usage", new[] { "Q > W > R > E > W", "E > W > R > Q > W" }));
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuList("rusage", "R Usage", new[] { "If Hits X Enemies", "If Killable with Combo" }));
                    ComboMenu.Add(new MenuSlider("rhit", "If X Enemies <= (If Hits X Enemies Mode)", 2, 1, 5));
                    ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                }
                RootMenu.Add(ComboMenu);


                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));
                    HarassMenu.Add(new MenuBool("useq", "Use Q to Harass"));
                    HarassMenu.Add(new MenuBool("usew", "Use W to Harass"));
                    HarassMenu.Add(new MenuBool("usee", "Use E to Harass"));
                }
                RootMenu.Add(HarassMenu);

                KillstealMenu = new Menu("misc", "Misc.");
                {
                    KillstealMenu.Add(new MenuBool("autoe", "Auto E on CC"));
                }
                RootMenu.Add(KillstealMenu);

                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                    DrawMenu.Add(new MenuBool("drawseed", "Draw Seeds"));
                }
                RootMenu.Add(DrawMenu);

            }
            RootMenu.Attach();
        }
        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {

                if (target != null && Args.EndPosition.Distance(Player) < E.Range)
                {
                    E.Cast(Args.EndPosition);
                }
            
        }
        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 800);
            W = new Spell(SpellSlot.W, 850);
            E = new Spell(SpellSlot.E, 1000);
            R = new Spell(SpellSlot.R, 700);
            Q.SetSkillshot(0.85f, 140f, 3000, false, SkillshotType.Line, false, HitChance.None);
            W.SetSkillshot(0.25f, 150, 1000f, false, SkillshotType.Circle, false, HitChance.None);
            E.SetSkillshot(0.25f, 60, 1300f,  false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(0.3f, 60, 1000f, false, SkillshotType.Circle);
        }
    }
}
