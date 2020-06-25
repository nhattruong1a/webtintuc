using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTuc.Models;

using PagedList;
using PagedList.Mvc;

namespace WebTinTuc.Controllers
{
    public class LayoutController : Controller
    {
        // GET: Layout
        dbWebTinTucDataContext data = new dbWebTinTucDataContext();

        public ActionResult Category()
        {
            var category =from categories in data.categories select categories;
            return PartialView(category);
        }

        public ActionResult Categories(int id,int ? page)
        {
            var news = from News in data.News where News.ID_Category == id select News;
            int pageSize = 5;
            int pageNum = (page ?? 1);
            return View(news.OrderByDescending(a=>a.ID).ToPagedList(pageNum,pageSize));
        }

        private List<New> GetBreakingNews(int count)
        {
            return data.News.OrderByDescending(a => a.DateSubmitted).Take(count).ToList();
        }

        public ActionResult BreakingNews()
        {
            var brn = GetBreakingNews(3);
            return PartialView(brn);
        }

        private List<New> GetInternational(int count)
        {
            var international = from International in data.News where International.ID_Category == 2 select International;
            return international.OrderByDescending(a => a.DateSubmitted).Take(count).ToList();
        }

        public ActionResult International()
        {
            var itt = GetInternational(3);
            return PartialView(itt);
        }

        public ActionResult Advertisement()
        {
            var advertisement = from advertisements in data.Advertisements select advertisements;
            return PartialView(advertisement);
        }

        public ActionResult Advertisement_Product()
        {
            var advertisement = from advertisements in data.Advertisement_Products select advertisements;
            return PartialView(advertisement);
        }
    }
}