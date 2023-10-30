using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mbill.Core.Domains.Entities.Bill
{
    [MongoCollection(Name = "bill_search_record")]
    public class BillSearchRecordEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public long UserId { get; set; }


        /// <summary>
        /// 用户
        /// </summary>
        public long UserBId { get; set; }

        /// <summary>
        /// 账单类型
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 分类BId
        /// </summary>
        public long CategoryBId { get; set; }

        /// <summary>
        /// 账单分类
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 账单账户BId
        /// </summary>
        public long AssetBId { get; set; }

        /// <summary>
        /// 账单账户
        /// </summary>
        public long? AssetId { get; set; }

        /// <summary>
        /// 金额区间最小值
        /// </summary>
        public decimal? AmountMin { get; set; }

        /// <summary>
        /// 金额区间最大值
        /// </summary>
        public decimal? AmountMax { get; set; }

        /// <summary>
        /// 账单时间起始
        /// </summary>
        public DateTime? TimeBegin { get; set; }

        /// <summary>
        /// 账单时间截止
        /// </summary>
        public DateTime? TimeEnd { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 搜索时间
        /// </summary>
        public DateTime SearchTime { get; set; }
    }
}
