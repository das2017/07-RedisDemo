using System;
using RedisDemo.Models;
using RedisDemo.Utils;
using ServiceStack.Text;

namespace RedisDemo
{
    public class RedisHashDemo
    {
        public static void Main1(string[] args)
        {
            Console.WriteLine("-----------------------Hash-------------------------");

            using (var client = RedisHelper.GetRedisClient())
            {
                var article = new ArticleModel
                {
                    Id = 18,
                    Title = "滴滴出行悄然提价 中国打车市场补贴战或将终结",
                    Views = 10,
                    Favourites = 0
                };
                const string articleKey = "150101:article:18";

                // 设置Hash中的多个字段
                client.SetRangeInHash(articleKey, article.ToStringDictionary());
                client.GetAllEntriesFromHash(articleKey).PrintDump();

                // 设置Hash中的单个字段
                client.SetEntryInHash(articleKey, "Content", "测试文章内容");
                client.GetAllEntriesFromHash(articleKey).PrintDump();

                // 对Hash中整数类型的字段做自增操作
                client.IncrementValueInHash(articleKey, "Views", 1).Print();

                client.GetAllEntriesFromHash(articleKey).PrintDump();
            }

            Console.ReadLine();
        }
    }
}
