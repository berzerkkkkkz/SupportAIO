using System;

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
    class Rakan : Champion
    {
        private int delayyyyyyyyy;
        private int meowdelay;
        private int language;

        internal Rakan()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }


        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"];
            bool useW = RootMenu["combo"]["usew"];




            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range+W.Range);

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
            if (useW)
            {
                foreach (var en in ObjectManager.Get<AIBaseClient>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (E.Mana + W.Mana< Player.Mana)
                                    {
                                        if (en.Distance(Player) < E.Range)
                                        {
                                            if (!Player.HasBuff("rakanerecast") && meowdelay <= Variables.TickCount)
                                            {
                                                if (en.Distance(target) < Player.Distance(target))
                                                {
                                                    E.CastOnUnit(en);
                                                }
                                            }

                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {

                                                if (W.Cast(target))
                                                {
                                                    meowdelay = Variables.TickCount + 1500;
                                                }
                                            }
                                        }
                                    }

                                }
                                if (en.Distance(Player) > E.Range)
                                {
                                    if (E.Mana + W.Mana < Player.Mana)
                                    {
                                        if (target.IsValidTarget(W.Range))
                                        {

                                            if (W.Cast(target))
                                            {
                                                meowdelay = Variables.TickCount + 1500;
                                            }
                                        }
                                        if (en.Distance(Player) < E.Range)
                                        {
                                            if (!Player.HasBuff("rakanerecast") && meowdelay < Variables.TickCount)
                                            {
                                                if (en.Distance(target) < Player.Distance(target))
                                                {
                                                    E.CastOnUnit(en);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (target.Distance(en) > 580)
                            {
                                if (target.IsValidTarget(W.Range))
                                {

                                    if (W.Cast(target))
                                    {
                                        meowdelay = Variables.TickCount + 1500;
                                    }
                                }
                            }
                        }
                        else if (en.Distance(target) > W.Range + E.Range)
                        {
                            if (target.IsValidTarget(W.Range))
                            {

                                if (W.Cast(target))
                                {
                                    meowdelay = Variables.TickCount + 1500;
                                }
                            }
                        }
                    }
                }
                if (!E.IsReady())
                {
                    if (target.IsValidTarget(W.Range))
                    {

                        if (W.Cast(target))
                        {
                            meowdelay = Variables.TickCount + 1500;
                        }
                    }
                }
            }
            if (RootMenu["combo"]["user"])
            {
                if (target != null && Player.CountEnemyHeroesInRange(1000) >=
                    RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value)
                {
                    if (target.HealthPercent <= RootMenu["combo"]["hp"].GetValue<MenuSlider>().Value)
                    {
                        R.Cast();
                    }
                }
            }
        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["wflash"].GetValue<MenuKeyBind>().Active)
            {

                var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range + 410);
                var Flash = ObjectManager.Player.GetSpellSlot("SummonerFlash");
                if (W.IsReady())
                {
                    if (Flash != SpellSlot.Unknown && ObjectManager.Player.Spellbook.GetSpell(Flash).IsReady)
                    {
                        var ummm = W.GetPrediction(target);
                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                        if (target.IsValidTarget())
                        {
                            if (target.Distance(Player) > W.Range)
                            {
                                if (ObjectManager.Player.Spellbook.CastSpell(Flash,(target.Position)))
                                {
                                    W.Cast(ummm.CastPosition);
                                }



                            }
                        }
                    }
                }
            }

            if (RootMenu["combo"]["engage"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range + W.Range);

                if (!target.IsValidTarget())
                {

                    return;
                }

                foreach (var en in ObjectManager.Get<AIBaseClient>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (E.Mana + W.Mana < Player.Mana)
                                    {
                                        if (en != null && delayyyyyyyyy <= Variables.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {
                                                var pred = W.GetPrediction(target);
                                                if (pred.Hitchance >= HitChance.High)
                                                {
                                                    W.Cast(pred.CastPosition, true);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var bestAllies = GameObjects.AllyHeroes
                .Where(t =>
                    t.Distance(ObjectManager.Player) < SupportAIO.Common.Champion.E.Range)
                .OrderBy(o => o.Health);
            foreach (var ally in bestAllies)
            {
                if (E.IsReady() &&
                    ally.IsValidTarget(E.Range) &&
                    ally.CountEnemyHeroesInRange(800f) > 0 &&
                    HealthPrediction.GetPrediction(ally, 250 + Game.Ping) <=
                    ally.MaxHealth / 4)
                {
                    E.CastOnUnit(ally);
                }
            }
            if (RootMenu["flee"]["key"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                if (RootMenu["flee"]["user"])
                {
                    R.Cast();
                }
                if (RootMenu["flee"]["usee"])
                {
                    foreach (var en in ObjectManager.Get<AIBaseClient>())
                    {
                        if (!en.IsDead)
                        {
                            if (en.Distance(Game.CursorPosCenter) < 200 && en.IsAlly && !en.IsMe &&
                                en.Distance(Player) <= E.Range)
                            {
                                E.CastOnUnit(en);
                            }

                        }
                    }
                }
            }
        }


        protected override void Farming()
        {
            throw new NotImplementedException();
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
            if (RootMenu["drawings"]["drawflee"] && RootMenu["flee"]["key"].GetValue<MenuKeyBind>().Active)
            {

                Render.Circle.DrawCircle(Game.CursorPosCenter, 200, Color.Ivory);

            }
            if (RootMenu["drawings"]["drawrange"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range + W.Range, Color.CornflowerBlue);
            }
            if (RootMenu["drawings"]["wflash"])
            {
                Render.Circle.DrawCircle(Player.Position, W.Range + 410, Color.HotPink);
            }

        }

        protected override void Killsteal()
        {

        }

        internal override void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs e)
        {
            if (e.Slot == SpellSlot.E)
            {
                delayyyyyyyyy = Variables.TickCount + 300;
            }
        }

        protected override void Harass()
        {

            bool useQ = RootMenu["harass"]["useq"];
            bool useE = RootMenu["harass"]["logic"];
            var target = Extensions.GetBestEnemyHeroTargetInRange(E.Range+W.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (useE)
            {
                foreach (var en in ObjectManager.Get<AIBaseClient>())
                {
                    if (!en.IsDead)
                    {
                        if (en.Distance(Player) < E.Range + W.Range && en.IsAlly && !en.IsMe)
                        {
                            if (target != null && target.Distance(en) <= 580)
                            {
                                if (en.Distance(Player) < E.Range)
                                {
                                    if (E.Mana + W.Mana < Player.Mana)
                                    {
                                        if (en != null && delayyyyyyyyy <= Variables.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                        if (Player.HasBuff("rakanerecast"))
                                        {
                                            if (target.IsValidTarget(W.Range))
                                            {
                                                var pred = W.GetPrediction(target);
                                                if (pred.Hitchance >= HitChance.High)
                                                {
                                                    W.Cast(pred.CastPosition, true);
                                                }
                                            }
                                        }
                                        if (en.HasBuff("RakanEShield") && delayyyyyyyyy <= Variables.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                        }
                                    }
                                }

                                if (en.Distance(Player) > E.Range)
                                {

                                    if (E.Mana + W.Mana < Player.Mana)
                                    {

                                        if (target.IsValidTarget(W.Range))
                                        {
                                            var pred = W.GetPrediction(target);
                                            if (pred.Hitchance >= HitChance.High)
                                            {
                                                W.Cast(pred.CastPosition, true);
                                            }
                                        }
                                        if (en.Distance(Player) < E.Range && delayyyyyyyyy <= Variables.TickCount)
                                        {
                                            E.CastOnUnit(en);
                                            if (en.HasBuff("RakanEShield"))
                                            {
                                                E.CastOnUnit(en);
                                            }
                                        }
                                    }
                                }

                            }
                        }

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
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuSlider("hitr", "若附近敌人 >=", 2, 1, 5));
                    ComboMenu.Add(new MenuSlider("hp", "若敌人血量 %<=", 50, 1, 100));
                    ComboMenu.Add(new MenuKeyBind("engage", "突进EW连招", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuKeyBind("wflash", "W闪!", Keys.G, KeyBindType.Press));

                }
                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuSlider("mana", "蓝量管理", 40, 1, 100));
                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("logic", "使用 E - W - E 骚扰逻辑"));

                }
                RootMenu.Add(HarassMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawflee", "显示逃跑半径"));
                    DrawMenu.Add(new MenuBool("drawrange", "显示EW突进距离"));
                    DrawMenu.Add(new MenuBool("wflash", "显示W闪距离"));
                }
                FarmMenu = new Menu("flee", "逃跑");
                {
                    FarmMenu.Add(new MenuKeyBind("key", "逃跑按键", Keys.Z, KeyBindType.Press));
                    FarmMenu.Add(new MenuBool("user", "使用 R", false));
                    FarmMenu.Add(new MenuBool("usee", "使用 E"));

                }
                RootMenu.Add(DrawMenu);
                RootMenu.Add(FarmMenu);
                EvadeMenu = new Menu("wset", "套盾逻辑");
                {
                    var First = new Menu("first", "技能列表");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);

                    EvadeMenu.Add(First);

                }

                RootMenu.Add(EvadeMenu);

            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuSlider("hitr", "If X Near Enemies", 2, 1, 5));
                    ComboMenu.Add(new MenuSlider("hp", "If Enemy X Health", 50, 1, 100));
                    ComboMenu.Add(new MenuKeyBind("engage", "Engage E - W Combo", Keys.T, KeyBindType.Press));
                    ComboMenu.Add(new MenuKeyBind("wflash", "W - Flash", Keys.G, KeyBindType.Press));

                }
                RootMenu.Add(ComboMenu);
                HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuSlider("mana", "Mana Manager", 40, 1, 100));
                    HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    HarassMenu.Add(new MenuBool("logic", "Use E - W - E Harass Logic"));

                }
                RootMenu.Add(HarassMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawflee", "Draw Flee Radius"));
                    DrawMenu.Add(new MenuBool("drawrange", "Draw Engage Range"));
                    DrawMenu.Add(new MenuBool("wflash", "Draw W-Flash"));
                }
                FarmMenu = new Menu("flee", "Flee");
                {
                    FarmMenu.Add(new MenuKeyBind("key", "Flee Key", Keys.Z, KeyBindType.Press));
                    FarmMenu.Add(new MenuBool("user", "Flee with R", false));
                    FarmMenu.Add(new MenuBool("usee", "Flee with E"));

                }
                RootMenu.Add(DrawMenu);
                RootMenu.Add(FarmMenu);
                EvadeMenu = new Menu("wset", "Shielding");
                {
                    var First = new Menu("first", "Spells Detector");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);

                    EvadeMenu.Add(First);

                }

                RootMenu.Add(EvadeMenu);
            }
            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 900);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 550);
            R = new Spell(SpellSlot.R, 0);
            Q.SetSkillshot(0.25f, 60, 1800, true, SkillshotType.Line, false, HitChance.None);
            W.SetSkillshot(0.25f, 50, 1800, false, SkillshotType.Circle, false, HitChance.None);

        }
    }
}
