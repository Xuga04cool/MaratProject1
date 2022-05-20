using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratProject
{
    static class BinCalc
    {
        public static List<int> DoubleSumm(List<int> a, List<int> b)
        {
            int c = FromDoubleToTen(a);
            int d = FromDoubleToTen(b);

            int ApB = c + d;
            List<int> retArr = FromTenToDouble(ApB);
            int max = Math.Max(a.Count, b.Count);
            for (int i = retArr.Count; i < max; i++)
            {
                retArr.Insert(0, 0);
            }
            return retArr;
        }

        public static long ArrToInt(List<int> arr)
        {
            string res = "";
            for (int i = 0; i < arr.Count; i++)
            {
                res += Convert.ToString(arr[i]);
            }
            return Convert.ToInt64(res);
        }


        public static List<int> FromTenToDouble(int n)
        {
            List<int> res = new List<int>();

            string strRes = Convert.ToString(n, 2);
            foreach (char s in strRes)
            {
                res.Add(Convert.ToInt32(s - '0'));
            }

            return res;
        }


        public static int FromDoubleToTen(List<int> n)
        {
            string bin = Convert.ToString(ArrToInt(n));


            return Convert.ToInt32(bin, 2);
        }


        public static List<int> ShiftLeft(List<int> arr)
        {
            List<int> retArr = new List<int>();

            for (int i = 0; i < arr.Count; i++)
            {
                if (i == arr.Count - 1)
                    retArr.Add(0);
                else
                    retArr.Add(arr[i + 1]);
            }

            return retArr;
        }

        public static string ToStringArray(List <int> arr)
        {
            string retSt = "";

            for (int i = 0; i < arr.Count; i++)
            {
                retSt += Convert.ToString(arr[i]);
            }

            return retSt;
        }




    }
}
