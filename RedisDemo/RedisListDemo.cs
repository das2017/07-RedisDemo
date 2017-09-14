using System;
using System.Collections.Generic;
using RedisDemo.Utils;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace RedisDemo
{
    public class RedisListDemo
    {
        public static void Main1()
        {
            Console.WriteLine("--------------------List--------------------");

            Save();

            Console.ReadLine();

            Query();

            Console.ReadLine();

            Remove();

            Console.ReadLine();
        }

        /// <summary>
        /// 新增，修改
        /// </summary>
        public static void Save()
        {
           using (var client = RedisHelper.GetRedisClient())
           {
               const string namesKey = "150101:savenames";

               // 单个元素添加到List尾部
               client.AddItemToList(namesKey, "Jack");
               Console.WriteLine("单个元素添加在List尾部：");
               client.GetAllItemsFromList(namesKey).PrintDump();

               // 多个元素添加到List尾部
               client.AddRangeToList(namesKey, new List<string> { "Jane", "Jim", "Joe" });
               Console.WriteLine("多个元素添加在List尾部：");
               client.GetAllItemsFromList(namesKey).PrintDump();

               // 单个元素添加到List头部
               client.PrependItemToList(namesKey, "Catherine");
               Console.WriteLine("在起始位置添加单个元素：");
               client.GetAllItemsFromList(namesKey).PrintDump();

               // 多个元素添加到List头部
               client.PrependRangeToList(namesKey, new List<string> { "Tom", "Tim" });
               Console.WriteLine("在起始位置添加多个元素：");
               client.GetAllItemsFromList(namesKey).PrintDump();

               // 根据指定索引设置元素的值
               client.SetItemInList(namesKey, 3, "Chloe");
               Console.WriteLine("修改第4个元素的值：");
               client.GetAllItemsFromList(namesKey).PrintDump();
           }            
        }

        /// <summary>
        /// 查询
        /// </summary>
        public static void Query()
        {
            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:querynames";

                client.AddRangeToList(key, new List<string> { "Dick", "Evan", "Ada", "Florance", "Jane", "Bob", "Jim", "Joe", "Catherine" });
                client.GetAllItemsFromList(key).PrintDump();
                
                Console.WriteLine("List的长度是：{0}", client.GetListCount(key));
                
                Console.WriteLine("第5个元素是：{0}", client.GetItemFromList(key, 4));
                
                Console.WriteLine("从第4个到第7个元素：");
                client.GetRangeFromList(key, 3, 6).PrintDump();
                
                Console.WriteLine("排序之后的第3个到第9个元素：");
                client.GetRangeFromSortedList(key, 2, 8).PrintDump();
            }            
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static void Remove()
        {
            using (var client = RedisHelper.GetRedisClient())
            {
                const string key = "150101:removenames";
                client.AddRangeToList(key, new List<string> { "Ada", "Bob", "Catherine", "Dick", "Evan", "Florance", "Jane", "Jim", "Joe" });
                client.GetAllItemsFromList(key).PrintDump();

                client.RemoveItemFromList(key, "Jane");
                Console.WriteLine("删除'Jane'：");
                client.GetAllItemsFromList(key).PrintDump();

                var startItem = client.RemoveStartFromList(key);
                Console.WriteLine("删除起始元素：{0}", startItem);                
                client.GetAllItemsFromList(key).PrintDump();

                var endItem = client.RemoveEndFromList(key);
                Console.WriteLine("删除末尾元素：{0}", endItem);               
                client.GetAllItemsFromList(key).PrintDump();

                Console.WriteLine("删除所有元素");
                client.RemoveAllFromList(key);
                client.GetAllItemsFromList(key).PrintDump();
            }
        }        
    }
}
