using System;
using RedisDemo.Models;
using RedisDemo.Utils;
using ServiceStack.Text;

namespace RedisDemo
{
    /// <summary>
    /// 泛型
    /// </summary>
    public class RedisGenericDemo
    {
        public static void Main1()
        {
            using (var client = RedisHelper.GetRedisClient())
            {
                var typedClient = client.As<ArticleModel>();
                var article = new ArticleModel
                {
                    Id = 3,
                    Title = "Demo Article",
                    Content = "Demo Article Content",
                    Views = 108,
                    Favourites = 12
                };

                // String
                Console.WriteLine("-----------String------------");
                const string stringKey = "150101:articleString:3";
                typedClient.SetValue(stringKey, article);
                Console.WriteLine("{0}的值：{1}{2}", stringKey, Environment.NewLine, typedClient.GetValue(stringKey).Dump());

                Console.ReadLine();

                // List
                Console.WriteLine("-----------List------------");
                const string listKey = "150101:articleList";
                var redisList = typedClient.Lists[listKey];
                typedClient.AddItemToList(redisList, article);
                Console.WriteLine("{0}的所有元素：{1}", listKey, typedClient.GetAllItemsFromList(redisList).Dump());

                Console.ReadLine();

                // Hash
                Console.WriteLine("-----------Hash------------");
                typedClient.StoreAsHash(article);
                typedClient.GetFromHash(article.Id).PrintDump();

                Console.ReadLine();

                // Set
                Console.WriteLine("-----------Set------------");
                const string setKey = "150101:articleSet";
                var redisSet = typedClient.Sets[setKey];
                typedClient.AddItemToSet(redisSet, article);
                typedClient.GetAllItemsFromSet(redisSet).PrintDump();

                Console.ReadLine();

                // Sorted Set
                Console.WriteLine("-----------Sorted Set------------");
                const string sortedSetKey = "150101:articleSortedSet";
                var redisSortedSet = typedClient.SortedSets[sortedSetKey];
                typedClient.AddItemToSortedSet(redisSortedSet, article, article.Views);
                typedClient.GetAllItemsFromSortedSet(redisSortedSet).PrintDump();
            }

            Console.ReadLine();
        }
    }
}
