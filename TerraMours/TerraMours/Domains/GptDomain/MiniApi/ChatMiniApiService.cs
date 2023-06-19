﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Serilog;
using StackExchange.Redis;
using System.IO.Pipelines;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt.Domains.GptDomain.Contracts.Req;
using TerraMours_Gpt.Domains.GptDomain.Contracts.Res;
using TerraMours_Gpt.Domains.GptDomain.IServices;
using TerraMours_Gpt.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt.Framework.Infrastructure.Contracts.GptModels;

namespace TerraMours_Gpt.Domains.GptDomain.MiniApi {
    public class ChatMiniApiService : ServiceBase {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IChatService _chatService;

        public ChatMiniApiService(IHttpContextAccessor httpContextAccessor, IChatService chatService) : base() {
            _httpContextAccessor = httpContextAccessor;
            _chatService = chatService;
            App.MapPost("/api/v1/Chat/ChatStream", ChatStream);

            App.MapPost("/api/v1/Chat/ImportSensitive", ImportSensitive);
            App.MapGet("/api/v1/Chat/AddSensitive", AddSensitive);
            App.MapGet("/api/v1/Chat/ChangeSensitive", ChangeSensitive);
            App.MapGet("/api/v1/Chat/DeleteSensitive", DeleteSensitive);
            App.MapPost("/api/v1/Chat/SensitiveList", SensitiveList);

            App.MapGet("/api/v1/Chat/UpdateKeyOptionsBalance", UpdateKeyOptionsBalance);
            App.MapGet("/api/v1/Chat/AddKeyOptions", AddKeyOptions);
            App.MapGet("/api/v1/Chat/ChangeKeyOptions", ChangeKeyOptions);
            App.MapGet("/api/v1/Chat/DeleteKeyOptions", DeleteKeyOptions);
            App.MapPost("/api/v1/Chat/KeyOptionsList", KeyOptionsList);
            App.MapGet("/api/v1/Chat/CheckBalance", CheckBalance);

            App.MapGet("/api/v1/Chat/AddChatConversation", AddChatConversation);
            App.MapGet("/api/v1/Chat/ChangeChatConversation", ChangeChatConversation);
            App.MapGet("/api/v1/Chat/DeleteChatConversation", DeleteChatConversation);
            App.MapPost("/api/v1/Chat/ChatConversationList", ChatConversationList);

            App.MapGet("/api/v1/Chat/DeleteChatRecord", DeleteChatRecord);
            App.MapPost("/api/v1/Chat/ChatRecordList", ChatRecordList);
        }
        #region 聊天记录

        /// <summary>
        /// Chat聊天接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Authorize]
        [Produces("application/octet-stream")]
        public async IAsyncEnumerable<string> ChatStream(ChatReq req) {
            if (_httpContextAccessor.HttpContext?.Items["key"] !=null) {
                req.Key = _httpContextAccessor.HttpContext?.Items["key"]?.ToString();
            }
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            req.UserId=userId;
            req.IP = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            //接口返回的完整内容
            string totalMsg = "";
            await foreach (string msg in _chatService.ChatProcessStream(req)) {
                totalMsg += msg;
                yield return totalMsg;
            }
        }

        /// <summary>
        /// 删除聊天记录
        /// </summary>
        /// <param name="recordId">id</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> DeleteChatRecord(long recordId) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.DeleteChatRecord(recordId, userId);
            return Results.Ok(res);
        }
        /// <summary>
        /// 聊天记录列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ChatRecordList(ChatRecordReq page) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.ChatRecordList(page);
            return Results.Ok(res);
        }
        #endregion
        #region 敏感词

        /// <summary>
        /// 导入敏感词字典
        /// </summary>
        /// <param name="file">敏感词字典文件</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ImportSensitive(IFormFile file)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res =await _chatService.ImportSensitive(file, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 添加敏感词
        /// </summary>
        /// <param name="word">敏感词</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> AddSensitive(string word)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res =await _chatService.AddSensitive(word, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 修改敏感词
        /// </summary>
        /// <param name="sensitiveId">主键id</param>
        /// <param name="word">敏感词</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ChangeSensitive(long sensitiveId, string word)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res =await _chatService.ChangeSensitive(sensitiveId,word, userId);
            return Results.Ok(res);
        }
        /// <summary>
        /// 删除敏感词
        /// </summary>
        /// <param name="sensitiveId"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> DeleteSensitive(long sensitiveId)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res =await _chatService.DeleteSensitive(sensitiveId, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 敏感词列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> SensitiveList(PageReq page)
        {
            var res = await _chatService.SensitiveList(page);
            return Results.Ok(res);
        }
        #endregion

        #region Key管理

        /// <summary>
        /// 更新key池的额度使用情况
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> UpdateKeyOptionsBalance()
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.UpdateKeyOptionsBalance(userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 添加key
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> AddKeyOptions(string apiKey)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.AddKeyOptions(apiKey, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 修改key
        /// </summary>
        /// <param name="keyId">id</param>
        /// <param name="apiKey">apiKey</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ChangeKeyOptions(long keyId, string apiKey)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.ChangeKeyOptions(keyId, apiKey, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 删除指定key
        /// </summary>
        /// <param name="keyId">id</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> DeleteKeyOptions(long keyId)
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.DeleteKeyOptions(keyId, userId);
            return Results.Ok(res);
        }
        /// <summary>
        /// key配置列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> KeyOptionsList(PageReq page)
        {
            var res = await _chatService.KeyOptionsList(page);
            return Results.Ok(res);
        }
        /// <summary>
        /// 检查余额，公共方法
        /// </summary>
        /// <param name="key">openai秘钥</param>
        /// <returns></returns>
        public async Task<IResult> CheckBalance(string key)
        {
            var res = await _chatService.CheckBalance(key);
            return Results.Ok(res);
        }
        #endregion

        #region 会话列表

        /// <summary>
        /// 添加会话
        /// </summary>
        /// <param name="conversationName">会话名称</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> AddChatConversation(string conversationName) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.AddChatConversation(conversationName, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 修改会话
        /// </summary>
        /// <param name="conversationId">id</param>
        /// <param name="conversationName">会话名称</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ChangeChatConversation(long conversationId, string conversationName) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.ChangeChatConversation(conversationId, conversationName, userId);
            return Results.Ok(res);
        }

        /// <summary>
        /// 删除会话
        /// </summary>
        /// <param name="conversationId">id</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> DeleteChatConversation(long conversationId) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.DeleteChatConversation(conversationId, userId);
            return Results.Ok(res);
        }
        /// <summary>
        /// 会话列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IResult> ChatConversationList(PageReq page) {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            var res = await _chatService.ChatConversationList(page,userId);
            return Results.Ok(res);
        }

        #endregion

    }
}
