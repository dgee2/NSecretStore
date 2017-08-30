using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSecretStore.Abstractions;

namespace NSecretStore
{
    public class NSecretStoreMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TemplateMatcher _requestMatcher;
        private readonly NSecretStoreOptions _options;
        private readonly IDictionary<string, Action<HttpRequest>> _actions;
        private readonly ISecretReader _secretReader;

        public NSecretStoreMiddleware(
            RequestDelegate next,
            NSecretStoreOptions options, ISecretReader secretReader)
        {
            _next = next;
            _options = options;
            _secretReader = secretReader;
            _requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.BaseAddress), new RouteValueDictionary());
            _actions = new Dictionary<string, Action<HttpRequest>>
            {
                { "GET", GetSecret },
                { "POST", PostSecret }
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!ProcessRequest(httpContext.Request))
            {
                await _next(httpContext);
            }
        }

        private bool ProcessRequest(HttpRequest request)
        {
            if (!RequestingSecret(request, out string method) || !_actions.ContainsKey(method)) return false;

            _actions[method](request);
            return true;
        }

        private bool RequestingSecret(HttpRequest request, out string method)
        {
            method = null;
            var routeValues = new RouteValueDictionary();
            if (!_requestMatcher.TryMatch(request.Path, routeValues)) return false;

            method = request.Method;
            return true;
        }

        private void GetSecret(HttpRequest request)
        {
            if (!request.Query.ContainsKey("id"))
            {
                throw new KeyNotFoundException("id not passed");
            }
            request.HttpContext.Response.WriteAsync(_secretReader.GetSecret(request.Query["id"]));
        }

        private void PostSecret(HttpRequest request)
        {
            using (var body = new StreamReader(request.Body))
            {
                var model = JsonConvert.DeserializeObject<Model>(body.ReadToEnd());
                _secretReader.SetSecret(model.Id, model.Value);
            }
        }

        private void DeleteSecret(HttpRequest request)
        {
            if (!request.Query.ContainsKey("id"))
            {
                throw new KeyNotFoundException("id not passed");
            }
            _secretReader.DeleteSecret(request.Query["id"]);
        }

        private class Model
        {
            public string Id { get; set; }
            public string Value { get; set; }
        }
    }
}