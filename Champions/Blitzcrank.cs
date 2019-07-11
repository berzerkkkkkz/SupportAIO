
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
    class Blitzcrank : Champion
    {
        private int language;

        internal Blitzcrank()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"];
            bool useE = RootMenu["combo"]["usee"];
            bool useR = RootMenu["combo"]["user"];
            float enemies = RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value;
            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (Q.IsReady() && useQ && target.IsValidTarget(Q.Range))
            {

                if (target != null && !RootMenu["black"][target.CharacterName.ToLower()])
                {
                    var pred = Q.GetPrediction(target);
                    if (pred.Hitchance >= HitChance.High)
                    {
                        Q.Cast(pred.CastPosition, true);
                    }
                }
            }
            if (E.IsReady() && useE)
            {

                if (target != null)
                {
                    if (!RootMenu["combo"]["eq"] && target.IsValidTarget(E.Range))
                    {
                        E.Cast();
                    }
                    if (RootMenu["combo"]["eq"])
                    {
                        if (target.HasBuff("rocketgrab2"))
                        {
                            E.Cast();
                        }
                    }
                }
            }

            if (R.IsReady() && useR && target.IsValidTarget(R.Range))
            {

                if (target != null && enemies <= Player.CountEnemyHeroesInRange(R.Range))
                {
                    R.Cast();
                }
            }
        }

        protected override void SemiR()
        {

            if (RootMenu["qset"]["autoq"])
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {
                    return;
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
                {

                    if (target != null)
                    {
                        var pred = Q.GetPrediction(target);
                        if (pred.Hitchance == HitChance.Immobile)
                        {

                            Q.Cast(pred.CastPosition);

                        }
                    }
                }
            }
            if (RootMenu["qset"]["grabq"].GetValue<MenuKeyBind>().Active)
            {
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

                if (!target.IsValidTarget())
                {
                    return;
                }

                if (Q.IsReady() && target.IsValidTarget(Q.Range))
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

        protected override void Farming()
        {
            
        }

        protected override void Drawings()
        {
            if (RootMenu["drawings"]["qmin"])
            {
                Render.Circle.DrawCircle(Player.Position, RootMenu["qset"]["minq"].GetValue<MenuSlider>().Value, Color.Crimson);
            }
            if (RootMenu["drawings"]["qmax"])
            {
                Render.Circle.DrawCircle(Player.Position, RootMenu["qset"]["maxq"].GetValue<MenuSlider>().Value, Color.Yellow);
            }
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Crimson);
            }
        }

        protected override void Killsteal()
        {
            if (Q.IsReady() &&
                RootMenu["killsteal"]["ksq"])
            {
                var bestTarget =Extensions.GetBestKillableHero(Q,DamageType.Magical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(Player.GetRealAutoAttackRange(bestTarget)) &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) > bestTarget.Health)
                {
                    Q.Cast(bestTarget);
                }
            }

            if (R.IsReady() &&
                RootMenu["killsteal"]["ksr"])
            {
                var bestTarget = Extensions.GetBestKillableHero(R,DamageType.Magical);
                if (bestTarget != null &&
                    !bestTarget.IsValidTarget(Player.GetRealAutoAttackRange(bestTarget)) &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.R) > bestTarget.Health)
                {
                    R.Cast();
                }
            }
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
                    ComboMenu.Add(new MenuBool("usee", "使用 E "));
                    ComboMenu.Add(new MenuBool("eq", "^- 仅Q到后E", false));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- 当可击中敌人>=", 2, 1, 5));
                }
                RootMenu.Add(ComboMenu);
                var QSet = new Menu("qset", "Q 设置");
                {
                    QSet.Add(new MenuKeyBind("grabq", "半自动 Q", Keys.T, KeyBindType.Press));
                    QSet.Add(new MenuBool("autoq", "自动Q目标落点", true));
                    QSet.Add(new MenuSlider("minq", "最近Q距离", 300, 10, 400));
                    QSet.Add(new MenuSlider("maxq", "最远Q距离", 900, 500, 900));
                }
                RootMenu.Add(QSet);
                WhiteList = new Menu("black", "Q黑名单");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "禁止Q: " + target.CharacterName, false));
                    }
                }

                RootMenu.Add(WhiteList);
                var DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("qmax", "显示 Q 最远距离"));
                    DrawMenu.Add(new MenuBool("qmin", "显示 Q 最近距离", false));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));
                }
                RootMenu.Add(DrawMenu);
                KillstealMenu = new Menu("killsteal", "抢人头");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "使用Q"));
                    KillstealMenu.Add(new MenuBool("ksr", "使用R"));
                }
                RootMenu.Add(KillstealMenu);
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q"));
                    ComboMenu.Add(new MenuBool("usee", "Use E "));
                    ComboMenu.Add(new MenuBool("eq", "^- Only if Q Landed"));
                    ComboMenu.Add(new MenuBool("user", "Use R"));
                    ComboMenu.Add(new MenuSlider("hitr", "^- if Hits X Enemies", 2, 1, 5));
                }
                RootMenu.Add(ComboMenu);
                var QSet = new Menu("qset", "Q Settings");
                {
                    QSet.Add(new MenuKeyBind("grabq", "Grab Q", Keys.T, KeyBindType.Press));
                    QSet.Add(new MenuBool("autoq", "Use Auto Q on Dash", true));
                    QSet.Add(new MenuSlider("minq", "Min Q Range", 300, 10, 400));
                    QSet.Add(new MenuSlider("maxq", "Max Q Range", 900, 500, 900));
                }
                RootMenu.Add(QSet);
                WhiteList = new Menu("black", "Black List");
                {
                    foreach (var target in GameObjects.EnemyHeroes)
                    {
                        WhiteList.Add(new MenuBool(target.CharacterName.ToLower(), "Block: " + target.CharacterName, false));
                    }
                }

                RootMenu.Add(WhiteList);
                var DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("qmax", "Draw Q Max."));
                    DrawMenu.Add(new MenuBool("qmin", "Draw Q Min."));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));
                }
                RootMenu.Add(DrawMenu);
                KillstealMenu = new Menu("killsteal", "Killsteal");
                {
                    KillstealMenu.Add(new MenuBool("ksq", "Killsteal with Q"));
                    KillstealMenu.Add(new MenuBool("ksr", "Killsteal with R"));
                }
                RootMenu.Add(KillstealMenu);
            }

            RootMenu.Attach();
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 900);
            W = new Spell(SpellSlot.W, 0);
            E = new Spell(SpellSlot.E, 300);
            R = new Spell(SpellSlot.R, 600);

            Q.SetSkillshot(0.25f, 70, 2000, true, SkillshotType.Line);
        }
    }
}
