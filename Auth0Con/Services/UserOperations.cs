using System;
using System.Collections.Generic;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0Con.Utilities;
using AutoMapper;
using AutoMapper.XpressionMapper.Extensions;
using RestSharp;

namespace Auth0Con.Services
{
    internal class UserOperations : IUserOperations
    {
        private readonly string _audience;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly IMapper _mapper;
        private Lazy<Token> _token;
        private Lazy<ManagementApiClient> _client;

        public UserOperations(string audience, string clientId, string clientSecret, IMapper mapper = null)
        {
            _audience = audience;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _mapper = mapper;
            _token = new Lazy<Token>(GetToken);
            _client = new Lazy<ManagementApiClient>(GetClient);
        }

        private ManagementApiClient GetClient()
        {
            return new ManagementApiClient(_token.Value.access_token, new Uri(_audience));
        }

        private Token GetToken()
        {
            var client = new RestClient("https://senlabltd.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            var value = $"{{\"client_id\":\"{_clientId}\",\"client_secret\":\"{_clientSecret}\",\"audience\":\"{_audience}\",\"grant_type\":\"client_credentials\"}}";
            request.AddParameter("application/json", value, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(response.Content);
            return token;
        }

        #region Implementation of IUserOperations

        public IEnumerable<RegisteredUser> GetAllUsers()
        {
            return new RowBuffer<RegisteredUser>(FetchUsers);
        }

        private IEnumerable<RegisteredUser> FetchUsers()
        {
            foreach (var user in _client.Value.Users.GetAllAsync().Result)
            {
                var registered = Map<RegisteredUser>(user);
                yield return registered;
            }
        }

        private TDest Map<TDest>(object user)
        {
            if (_mapper == null)
                return Mapper.Map<TDest>(user);

            return _mapper.Map<TDest>(user);
        }

        #endregion

        public static void Map(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, RegisteredUser>();
        }

        public RegisteredUser AddUser(string email, string password)
        {
            var userReq = new UserCreateRequest
            {
                Connection = "Username-Password-Authentication",
                Email = email,
                Password = password,
                EmailVerified = false,
                VerifyEmail = true,
            };
            var user = _client.Value.Users.CreateAsync(userReq).Result;
            return Map<RegisteredUser>(user);
        }

        public void DeleteUser(string id)
        {
            _client.Value.Users.DeleteAsync(id).Wait();
        }

        public void BlockUser(string id)
        {
            var updateRequest = new UserUpdateRequest
            {
                Blocked = true
            };
            _client.Value.Users.UpdateAsync(id, updateRequest).Wait();
        }

        public void UnblockUser(string id)
        {
            var updateRequest = new UserUpdateRequest
            {
                Blocked = false
            };
            _client.Value.Users.UpdateAsync(id, updateRequest).Wait();
        }
    }
}