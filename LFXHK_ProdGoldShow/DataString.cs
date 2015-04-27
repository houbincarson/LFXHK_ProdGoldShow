using System;
using System.Collections.Generic;
using System.Text;
using Java.Util;

namespace LFXHK_ProdGoldShow
{
    class DataString
    {
        private static string mYear;
        private static string mMonth;
        private static string mDay;
        private static string mWay;

        public static string StringData()
        {

            mYear = System.DateTime.Today.Year.ToString(); // 获取当前年份  
            mMonth = System.DateTime.Today.Month.ToString(); ; // 获取当前月份  
            mDay = System.DateTime.Today.Day.ToString(); // 获取当前月份的日期号码  
            string str = System.DateTime.Today.DayOfWeek.ToString();
            switch (str)
            {
                case "Monday":
                    mWay = "星期一";
                    break;
                case "Tuesday":
                    mWay = "星期二";
                    break;
                case "Wednesday":
                    mWay = "星期三";
                    break;
                case "Thursday":
                    mWay = "星期四";
                    break;
                case "Friday":
                    mWay = "星期五";
                    break;
                case "Saturday":
                    mWay = "星期六";
                    break;
                case "Sunday":
                    mWay = "星期日";
                    break;
            }
            return mYear + "年" + mMonth + "月" + mDay + "日" + "  " + mWay;
        }
    }
}
