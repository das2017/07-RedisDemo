using System;
using System.Collections.Generic;
using RedisDemo.Utils;
using ServiceStack.Text;

namespace RedisDemo
{
    public class RedisSetDemo
    {
        public static void Main1()
        {
            Console.WriteLine("------------------Set--------------------");

            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:ids";

                // 添加单个元素
                client.AddItemToSet(key, "12");
                client.GetAllItemsFromSet(key).PrintDump();

                // 添加多个元素
                client.AddRangeToSet(key, new List<string> { "14", "16", "15", "17", "12", "13" });
                client.GetAllItemsFromSet(key).PrintDump();
            
                Console.WriteLine("Set中是否包含'18'：{0}", client.SetContainsItem(key, "18"));
                Console.WriteLine("Set中是否包含'16'：{0}", client.SetContainsItem(key, "16"));
             
                Console.WriteLine("从Set中随机获取一个元素：{0}", client.GetRandomItemFromSet(key));
          
                Console.WriteLine("从Set中随机移除并返回被移除的这个元素：{0}", client.PopItemFromSet(key));

                const string key1 = "150101:ids1";
                client.AddRangeToSet(key1, new List<string> { "18", "19", "11", "2", "15" });

                // 取几个集合的交集
                client.GetIntersectFromSets(key, key1).PrintDump();
                // 取几个集合的并集
                client.GetUnionFromSets(key, key1).PrintDump();
            }

            Console.ReadLine();
        }
    }
}
