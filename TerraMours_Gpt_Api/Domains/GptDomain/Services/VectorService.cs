﻿using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Req;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Res;
using TerraMours_Gpt_Api.Domains.GptDomain.IServices;

namespace TerraMours_Gpt_Api.Domains.GptDomain.Services {
    public class VectorService : IVectorService
    {
        public Task<ApiResponse<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<VectorRes>>> GetList(int KnowledgeId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<VectorRes>>> Query(VectorQueryReq req)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> Update(VectorUpdateReq req)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<VectorRes>> Upsert(VectorReq req)
        {
            throw new NotImplementedException();
        }
    }
}
