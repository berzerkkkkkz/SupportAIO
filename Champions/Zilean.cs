
using System.Linq;

using EnsoulSharp;

using Extensions = SupportAIO.Common.Extensions;
using Color = System.Drawing.Color;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;
using Spell = EnsoulSharp.SDK.Spell;
using Champion = SupportAIO.Common.Champion;
using System.Windows.Forms;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.Prediction;
using EnsoulSharp.SDK.Utility;

namespace SupportAIO.Champions
{
    class Zilean : Champion
    {
        private int language;

        internal Zilean()
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

            bool useW = RootMenu["combo"]["usew"];
            bool useE = RootMenu["combo"]["usee"];

            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
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
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
            }
            if (useW && target.IsValidTarget(Q.Range) && !Q.IsReady() && Player.Mana >= Q.Mana+W.Mana)
            {
                if (target != null)
                {
                    W.Cast();
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
            if (RootMenu["combo"]["rusage"].GetValue<MenuList>().Index == 1)
            {
                foreach (var en in GameObjects.AllyHeroes)
                {
                    if (en != null && en.IsValidTarget(R.Range, true) && en.Distance(Player) < R.Range)
                    {
                            if (en.HealthPercent <= RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value && !en.IsRecalling() && !Player.IsRecalling() && en.CountEnemyHeroesInRange(R.Range) > 0)
                            {
                                R.CastOnUnit(en);
                            }
                        
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

                    Q.Cast(target);
                }

            }
            if (RootMenu["flee"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                E.CastOnUnit(Player);
            }
            if (RootMenu["qwq"].GetValue<MenuKeyBind>().Active)
            {
                var pos = (Game.CursorPosCenter - Player.Position).Normalized();
                if (Player.Distance(Game.CursorPosCenter) < 800)
                {
                    Q.Cast(Game.CursorPosCenter);
                }
                if (Player.Distance(Game.CursorPosCenter) > 800)
                {
                    Q.Cast(Player.Position + pos * 800);
                }
                if (!Q.IsReady() && Player.Mana >= Q.Mana + W.Mana)
                {
                    W.Cast();
                }
                if (Player.Distance(Game.CursorPosCenter) < 800)
                {
                    Q.Cast(Game.CursorPosCenter);
                }
                if (Player.Distance(Game.CursorPosCenter) > 800)
                {
                    Q.Cast(Player.Position + pos * 800);
                }
            }
            if (RootMenu["combo"]["slow"])
            {
                if (Player.HasBuffOfType(BuffType.Slow))
                {
                    E.CastOnUnit(Player);
                }
                foreach (var en in GameObjects.AllyHeroes)
                {
                    if (!en.IsDead && en.Distance(Player) < E.Range && en.HasBuffOfType(BuffType.Slow))
                    {
                        E.CastOnUnit(en);
                    }
                }
            }
        }

        protected override void Farming()
        {
            foreach (var minion in SupportAIO.Common.Extensions.GetEnemyLaneMinionsTargetsInRange(Q.Range))
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                bool useQ = RootMenu["farming"]["useq"];
                bool useW = RootMenu["farming"]["usew"];



                if (minion.IsValidTarget(Q.Range) && minion != null && useQ)
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false, 
                            minion.Position)) >= RootMenu["farming"]["hitq"].GetValue<MenuSlider>().Value)
                    {
                        Q.Cast(minion);
                    }
                }
                if (minion.IsValidTarget(Q.Range) && minion != null && useW)
                {
                    if (GameObjects.EnemyMinions.Count(h => h.IsValidTarget(300, false,
                            minion.Position)) >= RootMenu["farming"]["hitq"].GetValue<MenuSlider>().Value)
                    {
                        if (!Q.IsReady() && Player.Mana >= Q.Mana + W.Mana)
                        {
                            W.Cast();
                        }
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
            if (Player.ManaPercent >= RootMenu["harass"]["mana"].GetValue<MenuSlider>().Value)
            {
                bool useQ = RootMenu["harass"]["useq"];

                bool useW = RootMenu["harass"]["usew"];

                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

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
                if (useW && target.IsValidTarget(Q.Range))
                {
                    if (target != null && !Q.IsReady() && Player.Mana >= Q.Mana + W.Mana)
                    {
                        W.Cast();
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
                    ComboMenu.Add(new MenuBool("usew", "使用 W 重置Q"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("slow", "自动E被减速队友"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuList("rusage", "R 模式", new[] { "受到致命伤害", "低血量" }));
                    ComboMenu.Add(new MenuSlider("hitr", "若血量% <= (低血量模式)", 20, 1, 100));

                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                }
                RootMenu.Add(ComboMenu);


                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuSlider("mana", "蓝量控制", 50, 1, 100));

                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("usew", "使用 W 重置Q"));
                }
                RootMenu.Add(HarassMenu);

                KillstealMenu = new Menu("misc", "自动");
                {
                    KillstealMenu.Add(new MenuBool("autoq", "自动Q被控目标"));
                }
                RootMenu.Add(KillstealMenu);

                FarmMenu = new Menu("farming", "清线");
                {
                    FarmMenu.Add(new MenuBool("useq", "使用 Q"));
                    FarmMenu.Add(new MenuSlider("hitq", "^- 可击中小兵>=", 3, 1, 6));
                    FarmMenu.Add(new MenuBool("usew", "使用 W 重置Q"));
                }
                RootMenu.Add(FarmMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 范围"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 范围"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 范围"));

                }
                RootMenu.Add(DrawMenu);

                RootMenu.Add(new MenuKeyBind("qwq", "QWQ 到鼠标", Keys.G, KeyBindType.Press));
                RootMenu.Add(new MenuKeyBind("flee", "逃跑按键", Keys.Z, KeyBindType.Press));
            }

            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W for Q Reset"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("slow", "Use Auto E on Slowed Ally"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuList("rusage", "R Usage", new[] { "If Incoming Damage Kills", "At X Health" }));
                    ComboMenu.Add(new MenuSlider("hitr", "If X Health <= (Health Mode)", 20, 1, 100));

                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                }
                RootMenu.Add(ComboMenu);


                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuSlider("mana", "Mana Percent", 50, 1, 100));

                    HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    HarassMenu.Add(new MenuBool("usew", "Use W for Q Reset"));
                }
                RootMenu.Add(HarassMenu);

                KillstealMenu = new Menu("misc", "Misc.");
                {
                    KillstealMenu.Add(new MenuBool("autoq", "Auto Q on CC"));
                }
                RootMenu.Add(KillstealMenu);

                FarmMenu = new Menu("farming", "Farming");
                {
                    FarmMenu.Add(new MenuBool("useq", "Use Q to Farm"));
                    FarmMenu.Add(new MenuSlider("hitq", "^- if hits X", 3, 1, 6));
                    FarmMenu.Add(new MenuBool("usew", "Use W to Reset Q"));
                }
                RootMenu.Add(FarmMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

                }
                RootMenu.Add(DrawMenu);
                RootMenu.Add(new MenuKeyBind("qwq", "Q-W-Q to Mouse", Keys.T, KeyBindType.Press));
                RootMenu.Add(new MenuKeyBind("flee", "Flee Key", Keys.Z, KeyBindType.Press));
            }
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 900);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 550);
            R = new Spell(SpellSlot.R, 900);
            Q.SetSkillshot(0.75f, 105f, int.MaxValue, false, SkillshotType.Circle, false, HitChance.None);
        }
    }
}
