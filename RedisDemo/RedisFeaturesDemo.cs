using System;
using System.Linq;
using RedisDemo.Utils;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace RedisDemo
{
    /// <summary>
    /// Redis 特性
    /// </summary>
    public class RedisFeaturesDemo
    {
        public static void Main1()
        {
            Pipelining();

            Transaction();

            Redlock();

            Geo();  

            Console.ReadLine();
        }

        /// <summary>
        /// 管道
        /// </summary>
        private static void Pipelining()
        {
            Console.WriteLine("---------------------Pipelining----------------------");

            using (var client = RedisHelper.GetRedisClient())
            {
                const string stringKey = "150101:pipeline:string";
                const string setKey = "150101:pipeline:set";

                using (var pipeline = client.CreatePipeline())
                {
                    pipeline.QueueCommand(cli => cli.SetValue(stringKey, "stringvalue"));
                    pipeline.QueueCommand(cli => cli.AddItemToSet(setKey, "12"));

                    pipeline.Flush();
                }

                Console.WriteLine("{0}的值为：{1}", stringKey, client.GetValue(stringKey));
                Console.WriteLine("{0}的元素：{1}", setKey, client.GetAllItemsFromSet(setKey).Dump());
            }
        }

        /// <summary>
        /// 事务
        /// </summary>
        private static void Transaction()
        {
            Console.WriteLine("---------------------Transaction----------------------");

            using (var client = RedisHelper.GetRedisClient())
            {                
                //bool somethingWrong = false;
                bool somethingWrong = true;
                const string stringKey = "150101:tran:string";
                const string hashKey = "150101:tran:hash";
                const string listKey = "150101:tran:list";

                using (var transaction = client.CreateTransaction())
                {
                    try
                    {
                        transaction.QueueCommand(cli => cli.SetValue(stringKey, "teststring", TimeSpan.FromSeconds(180)));
                        transaction.QueueCommand(cli => cli.SetEntryInHash(hashKey, "hashfield", "hashvalue"));
                        transaction.QueueCommand(cli => cli.AddItemToList(listKey, "listitem1"));
                        if (somethingWrong) throw new Exception();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }

                Console.WriteLine("{0}的值为：{1}", stringKey, client.GetValue(stringKey));
                Console.WriteLine("{0}的元素：{1}", hashKey, client.GetAllEntriesFromHash(hashKey).Dump());
                Console.WriteLine("{0}的元素：{1}", listKey, client.GetAllItemsFromList(listKey).Dump());
            }
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        private static void Redlock()
        {
            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:locker";
                const string counterKey = "150101:counter";

                client.SetValue(counterKey, "56");
                using (client.AcquireLock(key, TimeSpan.FromSeconds(10)))
                {
                    client.SetValue(counterKey, "85");
                    client.GetValue(counterKey).Print();
                }
            }
        }

        /// <summary>
        /// 地理信息
        /// </summary>
        private static void Geo()
        {
            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:cities";

                client.AddGeoMembers(key, new[]
                {
                    new RedisGeo(113.14, 23.08, "广州"),
                    new RedisGeo(113.06, 23.02, "佛山"),
                    new RedisGeo(114.22, 23.05, "惠州"),
                    new RedisGeo(114.07, 22.33, "深圳"),
                    new RedisGeo(113.34, 22.17, "珠海"),
                    new RedisGeo(117.17, 31.52, "合肥"),
                    new RedisGeo(116.24, 39.55, "北京"),
                    new RedisGeo(103.51, 36.04, "兰州"),
                    new RedisGeo(106.42, 26.35, "贵阳"),
                    new RedisGeo(110.20, 20.02, "海口"),
                    new RedisGeo(114.30, 38.02, "石家庄"),
                    new RedisGeo(113.40, 34.46, "郑州"),
                    new RedisGeo(126.36, 45.44, "哈尔滨"),
                    new RedisGeo(114.17, 30.35, "武汉"),
                    new RedisGeo(112.59, 28.12, "长沙"),
                    new RedisGeo(125.19, 43.54, "长春"),
                    new RedisGeo(118.46, 32.03, "南京"),
                    new RedisGeo(115.55, 28.40, "南昌"),
                    new RedisGeo(123.25, 41.48, "沈阳"),
                    new RedisGeo(101.48, 36.38, "西宁"),
                    new RedisGeo(117, 36.40, "济南"),
                    new RedisGeo(112.33, 37.54, "太原"),
                    new RedisGeo(108.57, 34.17, "西安"),
                    new RedisGeo(121.29, 31.14, "上海"),
                    new RedisGeo(104.04, 30.40, "成都"),
                    new RedisGeo(117.12, 39.02, "天津"),
                    new RedisGeo(91.08, 29.39, "拉萨"),
                    new RedisGeo(87.36, 43.35, "乌鲁木齐"),
                    new RedisGeo(102.42, 25.04, "昆明"),
                    new RedisGeo(120.10, 30.16, "杭州"),
                    new RedisGeo(106.33, 29.35, "重庆"),
                });

                Console.Write("武汉到广州的距离为：");
                var distance = client.CalculateDistanceBetweenGeoMembers(key, "武汉", "广州", "km");
                Console.WriteLine("{0}公里", distance);

                Console.WriteLine("查找武汉周围1000公里范围内的城市：");
                client.FindGeoMembersInRadius(key, "武汉", 1000, "km").PrintDump();

                Console.WriteLine("查找武汉周边500公里范围内的城市，并显示距离，且按照距离排序：");
                var geoResults = client.FindGeoResultsInRadius(key, "武汉", 500, "km", sortByNearest: true);
                geoResults.Select(i => new
                {
                    i.Member,
                    Distance = i.Distance + i.Unit
                }).ToArray().PrintDump();
            }
        }
    }
}
