using TerraMours.Framework.Infrastructure.Contracts.SystemModels;

namespace TerraMours_Gpt.Framework.Infrastructure.Contracts.ProductModels
{
    /// <summary>
    /// 商品表
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 商品折扣 默认不打折
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// 商品图片路径
        /// </summary>
        public string? ImagePath { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int? Stock { get; set; }
        /// <summary>
        /// 是否是Vip(包月会员)
        /// </summary>
        public bool? IsVIP { get; set; }
        /// <summary>
        /// 会员等级 (1 为月度会员，2为极度会员，3会年度会员这与 价格相关，由于user表已经设计了，下次遇到相似问题设计为enum类)
        /// </summary>
        public int? VipLevel { get; set; }
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 商品分类
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 初始化商品实体类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="categoryId"></param>
        /// <param name="stock"></param>
        /// <param name="isVIP"></param>
        /// <param name="vipLevel"></param>
        public Product(string name, string description, decimal price, long categoryId, int? stock, bool? isVIP, int? vipLevel)
        {
            //初始化用户 ：以下是有用的字段
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.CategoryId = categoryId;
            this.IsVIP = isVIP;
            this.VipLevel = vipLevel;
            //EntityBase
            Enable = true;
            CreateDate = DateTime.Now;
            this.Stock = stock;
        }

        /// <summary>
        /// 修改商品实体类信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="categoryId"></param>
        /// <param name="isVIP"></param>
        /// <param name="vipLevel"></param>
        /// <returns></returns>
        public Product UpdateProduct(string name, string description, decimal price, long categoryId, bool? isVIP, int? vipLevel)
        {
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.IsVIP = isVIP;
            this.VipLevel = vipLevel;
            this.CategoryId = categoryId;
            this.ModifyDate = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 下架分类 或者删除分类
        /// </summary>
        /// <returns></returns>
        public Product DeleteProduct()
        {
            this.Enable = false;
            this.ModifyDate = DateTime.Now;
            return this;
        }

    }
}
