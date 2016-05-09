using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            /*int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Point[] newValues = ConvertingPoints(values);
            foreach (Point j in newValues)
                Console.WriteLine(j);
            */
            int[] a = { 1, 2, 3 };
            int[] b = { 4, 5,8 };
            
            foreach(int r in Zip(a,b,(x,y)=> x+y))
                {
                Console.WriteLine(r);
                }
        }

        static void PrettyPrint<T>(T [] values)
        {
            /* a) */
            Array.ForEach(values, new Action<T>(Print));

            /* b) */
            Array.ForEach(values, (T t) => { Console.WriteLine(t.GetType()); Console.WriteLine(t.ToString()); });
        }

        static void Print<T>(T value)
        {
            Console.WriteLine(value.GetType());
            Console.WriteLine(value.ToString());
        }
        
        static Point ConvertToPoint(int value)
        {
            Point point = new Point();
            point.x = value;
            point.y = value * value;
            return point;
        }

        public struct Point
        {
            public int x;
            public int y;

            public override string ToString()
            {
                return "(" + x + "," + y + ")";
            }
        }

        static Point [] ConvertingPoints(int [] array)
        {
            Converter<int, Point> converter = new Converter<int, Point>(ConvertToPoint);
            return Array.ConvertAll(array, converter);
        }


        /* 3 exercicio */

        static IEnumerable<int> BetweenRange(int begin, int end)
        {
            for(int i=begin; begin < end; ++i)
            {
                yield return i;
            }
        }

        static IEnumerable<T> Concat<T>(IEnumerable<T> seq1, IEnumerable<T> seq2)
        {
            IEnumerable<T> toIterate = seq1.Concat(seq2);
            IEnumerator<T> iter = toIterate.GetEnumerator();
            while (iter.MoveNext())
            {
                yield return iter.Current;
            }
        }

        /* Analise */
        /*1.*/
        delegate int Operation(String s);

        /* Não existe instrução IL que faça um call sobre um ponteiro para um método. O método Invoke só é preenchido 
         * pelo jitter na tradução de IL para nativo. */


        /*Pratica II - b*/

        static IEnumerable<R> Zip<TA, TB, R>(IEnumerable<TA> a, IEnumerable<TB> b, Func<TA, TB, R> join)
        {
            IEnumerator<TA> AEnum = a.GetEnumerator();
            IEnumerator<TB> BEnum = b.GetEnumerator();
            while (AEnum.MoveNext() && BEnum.MoveNext())
            {
                yield return join(AEnum.Current,BEnum.Current);
            }
        }
    }
}

