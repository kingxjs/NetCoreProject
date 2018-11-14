using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreObject.Core
{
    public class CommonModel
    {
        public static decimal GetGUID()
        {
            var y = long.Parse(string.Format("{0:yyyyMMddHHmmss}", DateTime.Now).ToString()); //ffff
            var one = y * Math.Pow(10, 14);
            var one_str = Convert.ToDecimal(Decimal.Parse(one.ToString(), System.Globalization.NumberStyles.Float));

            var t = DateTime.Now.Ticks;
            //var two = t * Math.Pow(10, 7);
            var two = t;
            var two_str = Convert.ToDecimal(Decimal.Parse(two.ToString(), System.Globalization.NumberStyles.Float));

            var three = Math.Abs(Guid.NewGuid().GetHashCode());
            var three_str = Convert.ToDecimal(Decimal.Parse(three.ToString(), System.Globalization.NumberStyles.Float));
            var guid = one_str + two_str + three_str;
            return guid;
        }
    }
}
