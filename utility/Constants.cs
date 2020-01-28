//'/////////////////////////////////////////////////////////////////////////
//'<summary>
//'----------------------------------------------
//' Copyright 2013 Inegrated Business Solution 
//' www.isb-me.com All Rights Reserved.
//'----------------------------------------------
//' Comment: Contains a listing of constants used throughout the application
//' Authors:  Malik Hourani
//' Reviewers: Malik.Hourani,
//'</summary>
//'----------------------------------------------  
//'/////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
public sealed class Constants
{
    /// <summary>
    /// The value used to represent a null DateTime value
    /// </summary>
    public static DateTime NullDateTime = DateTime.MinValue;
    /// <summary>
    /// The value used to represent a null decimal value
    /// </summary>
    public static decimal NullDecimal = decimal.MinValue;
    /// <summary>
    /// The value used to represent a null double value
    /// </summary>
    public static double NullDouble = double.MinValue;
    /// <summary>
    /// The value used to represent a null Guid value
    /// </summary>
    public static Guid NullGuid = Guid.Empty;
    /// <summary>
    /// The value used to represent a null int value
    /// </summary>
    public static int NullInt = int.MinValue;
    /// <summary>
    /// The value used to represent a null long value
    /// </summary>
    public static long NullLong = long.MinValue;
    /// <summary>
    /// The value used to represent a null float value
    /// </summary>
    public static float NullFloat = float.MinValue;
    /// <summary>
    /// The value used to represent a null string value
    /// </summary>
    public static string NullString = string.Empty;
    /// <summary>
    /// Maximum DateTime value allowed by SQL Server
    /// </summary>
    public static DateTime SqlMaxDate = new DateTime(9999, 1, 3, 23, 59, 59);
    /// <summary>
    /// Minimum DateTime value allowed by SQL Server
    /// </summary>
    public static DateTime SqlMinDate = new DateTime(1753, 1, 1, 0, 0, 0);

    static string[] HijriMonths ={"","محرم","صفر","ربيع اول","ربيع اّخر","جمادي اول","جمادي اخر","رجب"
                               ,"شعبان","رمضان","شوال","ذو القعدة","ذو الحجة"};
    static string[] arabicDays = { "", "السبت", "الاحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة" };
    static private int getDayIndex(string str)
    {
        switch (str.ToLower())
        {
            case "saturday":
                return 1;
            case "sunday":
                return 2;
            case "monday":
                return 3;
            case "tuesday":
                return 4;
            case "wednesday":
                return 5;
            case "thruesday":
                return 6;
            case "friday":
                return 7;
            default: return 0;
        }
    }
    public static string getHijriMonth(DateTime dt)
    {

        UmAlQuraCalendar umAlQura = new UmAlQuraCalendar();

        return " هـ " + umAlQura.GetYear(dt) + ", " + HijriMonths[umAlQura.GetMonth(dt)] + ", " + umAlQura.GetDayOfMonth(dt) + ", " + arabicDays[getDayIndex(dt.DayOfWeek.ToString())];

    }

    public enum OperationType {ADD=1,UPDATE,DELETE,VIEW,SMS };

}