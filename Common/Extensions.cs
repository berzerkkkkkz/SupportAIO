namespace SupportAIO.Common
{
    using EnsoulSharp;
    using EnsoulSharp.SDK;
    using SharpDX;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The UtilityData class.
    /// </summary>
    internal static class Extensions
    {
        #region Public Methods and Operators

        public static bool AnyWallInBetween(Vector3 startPos, Vector2 endPos)
        {
            for (var i = 0; i < startPos.Distance(endPos); i++)
            {
                if ((startPos + Vector3.Normalize(startPos - endPos.ToVector3()) * i).IsWall())
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        ///     Gets the valid ally heroes targets in the game.
        /// </summary>
        public static List<AIHeroClient> GetAllyHeroesTargets()
        {
            return GetAllyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetAllyHeroesTargetsInRange(float range)
        {
            return GameObjects.AllyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetAllyLaneMinionsTargets()
        {
            return GetAllyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid ally lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetAllyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.AllyMinions.Where(m => m.IsValidTarget(range, true)).ToList();
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetBestEnemyHeroesTargets()
        {
            return GetBestEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the best valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        private static List<AIHeroClient> GetBestEnemyHeroesTargetsInRange(float range)
        {
            return TargetSelector.GetTargets(range);
        }


        public static AIHeroClient GetBestEnemyHeroTarget()
        {
            return GetBestEnemyHeroTargetInRange(float.MaxValue);
        }

        public static AIHeroClient GetBestEnemyHeroTargetInRange(float range)
        {
            var target = TargetSelector.GetTarget(range);
            if (target != null && target.IsValidTarget() && !target.IsInvulnerable)
            {
                return target;
            }
            return null;
        }

        public static AIHeroClient GetBestKillableHero(Spell spell, DamageType damageType = DamageType.True,
          bool ignoreShields = false)
        {
            return TargetSelector.GetTargets(spell.Range).FirstOrDefault(t => t.IsValidTarget());
        }
        public static AIHeroClient GetBestKillableHeroEQ(DamageType damageType = DamageType.True, bool ignoreShields = false)
        {
            return TargetSelector.GetTargets(1800).FirstOrDefault(t => t.IsValidTarget());
        }
        /// <summary>
        ///     Gets the valid enemy heroes targets in the game.
        /// </summary>
        public static List<AIHeroClient> GetEnemyHeroesTargets()
        {
            return GetEnemyHeroesTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid enemy heroes targets in the game inside a determined range.
        /// </summary>
        public static List<AIHeroClient> GetEnemyHeroesTargetsInRange(float range)
        {
            return GameObjects.EnemyHeroes.Where(h => h.IsValidTarget(range)).ToList();
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game.
        /// </summary>
        public static List<AIMinionClient> GetEnemyLaneMinionsTargets()
        {
            return GetEnemyLaneMinionsTargetsInRange(float.MaxValue);
        }

        /// <summary>
        ///     Gets the valid lane minions targets in the game inside a determined range.
        /// </summary>
        public static List<AIMinionClient> GetEnemyLaneMinionsTargetsInRange(float range)
        {
            return GameObjects.EnemyMinions.Where(x => x.IsValidTarget(range) && x.IsMinion()).Cast<AIMinionClient>().ToList();
        }

        #endregion
    }
}