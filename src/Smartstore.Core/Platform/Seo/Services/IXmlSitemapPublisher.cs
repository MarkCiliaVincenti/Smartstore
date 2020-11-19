﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dasync.Collections;
using Microsoft.AspNetCore.Routing;
using Smartstore.Core.Localization;

namespace Smartstore.Core.Seo
{
    public partial interface IXmlSitemapPublisher
    {
        XmlSitemapProvider PublishXmlSitemap(XmlSitemapBuildContext context);
    }

    public abstract class XmlSitemapProvider
    {
        public virtual Task<int> GetTotalCountAsync()
            => Task.FromResult(0);

        public virtual IAsyncEnumerable<NamedEntity> EnlistAsync(CancellationToken cancelToken = default)
            => AsyncEnumerable.Empty<NamedEntity>();

        public virtual IAsyncEnumerable<XmlSitemapNode> EnlistNodesAsync(Language language, CancellationToken cancelToken = default)
            => AsyncEnumerable.Empty<XmlSitemapNode>();

        public virtual XmlSitemapNode CreateNode(LinkGenerator linkGenerator, string baseUrl, NamedEntity entity, UrlRecordCollection slugs, Language language)
        {
            var slug = slugs.GetSlug(language.Id, entity.Id, true);
            var path = linkGenerator.GetPathByRouteValues(entity.EntityName, new { SeName = slug }).EmptyNull().TrimStart('/');
            var loc = baseUrl + path;

            return new XmlSitemapNode
            {
                LastMod = entity.LastMod,
                Loc = loc
            };
        }

        public virtual int Order { get; }
    }
}
