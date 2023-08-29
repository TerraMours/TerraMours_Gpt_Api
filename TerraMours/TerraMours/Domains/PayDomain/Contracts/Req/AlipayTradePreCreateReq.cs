using System.ComponentModel.DataAnnotations;

namespace TerraMours_Gpt.Domains.PayDomain.Contracts.Req
{
    public class AlipayTradePreCreateReq
    {
        /*[Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "subject")]
        public string Subject { get; set; }

        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_amount")]
        public string TotalAmount { get; set; }*/

        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public long ProductId { get; set; }
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
        public decimal Discount { get; set; }
        /// <summary>
        /// 商品分类Id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 是否是Vip(包月会员)
        /// </summary>
        public bool? IsVIP { get; set; }
        /// <summary>
        /// 会员等级 (1 为月度会员，2为极度会员，3会年度会员这与 价格相关，由于user表已经设计了，下次遇到相似问题设计为enum类)
        /// </summary>
        public int? VipLevel { get; set; }

    }
}
