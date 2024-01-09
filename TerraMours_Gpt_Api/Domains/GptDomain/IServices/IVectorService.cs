﻿using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Req;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Res;

namespace TerraMours_Gpt_Api.Domains.GptDomain.IServices {
    public interface IVectorService {
        Task<ApiResponse<List<VectorRes>>> GetList(int KnowledgeId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Delete(int id);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<List<VectorRes>>> Query(VectorQueryReq req);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Update(VectorUpdateReq req);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<VectorRes>> Upsert(VectorReq req);
    }
}
