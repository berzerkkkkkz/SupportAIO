

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
    class Leona : Champion
    {
        private int language;

        internal Leona()
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


            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            
            if (target.IsValidTarget(E.Range) && useE && RootMenu["whitelist"][target.CharacterName.ToLower()])
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
            if (target.IsValidTarget(300) && useQ)
            {

                if (target != null)
                {
                    if (RootMenu["combo"]["eq"])
                    {
                        if (target.HasBuff("leonazenithbladeroot"))
                        {

                            Q.Cast();
                        }
                    }
                    if (!RootMenu["combo"]["eq"])
                    {

                        Q.Cast();

                    }
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.Cast();
                }
            }


            if (useR)
            {



                if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value > 1)
                {
                    if (target != null &&
                        target.CountEnemyHeroesInRange(300) >= RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value &&
                        target.IsValidTarget(R.Range))
                    {
                        var pred = R.GetPrediction(target);
                        if (pred.Hitchance >= HitChance.High)
                        {
                            R.Cast(pred.CastPosition, true);
                        }
                    }
                }
                if (RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value == 1)
                {
                    if (target != null &&

                        target.IsValidTarget(R.Range))
                    {
                        var pred = R.GetPrediction(target);
                        if (pred.Hitchance >= HitChance.High)
                        {
                            R.Cast(pred.CastPosition, true);
                        }
                    }
                }


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


                if (target != null &&
                    target.IsValidTarget(R.Range))
                {
                    var pred = R.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        R.Cast(pred.CastPosition, true);
                    }
                }
            }
        }

        
    

        protected override void Farming()
        {
        }

        protected override void Drawings()
        {
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Crimson);
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
            bool useQ = RootMenu["harass"]["useq"];
            bool useW = RootMenu["harass"]["usew"];
            bool useE = RootMenu["harass"]["usee"];



            var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range);

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
            if (target.IsValidTarget(1000) && useQ)
            {

                if (target != null)
                {
                    if (RootMenu["harass"]["eq"])
                    {
                        if (target.HasBuff("leonazenithbladeroot"))
                        {

                            Q.Cast();
                        }
                    }
                    if (!RootMenu["harass"]["eq"])
                    {

                        Q.Cast();

                    }
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.Cast();
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
                    ComboMenu.Add(new MenuBool("eq", "^- 仅当E中了"));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- 可击中>=", 2, 1, 5));
                    ComboMenu.Add(new MenuKeyBind("semir", "半自动 R", Keys.T, KeyBindType.Press));


                }
                RootMenu.Add(ComboMenu);
                var HarassMenu = new Menu("harass", "骚扰");
                {
                    HarassMenu.Add(new MenuBool("useq", "使用 Q"));
                    HarassMenu.Add(new MenuBool("eq", "^- 仅当E中了"));
                    HarassMenu.Add(new MenuBool("usew", "使用 W"));
                    HarassMenu.Add(new MenuBool("usee", "使用 E"));


                }
                RootMenu.Add(HarassMenu);
                WhiteList = new Menu("whitelist", "E 白名单");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "启用: " + target.CharacterName));
                    }
                }
                RootMenu.Add(WhiteList);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                }
                RootMenu.Add(DrawMenu);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("eq", "Use Q only if E Hits"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- if Hits", 2, 1, 5));
                    ComboMenu.Add(new MenuKeyBind("semir", "Semi-R Key", Keys.T, KeyBindType.Press));


                }
                RootMenu.Add(ComboMenu);
                var HarassMenu = new Menu("harass", "Harass");
                {
                    HarassMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    HarassMenu.Add(new MenuBool("eq", "Use Q only if E Hits"));
                    HarassMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    HarassMenu.Add(new MenuBool("usee", "Use E in Combo"));


                }
                RootMenu.Add(HarassMenu);
                WhiteList = new Menu("whitelist", "E Whitelist");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "Enable: " + target.CharacterName));
                    }
                }
                RootMenu.Add(WhiteList);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

                }
                RootMenu.Add(DrawMenu);
            }

            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 0);
            W = new Spell(SpellSlot.W, 275);
            E = new Spell(SpellSlot.E, 875);
            R = new Spell(SpellSlot.R, 1200);

            E.SetSkillshot(0.25f, 20, 2400, false, SkillshotType.Line, false, HitChance.None);
            R.SetSkillshot(1.3f, 300, float.MaxValue, false, SkillshotType.Circle);
        }
    }
}
