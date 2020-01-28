using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLiveSessionExcel.utilities
{
    public class Days
    { 
        public int DayNumber;
        public string DayNameAr;
        public DateTime DayDate;
        public string DayNameEn;
        public Session[,] sessions = new Session[6,18];//sessions ,studios 
       

    }
}
