namespace RedisDemo.Models
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 查看数
        /// </summary>
        public int Views { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favourites { get; set; }
    }
}
