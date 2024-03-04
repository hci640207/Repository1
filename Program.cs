using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ENC
{
    class Program
    {
        static void Main(string[] args)
        {
            //void Test(Action<string[]> action, string note)
            //{
            //    var t = new Stopwatch();
            //    t.Reset();
            //    t.Start();
            //    action.Invoke(args);
            //    t.Stop();
            //    Console.WriteLine($"{note} {t.Elapsed}");
            //}
            //Test(V1, "V1");
            //Test(V1, "V1");
            //Test(V1, "V1");
            //Test(V2, "V2");
            //Test(V2, "V2");
            //Test(V2, "V2");
            V2(args);
        }
        private static void V2(string[] args)
        {
            if (ArgsError(args == null || args.Length < 2)) return;
            if (ArgsError(!File.Exists(args[1])))
            {
                Console.WriteLine("檔案不存在");
                return;
            }
            var k = Encoding.UTF8.GetBytes(args[0]);
            var buf = new byte[8192];
            using (var stream = new FileStream(args[1], FileMode.Open))
            {
                while (true)
                {
                    var len = stream.Read(buf, 0, buf.Length);
                    if (len < 1) break;
                    for (int i = 0; i < len; i++)
                        buf[i] ^= k[i % k.Length];
                    stream.Seek(-len, SeekOrigin.Current);
                    stream.Write(buf, 0, len);
                }
            }
        }
        private static void V1(string[] args)
        {
            if (ArgsError(args == null || args.Length < 2)) return;
            if (ArgsError(!File.Exists(args[1])))
            {
                Console.WriteLine("檔案不存在");
                return;
            }
            var k = Encoding.UTF8.GetBytes(args[0]);
            var buf = new byte[k.Length];
            using (var stream = new FileStream(args[1], FileMode.Open))
            {
                while (true)
                {
                    var len = stream.Read(buf, 0, buf.Length);
                    if (len < 1) break;
                    for (int i = 0; i < k.Length; i++)
                        buf[i] ^= k[i];
                    stream.Seek(-len, SeekOrigin.Current);
                    stream.Write(buf, 0, len);
                }
            }
        }

        static bool ArgsError(bool condition)
        {
            if (!condition) return false;
            Console.WriteLine(@"
ENC {KEY} {FilePath}

ex:
   ENC foobar C:\tmp\ABC.zip 
");
            return true;
        }
    }
}
