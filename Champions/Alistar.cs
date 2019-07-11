using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI.Values;
using EnsoulSharp.SDK.Utility;


using Extensions = SupportAIO.Common.Extensions;
using Color = System.Drawing.Color;
using Menu = EnsoulSharp.SDK.MenuUI.Menu;
using Spell = EnsoulSharp.SDK.Spell;
using System.Windows.Forms;
using static EnsoulSharp.SDK.Gapcloser;

namespace SupportAIO.Champions
{
    class Alistar : Common.Champion
    {
        internal static int language = 0;

        internal Alistar()
        {
            this.SetSpells();
            this.SetMenu();
            this.SetEvents();
        }

        protected override void Combo()
        {

            bool useQ = RootMenu["combo"]["useq"];
            bool useW = RootMenu["combo"]["usew"];
            bool useR = RootMenu["combo"]["user"];
            float enemies = RootMenu["combo"]["hitr"].GetValue<MenuSlider>().Value;
            float hp = RootMenu["combo"]["hp"].GetValue<MenuSlider>().Value;
            var target = Extensions.GetBestEnemyHeroTargetInRange(W.Range);

            if (!target.IsValidTarget())
            {

                return;
            }

            if (W.IsReady() && Q.IsReady() && useW && target.IsValidTarget(W.Range) && Q.Mana + W.Mana <= Player.Mana)
            {

                if (target != null)
                {
                    W.CastOnUnit(target);
                }
            }
            if (target.IsValidTarget(Q.Range)  && useQ)
            {

                if (target != null)
                {
                    Q.Cast();
                }
            }

            if (target.IsValidTarget(E.Range))
            {

                if (target != null)
                {
                    E.Cast();
                }
            }

            if (useR)
            {

                if (target != null && enemies <= Player.CountEnemyHeroesInRange(W.Range))
                {
                    if (hp >= Player.HealthPercent)
                    {
                        R.Cast();
                    }
                }
            }

        }

