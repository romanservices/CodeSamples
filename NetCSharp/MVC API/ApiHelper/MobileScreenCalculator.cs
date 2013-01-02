using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotFramework.Web.Mvc.Api.ApiHelper
{
    public class MobileScreenCalculator
    {
        public static int HtmlWordCount(int width, int height)
        {
           
            int sum = width*height/1000;
            if(sum  > 100 && sum < 300)
            {
                return 55;
            }
            if (sum > 300 && sum < 400 )
            {
                return 65;
            }
            if (sum > 400 && sum < 500)
            {
                return 75;
            }
            if (sum > 500 && sum < 600)
            {
                return 85;
            }
            if (sum > 600 && sum < 700)
            {
                return 275;
            }
            if (sum > 700 && sum < 800)
            {
                return 320;
            }
            if (sum > 800 && sum < 900)
            {

            }
            if (sum > 900 && sum < 1000)
            {

            }
            if (sum > 1000 && sum < 1100)
            {

            }
            if (sum > 1100 && sum < 1200)
            {

            }
            if (sum > 1200 && sum < 1300)
            {

            }
            if (sum > 1500 && sum < 2500)
            {

            }
            return 100;
        }
    }
}