﻿// Copyright 2014 - 2014 Esk0r
// IsSafeResult.cs is part of Evade.
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

namespace SupportAIO.SpellBlocking
{
    #region

    using System.Collections.Generic;

    #endregion

    public struct IsSafeResult
    {
        public bool IsSafe;
        public List<Skillshot> SkillshotList;
    }
}
