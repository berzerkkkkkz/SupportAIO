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
    class Janna : Champion
    {
        private int language;

        internal Janna()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        protected override void Combo()
        {
            bool useQ = RootMenu["combo"]["useq"];
            bool useW = RootMenu["combo"]["usew"];




            var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range);

            if (!target.IsValidTarget())
            {

                return;
            }


            if (target.IsValidTarget(Q.Range) && useQ)
            {

                if (target != null)
                {
                    Q.Cast(target);
                }
            }
            if (target.IsValidTarget(W.Range) && useW)
            {

                if (target != null)
                {
                    W.CastOnUnit(target);
                }
            }
        }

        protected override void SemiR()
        {


            if (RootMenu["insec"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);

                var Flash = ObjectManager.Player.GetSpellSlot("SummonerFlash");

                var target = Extensions.GetBestEnemyHeroTargetInRange(R.Range + 410);
                if (R.IsReady())
                {
                    if (Flash != SpellSlot.Unknown && ObjectManager.Player.Spellbook.GetSpell(Flash).IsReady && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(380))
                        {

                            foreach (var ally in GameObjects.AllyHeroes)
                            {
                                if (ally != null && ally.Distance(Player) < 1000 && !ally.IsMe)
                                {
                                    if (ObjectManager.Player.Spellbook.CastSpell(Flash, (target.Position.Extend(ally.Position, -100))))
                                    {
                                        R.Cast();

                                    }
                                }
                            }
                            if (GameObjects.AllyHeroes.Where(x => x.Distance(Player) < E.Range)
                                    .Count() == 1)
                            {
                                if (ObjectManager.Player.Spellbook.CastSpell(Flash, (target.Position.Extend(Player.Position, -100))))
                                {
                                    R.Cast();

                                }
                            }


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
            if (RootMenu["drawings"]["drawr"])
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color.Yellow);
            }

        }

        protected override void Killsteal()
        {

        }

        protected override void Harass()
        {

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

                RootMenu.Add(new MenuKeyBind("insec", "回旋吹", Keys.T, KeyBindType.Press));


                ComboMenu = new Menu("combo", "连招");
                {
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("support", "辅助模式"));
                }
                RootMenu.Add(ComboMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离"));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离"));
                    DrawMenu.Add(new MenuBool("drawr", "显示 R 距离"));

                }
                RootMenu.Add(DrawMenu);

                EvadeMenu = new Menu("wset", "E设定");
                {
                    var First = new Menu("first", "技能列表");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);
                    var test = new Menu("Misc.E.Spell.Menu", "用E增加队友攻击");
                    foreach (var spell in
                        GameObjects.AllyHeroes.Where(h => !h.IsMe)
                            .SelectMany(
                                hero => DamageBoostDatabase.Spells.Where(s => s.Champion == hero.CharacterName)))
                    {
                        test.Add(new MenuBool("Misc.E.Spell." + spell.Spell, spell.Champion + " " + spell.Slot));
                    }
                    EvadeMenu.Add(test);
                    EvadeMenu.Add(First);
                    //var zlib = new Menu("zlib", "ZLib");

                    //SupportAIO.ZLib.Attach(EvadeMenu);


                }
                RootMenu.Add(EvadeMenu);
            }

            else
            {
                RootMenu.Add(new MenuKeyBind("insec", "Insec Key", Keys.T, KeyBindType.Press));

                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("support", "Support Mode"));
                }
                RootMenu.Add(ComboMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawr", "Draw R Range"));

                }
                RootMenu.Add(DrawMenu);

                EvadeMenu = new Menu("wset", "Shielding");
                {
                    var First = new Menu("first", "Spells Detector");
                    SpellBlocking.EvadeManager.Attach(First);
                    SpellBlocking.EvadeOthers.Attach(First);
                    SpellBlocking.EvadeTargetManager.Attach(First);
                    var test = new Menu("Misc.E.Spell.Menu", "Boost Ally Damage on Spells");
                    foreach (var spell in
                        GameObjects.AllyHeroes.Where(h => !h.IsMe)
                            .SelectMany(
                                hero => DamageBoostDatabase.Spells.Where(s => s.Champion == hero.CharacterName)))
                    {
                        test.Add(new MenuBool("Misc.E.Spell." + spell.Spell, spell.Champion + " " + spell.Slot));
                    }
                    EvadeMenu.Add(test);
                    EvadeMenu.Add(First);

                }
                RootMenu.Add(EvadeMenu);
            }

            RootMenu.Attach();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {

            if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
            {
                Q.Cast(Args.EndPosition);
            }

        }



        internal override void OnProcessSpellCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            if (!E.IsReady())
            {
                return;
            }

            if (sender.IsValid && sender.IsAlly && !sender.IsMe && sender.Distance(Player) < E.Range)
            {
                var spell = args.SData.Name;
                var caster = sender as AIHeroClient;

                if (DamageBoostDatabase.Spells.Any(s => s.Spell == spell) && caster.CountEnemyHeroesInRange(1500) > 0)
                {
                    if (RootMenu["wset"]["Misc.E.Spell.Menu"]["Misc.E.Spell." + args.SData.Name])
                    {

                        E.CastOnUnit(caster);


                    }
                }
            }
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 830);
            W = new Spell(SpellSlot.W, 600);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 725);
            Q.SetSkillshot(0.25f, 120f, 900f, false, SkillshotType.Line);
        }
    }
}
