using System;
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
    class Karma : Champion
    {
        private int language;

        internal Karma()
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



            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            switch (RootMenu["combo"]["combomode"].GetValue<MenuList>().Index)
            {
                case 0:
                    if (R.IsReady() && Q.IsReady())
                    {
                        if (target.IsValidTarget(Q.Range) && useQ)
                        {

                            if (target != null)
                            {
                                var pred = Q.GetPrediction(target);
                                if (pred.Hitchance >= HitChance.High)
                                {
                                    R.Cast();
                                    DelayAction.Add(200, () =>
                                    {
                                        Q.Cast(pred.CastPosition, true);
                                    });
                                }
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useE && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }
                    break;
                case 1:
                    if (R.IsReady() && W.IsReady())
                    {
                        if (target.IsValidTarget(W.Range) && useQ)
                        {

                            if (target != null)
                            {
                                R.Cast();
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW)
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    if ((!R.IsReady() || !W.IsReady()) && useQ && Q.IsReady() && !Player.HasBuff("KarmaMantra"))
                    {
                        if (target.IsValidTarget(Q.Range))
                        {
                            var pred = Q.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                Q.Cast(pred.CastPosition, true);
                            }
                        }
                    }


                    if (target.IsValidTarget(W.Range) && useE && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }
                    break;
                case 2:
                    if (R.IsReady() && E.IsReady())
                    {
                        if (target.IsValidTarget(Q.Range - 50) && useE)
                        {

                            if (target != null)
                            {
                                R.Cast();
                            }
                        }
                    }

                    if (target.IsValidTarget(W.Range) && useE)
                    {

                        if (target != null)
                        {
                            E.Cast();
                        }
                    }

                    if (useQ && Q.IsReady() && !Player.HasBuff("KarmaMantra"))
                    {
                        if (target.IsValidTarget(Q.Range))
                        {
                            var pred = Q.GetPrediction(target);
                            if (pred.Hitchance >= HitChance.High)
                            {
                                Q.Cast(pred.CastPosition, true);
                            }
                        }
                    }
                    if (target.IsValidTarget(W.Range) && useW && !Player.HasBuff("KarmaMantra"))
                    {

                        if (target != null)
                        {
                            W.CastOnUnit(target);
                        }
                    }
                    break;
            }
        }


        protected override void SemiR()
        {
            if (RootMenu["rq"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                if (R.Cast())
                {
                    Q.Cast(Game.CursorPosCenter);
                }
            }
            if (RootMenu["combo"]["save"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                if (R.IsReady() && target.Distance(Player) < W.Range)
                {
                    R.Cast();
                }
                if (W.IsReady() && target.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
                if (E.IsReady() && target.Distance(Player) < W.Range && !Player.HasBuff("KarmaMantra"))
                {
                    E.Cast();
                }
                if (Q.IsReady() && target.Distance(Player) < Q.Range && !Player.HasBuff("KarmaMantra"))
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
            }
            if (RootMenu["combo"]["chase"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }
                if (E.IsReady() && target.Distance(Player) < W.Range && !Player.HasBuff("KarmaMantra"))
                {
                    E.Cast();
                }
                if (W.IsReady() && target.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
                if (R.IsReady() && target.Distance(Player) < Q.Range)
                {
                    R.Cast();
                }
                if (Q.IsReady() && target.Distance(Player) < Q.Range && Player.HasBuff("KarmaMantra"))
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
                if (Q.IsReady() && target.Distance(Player) < Q.Range && !Player.HasBuff("KarmaMantra") && !R.IsReady())
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
            }
        }


        protected override void Farming()
        {

            foreach (var minion in GameObjects.Jungle.Where(m => m.IsValidTarget(Q.Range)).ToList())
            {
                if (!minion.IsValidTarget())
                {
                    return;
                }
                bool useQ = RootMenu["farming"]["useq"];
                bool useW = RootMenu["farming"]["usew"];



                if (minion.IsValidTarget(Q.Range) && minion != null && useQ)
                {
                    Q.Cast(minion);
                }
                if (minion.IsValidTarget(W.Range) && minion != null && useW)
                {
                    W.CastOnUnit(minion);
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
            

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {
            throw new NotImplementedException();
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
                    ComboMenu.Add(new MenuList("combomode", "连招模式", new[] { "R - Q", "R - W", "R - E" }));
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                    ComboMenu.Add(new MenuKeyBind("chase", "追击按键", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuKeyBind("save", "逃跑按键", Keys.Z, KeyBindType.Press));

                }
                RootMenu.Add(ComboMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 范围"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 范围"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 范围"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("farming", "清野");
                {
                    FarmMenu.Add(new MenuBool("useq", "使用Q"));
                    FarmMenu.Add(new MenuBool("usew", "使用W"));
                }
                RootMenu.Add(FarmMenu);

                EvadeMenu = new Menu("wset", "护盾设置");
                {
                    var First = new Menu("first", "技能列表");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);

                    EvadeMenu.Add(First);


                }
                RootMenu.Add(new MenuKeyBind("rq", "RQ 到鼠标位置", Keys.G, KeyBindType.Press));
                RootMenu.Add(EvadeMenu);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuList("combomode", "Combo Mode", new[] { "R - Q", "R - W", "R - E" }));
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                    ComboMenu.Add(new MenuKeyBind("chase", "Chase Combo", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuKeyBind("save", "Survive Combo", Keys.Z, KeyBindType.Press));

                }
                RootMenu.Add(ComboMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                }
                RootMenu.Add(DrawMenu);
                FarmMenu = new Menu("farming", "Farming");
                {
                    FarmMenu.Add(new MenuBool("useq", "Use Q in Jungle"));
                    FarmMenu.Add(new MenuBool("usew", "Use W in Jungle"));
                }
                RootMenu.Add(FarmMenu);

                EvadeMenu = new Menu("wset", "Shielding");
                {
                    var First = new Menu("first", "Spells Detector");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);

                    EvadeMenu.Add(First);

                }
                RootMenu.Add(new MenuKeyBind("rq", "RQ To Mouse", Keys.G, KeyBindType.Press));
                RootMenu.Add(EvadeMenu);

            }
            RootMenu.Attach();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {
      
                if (target != null && Args.EndPosition.Distance(Player) < W.Range)
                {
                    W.CastOnUnit(target);
                }
            
        }
        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 950);
            W = new Spell(SpellSlot.W, 675);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 60, 1700, true, SkillshotType.Line);
        }
    }
}
