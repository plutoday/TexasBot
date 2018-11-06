using System;
using System.Collections.Generic;
using System.Linq;

namespace Preflop.StartingHands
{
    public static class StartingHandEvs
    {
        private static Dictionary<string, double> evs = new Dictionary<string, double>();

        private static List<Tuple<string, int>> rankedCount = new List<Tuple<string, int>>(); 

        private static Dictionary<string, double> percentiles = new Dictionary<string, double>();

        public static double GetPercentila(string handName)
        {
            if (evs.Count == 0)
            {
                Initialize();
            }

            if (!percentiles.ContainsKey(handName))
            {
                throw new InvalidOperationException($"{handName} not contained in percentiles");
            }

            return percentiles[handName];
        }

        public static double GetEv(string handName)
        {
            if (evs.Count == 0)
            {
                Initialize();
            }

            if (!evs.ContainsKey(handName))
            {
                throw new InvalidOperationException($"{handName} not contained in evs");
            }

            return evs[handName];
        }

        private static void Initialize()
        {
            InitEvs();
            InitRankedCount();
        }

        private static void InitRankedCount()
        {
            foreach (var ev in evs)
            {
                int count;
                if (ev.Key[0] == ev.Key[1])
                {
                    count = 6;
                }
                else if (ev.Key.EndsWith("s"))
                {
                    count = 4;
                }
                else
                {
                    count = 16;
                }
                rankedCount.Add(new Tuple<string, int>(ev.Key, count));
            }

            rankedCount.Sort((t1, t2) => evs[t2.Item1].CompareTo(evs[t1.Item1]));

            int totalCount = rankedCount.Sum(t => t.Item2);
            int countSoFar = 0;
            foreach (var tuple in rankedCount)
            {
                countSoFar += tuple.Item2;
                percentiles.Add(tuple.Item1, (double)countSoFar/totalCount);
            }
        }
        