        protected override void SemiR()
        {
            if (RootMenu["combo"]["autor"])
            {
                if (Player.HasBuffOfType(BuffType.Charm) || Player.HasBuffOfType(BuffType.Stun) ||
                    Player.HasBuffOfType(BuffType.Fear) || Player.HasBuffOfType(BuffType.Snare) ||
                    Player.HasBuffOfType(BuffType.Taunt) || Player.HasBuffOfType(BuffType.Knockback) ||
                    Player.HasBuffOfType(BuffType.Suppression) && Player.CountEnemyHeroesInRange(W.Range)>1)
                {
                    R.Cast();
                }
            }
            var Flash = ObjectManager.Player.GetSpellSlot("SummonerFlash");
            if (RootMenu["flashe"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                var target = Extensions.GetBestEnemyHeroTargetInRange(1300);
                    if (Q.IsReady())
                {
                    if (Flash != SpellSlot.Unknown && ObjectManager.Player.Spellbook.GetSpell(Flash).IsReady && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(1300))
                        {
                            foreach (var en in GameObjects.EnemyMinions)
                            {
                                if (!en.IsDead && en.IsValidTarget(W.Range) && en.Distance(target) < 730)
                                {

                                    if (target.Distance(Player) > Q.Range && target != null)
                                    {
                                        W.CastOnUnit(en);
                                    }
                                    if (Q.IsReady())
                                    {
                                        if (Flash != SpellSlot.Unknown && ObjectManager.Player.Spellbook.GetSpell(Flash).IsReady && target.IsValidTarget())
                                        {
                                            if (target.IsValidTarget(Q.Range + 410))
                                            {
                                                if (target.Distance(Player) > Q.Range && target != null)
                                                {
                                                    if (Q.Cast())
                                                    {
                                                        DelayAction.Add(200, () =>
                                                        {
                                                            ObjectManager.Player.Spellbook.CastSpell(Flash, target.Position);
                                                        });

                                                    }
                                                }
                                            }
                                        }
                                    };
                                }
                            }
                        }
                    }
                }
            }
            if (RootMenu["flashq"].GetValue<MenuKeyBind>().Active)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPosCenter);
                var target = Extensions.GetBestEnemyHeroTargetInRange(Q.Range + 380);
                if (Q.IsReady())
                {
                    if (Flash != SpellSlot.Unknown && ObjectManager.Player.Spellbook.GetSpell(Flash).IsReady && target.IsValidTarget())
                    {
                        if (target.IsValidTarget(Q.Range + 410))
                        {
                            if (target.Distance(Player) > Q.Range && target != null)
                            {
                                if (Q.Cast())
                                {

                                    DelayAction.Add(200, () =>
                                    {
                                        ObjectManager.Player.Spellbook.CastSpell(Flash, target.Position);
                                    });

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
                Render.Circle.DrawCircle(Player.Position, Q.Range,  Color.Crimson);
            }
            if (RootMenu["drawings"]["draww"])
            {
                Render.Circle.DrawCircle(Player.Position, W.Range,  Color.Yellow);
            }
            if (RootMenu["drawings"]["drawe"])
            {
                Render.Circle.DrawCircle(Player.Position, E.Range,  Color.Yellow);
            }
            if (RootMenu["drawings"]["drawflash"])
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range + 380,  Color.WhiteSmoke);
            }
            if (RootMenu["drawings"]["drawengage"])
            {
                Render.Circle.DrawCircle(Player.Position, 1300, Color.Turquoise);
            }
        }

        protected override void Killsteal()
        {
            if (Q.IsReady() &&
                RootMenu["killsteal"]["useq"])
            {
                var bestTarget = Extensions.GetBestKillableHero(Q, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.Q) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(Q.Range))
                {
                    Q.Cast();
                }
            }
            if (W.IsReady() &&
                RootMenu["killsteal"]["usew"])
            {
                var bestTarget = Extensions.GetBestKillableHero(W, DamageType.Magical, false);
                if (bestTarget != null &&
                    Player.GetSpellDamage(bestTarget, SpellSlot.W) >= bestTarget.Health &&
                    bestTarget.IsValidTarget(W.Range))
                {
                    W.CastOnUnit(bestTarget);
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
            RootMenu.Add(new MenuSeparator("1","Press F5 to reload language(按 F5 确认切换语言)"));
            language = RootMenu.GetValue<MenuList>("language").Index;

            if (language != 1)
            {
                ComboMenu = new Menu("combo", "连招");
                {
                    ComboMenu.Add(new MenuBool("useq", "使用 Q"));
                    ComboMenu.Add(new MenuBool("usew", "使用 W"));
                    ComboMenu.Add(new MenuBool("usee", "使用 E"));
                    ComboMenu.Add(new MenuBool("user", "使用 R"));
                    ComboMenu.Add(new MenuBool("autor", "被控自动R"));
                    ComboMenu.Add(new MenuSlider("hp", "自动R 当血量%<=", 25, 10, 100));
                    ComboMenu.Add(new MenuSlider("hitr", "自动R时附近敌人>=", 2, 1, 5));

                }
                RootMenu.Add(ComboMenu);
                KillstealMenu = new Menu("killsteal", "抢人头");
                {
                    KillstealMenu.Add(new MenuBool("useq", "使用 Q", false));
                    KillstealMenu.Add(new MenuBool("usew", "使用 W", false));

                }
                RootMenu.Add(KillstealMenu);
                DrawMenu = new Menu("drawings", "显示");
                {
                    DrawMenu.Add(new MenuBool("drawq", "显示 Q 距离", false));
                    DrawMenu.Add(new MenuBool("draww", "显示 W 距离"));
                    DrawMenu.Add(new MenuBool("drawe", "显示 E 距离", false));
                    DrawMenu.Add(new MenuBool("drawflash", "显示Q闪距离"));
                    DrawMenu.Add(new MenuBool("drawengage", "显示WQ闪距离", false));

                }
                RootMenu.Add(DrawMenu);

                RootMenu.Add(new MenuKeyBind("flashq", "Q闪!", Keys.T, KeyBindType.Press));
                RootMenu.Add(new MenuKeyBind("flashe", "WQ闪!", Keys.G, KeyBindType.Press));
             
            }
            else
            {
                ComboMenu = new Menu("combo", "Combo");
                {
                    ComboMenu.Add(new MenuBool("useq", "Use Q in Combo"));
                    ComboMenu.Add(new MenuBool("usew", "Use W in Combo"));
                    ComboMenu.Add(new MenuBool("usee", "Use E in Combo"));
                    ComboMenu.Add(new MenuBool("user", "Use R in Combo"));
                    ComboMenu.Add(new MenuBool("autor", "Auto R on CC"));
                    ComboMenu.Add(new MenuSlider("hp", "Use R if HP <=", 25, 10, 100));
                    ComboMenu.Add(new MenuSlider("hitr", "Min. Enemies", 2, 1, 5));

                }
                RootMenu.Add(ComboMenu);
                KillstealMenu = new Menu("killsteal", "Killsteal");
                {
                    KillstealMenu.Add(new MenuBool("useq", "Use Q to Killsteal"));
                    KillstealMenu.Add(new MenuBool("usew", "Use W to Killsteal"));

                }
                RootMenu.Add(KillstealMenu);
                DrawMenu = new Menu("drawings", "Drawings");
                {
                    DrawMenu.Add(new MenuBool("drawq", "Draw Q Range"));
                    DrawMenu.Add(new MenuBool("draww", "Draw W Range"));
                    DrawMenu.Add(new MenuBool("drawe", "Draw E Range"));
                    DrawMenu.Add(new MenuBool("drawflash", "Draw Q Flash Range"));
                    DrawMenu.Add(new MenuBool("drawengage", "Draw Engage Range"));

                }
                RootMenu.Add(DrawMenu);

                RootMenu.Add(new MenuKeyBind("flashq", "Q - Flash", Keys.T, KeyBindType.Press));
                RootMenu.Add(new MenuKeyBind("flashe", "W - Q - Flash", Keys.G, KeyBindType.Press));
            }
            RootMenu.Attach();
        }

        internal override void OnGapcloser(AIBaseClient target, GapcloserArgs Args)
        {
           
                if (target != null && Args.EndPosition.Distance(Player) < Q.Range)
                {
                    Q.Cast();
                }
            
        }

        protected override void SetSpells()
        {
            Q = new Spell(SpellSlot.Q, 365);
            W = new Spell(SpellSlot.W, 650);
            E = new Spell(SpellSlot.E, 350);
            R = new Spell(SpellSlot.R, 0);

        }
    }
}
