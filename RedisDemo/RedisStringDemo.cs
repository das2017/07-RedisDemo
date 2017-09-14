using System;
using RedisDemo.Models;
using RedisDemo.Utils;
using ServiceStack.Text;

namespace RedisDemo
{
    public class RedisStringDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("-----------------String------------------");

            using (var client = RedisHelper.GetRedisClient())
            {
                // 保存一个字符串
                const string key = "150101:name";
                client.SetValue(key, "Jack", TimeSpan.FromMinutes(5));
                Console.WriteLine("{0}的值是：{1}", key, client.GetValue(key));

                Console.ReadLine();

                // 保存一个对象
                var newPerson = new PersonModel { Id = 1, Name = "Jack", Age = 19, Telephone = "87976562" };
                var personKey = "150101:person:1";
                client.Set<PersonModel>(personKey, newPerson, TimeSpan.FromMinutes(5));
                Console.WriteLine("{0}的值是：", personKey);
                client.Get<PersonModel>(personKey).PrintDump();

                Console.ReadLine();

                // 整数自增/自减
                const string counterKey = "150101:counter";
                client.SetValue(counterKey, "12");
                
                client.IncrementValue(counterKey).Print();
                client.IncrementValueBy(counterKey, 5).Print();

                client.DecrementValue(counterKey).Print();
                client.DecrementValueBy(counterKey, 12).Print();
            }

            Console.ReadLine();
        }
    }
}