        private static void InitEvs()
        {

            //1
            evs.Add("AA", 2.32);
            evs.Add("KK", 1.67);
            evs.Add("QQ", 1.22);
            evs.Add("JJ", 0.86);
            evs.Add("AKs", 0.78);
            evs.Add("AQs", 0.59);
            evs.Add("TT", 0.58);
            evs.Add("AK", 0.51);
            evs.Add("AJs", 0.44);
            evs.Add("KQs", 0.39);
            //11
            evs.Add("99", 0.38);
            evs.Add("ATs", 0.32);
            evs.Add("AQ", 0.31);
            evs.Add("KJs", 0.29);
            evs.Add("88", 0.25);
            evs.Add("QJs", 0.23);
            evs.Add("KTs", 0.20);
            evs.Add("A9s", 0.191);
            evs.Add("AJ", 0.19);
            evs.Add("QTs", 0.17);
            //21
            evs.Add("KQ", 0.161);
            evs.Add("77", 0.16);
            evs.Add("JTs", 0.15);
            evs.Add("A8s", 0.10);
            evs.Add("K9s", 0.09);
            evs.Add("AT", 0.083);
            evs.Add("A5s", 0.082);
            evs.Add("A7s", 0.081);
            evs.Add("KJ", 0.08);
            evs.Add("66", 0.07);
            //31
            evs.Add("T9s", 0.053);
            evs.Add("A4s", 0.052);
            evs.Add("Q9s", 0.051);
            evs.Add("J9s", 0.04);
            evs.Add("QJ", 0.031);
            evs.Add("A6s", 0.03);
            evs.Add("55", 0.021);
            evs.Add("A3s", 0.02);
            evs.Add("K8s", 0.011);
            evs.Add("KT", 0.01);
            //41
            evs.Add("98s", 0.003);
            evs.Add("T8s", 0.002);
            evs.Add("K7s", 0.001);
            evs.Add("Z2s", 0.00);
            evs.Add("87s", -0.02);
            evs.Add("QT", -0.021);
            evs.Add("Q8s", -0.022);
            evs.Add("44", -0.03);
            evs.Add("A9", -0.031);
            evs.Add("J8s", -0.032);
            //51
            evs.Add("76s", -0.033);
            evs.Add("JT", -0.034);
            evs.Add("97s", -0.04);
            evs.Add("K6s", -0.041);
            evs.Add("K5s", -0.05);
            evs.Add("K4s", -0.051);
            evs.Add("T7s", -0.052);
            evs.Add("Q7s", -0.06);
            evs.Add("K9", -0.07);
            evs.Add("65s", -0.071);
            //61
            evs.Add("T9", -0.07);
            evs.Add("86s", -0.071);
            evs.Add("A8", -0.072);
            evs.Add("J7s", -0.073);
            evs.Add("33", -0.074);
            evs.Add("54s", -0.08);
            evs.Add("Q6s", -0.081);
            evs.Add("K3s", -0.082);
            evs.Add("Q9", -0.083);
            evs.Add("75s", -0.09);
            //71
            evs.Add("22", -0.091);
            evs.Add("J9", -0.092);
            evs.Add("64s", -0.093);
            evs.Add("Q5s", -0.094);
            evs.Add("K2s", -0.095);
            evs.Add("96s", -0.096);
            evs.Add("Q3s", -0.10);
            evs.Add("J8", -0.101);
            evs.Add("98", -0.102);
            evs.Add("T8", -0.103);
            //81
            evs.Add("97", -0.104);
            evs.Add("A7", -0.105);
            evs.Add("T7", -0.106);
            evs.Add("Q4s", -0.107);
            evs.Add("Q8", -0.1101);
            evs.Add("J5s", -0.1102);
            evs.Add("T6", -0.1103);
            evs.Add("75", -0.1104);
            evs.Add("J4s", -0.1105);
            evs.Add("74s", -0.1106);
            //91
            evs.Add("K8", -0.1107);
            evs.Add("86", -0.1108);
            evs.Add("53s", -0.1109);
            evs.Add("K7", -0.111);
            evs.Add("63s", -0.1111);
            evs.Add("J6s", -0.1112);
            evs.Add("85", -0.1113);
            evs.Add("T6s", -0.1114);
            evs.Add("76", -0.1115);
            evs.Add("A6", -0.12);
            //101
            evs.Add("T2", -0.1201);
            evs.Add("95s", -0.1202);
            evs.Add("84", -0.1203);
            evs.Add("62", -0.1204);
            evs.Add("T5s", -0.1205);
            evs.Add("95", -0.1206);
            evs.Add("A5", -0.1207);
            evs.Add("Q7", -0.1208);
            evs.Add("T5", -0.1209);
            evs.Add("87", -0.121);
            //111
            evs.Add("83", -0.1212);
            evs.Add("65", -0.1213);
            evs.Add("Q2s", -0.1214);
            evs.Add("94", -0.1215);
            evs.Add("74", -0.1216);
            evs.Add("54", -0.1217);
            evs.Add("A4", -0.1218);
            evs.Add("T4", -0.1219);
            evs.Add("82", -0.122);
            evs.Add("64", -0.1221);
            //121
            evs.Add("42", -0.1222);
            evs.Add("J7", -0.1223);
            evs.Add("93", -0.1224);
            evs.Add("85s", -0.1225);
            evs.Add("73", -0.1226);
            evs.Add("53", -0.1227);
            evs.Add("T3", -0.1228);
            evs.Add("63", -0.1229);
            evs.Add("K6", -0.123);
            evs.Add("J6", -0.1231);
            //131
            evs.Add("96", -0.1232);
            evs.Add("92", -0.1233);
            evs.Add("72", -0.1234);
            evs.Add("52", -0.1235);
            evs.Add("Q4", -0.13);
            evs.Add("K5", -0.1301);
            evs.Add("J5", -0.1302);
            evs.Add("43s", -0.1303);
            evs.Add("Q3", -0.1304);
            evs.Add("43", -0.1305);
            //141
            evs.Add("K4", -0.1306);
            evs.Add("J4", -0.1307);
            evs.Add("T4s", -0.1308);
            evs.Add("Q6", -0.1309);
            evs.Add("Q2", -0.131);
            evs.Add("J3s", -0.1311);
            evs.Add("J3", -0.1312);
            evs.Add("T3s", -0.1313);
            evs.Add("A3", -0.1314);
            evs.Add("Q5", -0.1315);
            //151
            evs.Add("J2", -0.1316);
            evs.Add("84s", -0.1317);
            evs.Add("82s", -0.140);
            evs.Add("42s", -0.1401);
            evs.Add("93s", -0.1402);
            evs.Add("73s", -0.1403);
            evs.Add("K3", -0.1404);
            evs.Add("J2s", -0.1405);
            evs.Add("92s", -0.1406);
            evs.Add("52s", -0.1407);
            //161
            evs.Add("K2", -0.1408);
            evs.Add("T2s", -0.1409);
            evs.Add("62s", -0.141);
            evs.Add("32", -0.1412);
            evs.Add("A2", -0.15);
            evs.Add("83s", -0.151);
            evs.Add("94s", -0.152);
            evs.Add("72s", -0.153);
            evs.Add("32s", -0.154);

        }
    }
}
