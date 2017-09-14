using System;
using System.Collections.Generic;
using RedisDemo.Utils;
using ServiceStack.Text;

namespace RedisDemo
{
    public class RedisSortedSetDemo
    {
        public static void Main1()
        {
            Console.WriteLine("-----------------------Sorted Set----------------------");

            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:students";

                // 添加单个
                client.AddItemToSortedSet(key, "Jack", 96);
                client.GetAllItemsFromSortedSet(key).PrintDump();

                // 添加多个
                client.AddRangeToSortedSet(key, new List<string> { "Jane", "Jim", "Tony", "Mary", "Catherine" }, 92);

                // 获取所有元素
                Console.WriteLine("获取Sorted Set中的所有元素：");
                client.GetAllItemsFromSortedSet(key).PrintDump();

                // 获取指定范围内的元素，并且包含元素的score
                Console.WriteLine("获取指定范围的元素（包含score）：");
                client.GetRangeWithScoresFromSortedSet(key, 1, 8).PrintDump();
            }

            Console.ReadLine();
        }
    }
}
