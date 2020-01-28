using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageLiveSessionWeb
{
    public class StudiosCount : IEquatable<StudiosCount>
    {
        public int APP_DAY;
        public int STUDIO_NUMBER;

        public bool Equals(StudiosCount other)
        {
            if (APP_DAY == other.APP_DAY && STUDIO_NUMBER == other.STUDIO_NUMBER)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashFirstName = APP_DAY == 0 ? 0 : APP_DAY.GetHashCode();
            int hashLastName = APP_DAY == 0 ? 0 : APP_DAY.GetHashCode();

            return hashFirstName ^ hashLastName;
        }
    }
}