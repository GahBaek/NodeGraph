using NodeNetwork.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Services
{
    /*
     *  Page 단위 관리, 실행에 집중.
     */
    public interface IPageManager
    {
        Guid RegisterPage(IPage page);
        bool RemovePage(Guid pageId);
        IPage GetPage(Guid pageId);
        IContext Run(Guid pageId, IContext ctx);

        void SavePage(IPage page, string path);
        IPage LoadPage(string path);
    }

    public sealed class PageManager : IPageManager
    {
        private readonly Dictionary<Guid, IPage> _pages = new();

        public Guid RegisterPage(IPage page) { _pages[page.Id] = page; return page.Id; }
        public bool RemovePage(Guid pageId) => _pages.Remove(pageId);
        public IPage GetPage(Guid pageId) => _pages.TryGetValue(pageId, out var p)
            ? p : throw new KeyNotFoundException($"{pageId}");

        public IContext Run(Guid pageId, IContext ctx)
        {
            var page = GetPage(pageId);
            return page.Exec(ctx);
        }
        public void SavePage(IPage page, string path)
        {
            // 직렬화
        }

        public IPage LoadPage(string path)
        {
            // 역직렬화
            throw new NotImplementedException();
        }
    }


}
