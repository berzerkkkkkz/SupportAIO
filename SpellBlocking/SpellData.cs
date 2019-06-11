﻿// Copyright 2014 - 2014 Esk0r
// SpellData.cs is part of Evade.
// 
// Evade is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Evade is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Evade. If not, see <http://www.gnu.org/licenses/>.

// GitHub: https://github.com/Esk0r/LeagueSharp/blob/master/Evade

using System.Linq;

namespace SupportAIO.SpellBlocking
{
    #region

    using EnsoulSharp;
    using EnsoulSharp.SDK;

    #endregion

    public class SpellData
    {
        public bool AddHitbox;
        public bool CanBeRemoved = false;
        public bool Centered;
        public string ChampionName;
        public CollisionObjectTypes[] CollisionObjects = { };
        public int DangerValue;
        public int Delay;
        public bool DisabledByDefault = false;
        public bool DisableFowDetection = false;
        public bool DontAddExtraDuration;
        public bool DontCheckForDuplicates = false;
        public bool DontCross = false;
        public bool DontRemove = false;
        public int ExtraDuration;
        public string[] ExtraMissileNames = { };
        public int ExtraRange = -1;
        public string[] ExtraSpellNames = { };
        public bool FixedRange;
        public bool ForceRemove = false;
        public bool FollowCaster = false;
        public string FromObject = "";
        public string[] FromObjects = { };
        public int Id = -1;
        public bool Invert;
        public bool IsDangerous = false;
        public int MissileAccel = 0;
        public bool MissileDelayed;
        public bool MissileFollowsUnit;
        public int MissileMaxSpeed;
        public int MissileMinSpeed;
        public int MissileSpeed;
        public string MissileSpellName = "";
        public float MultipleAngle;
        public int MultipleNumber = -1;
        public int RingRadius;
        public string SourceObjectName = "";
        public SpellSlot Slot;
        public string SpellName;
        public bool TakeClosestPath = false;
        public string ToggleParticleName = "";
        public SkillShotType Type;
        private int _radius;
        private int _range;
        
        public SpellData() { }

        public SpellData(string championName,
            string spellName,
            SpellSlot slot,
            SkillShotType type,
            int delay,
            int range,
            int radius,
            int missileSpeed,
            bool addHitbox,
            bool fixedRange,
            int defaultDangerValue)
        {
            ChampionName = championName;
            SpellName = spellName;
            Slot = slot;
            Type = type;
            Delay = delay;
            Range = range;
            _radius = radius;
            MissileSpeed = missileSpeed;
            AddHitbox = addHitbox;
            FixedRange = fixedRange;
            DangerValue = defaultDangerValue;
        }

        public string MenuItemName => SpellName;

        public int RawRadius => _radius;

        public int RawRange => _range;

        public int Radius
        {
            get
            {
                return !AddHitbox
                    ? _radius + EvadeManager.SkillShotsExtraRadius
                    : EvadeManager.SkillShotsExtraRadius + _radius + (int)GameObjects.AllyHeroes.FirstOrDefault(x => x.Distance(ObjectManager.Player) < SupportAIO.Common.Champion.E.Range).BoundingRadius;
            }
            set { _radius = value; }
        }

        public int Range
        {
            get
            {
                return _range +
                       (Type == SkillShotType.SkillshotLine || Type == SkillShotType.SkillshotMissileLine
                           ? EvadeManager.SkillShotsExtraRange
                           : 0);
            }
            set { _range = value; }
        }
    }
}
