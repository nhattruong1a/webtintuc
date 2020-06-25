using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTinTuc.Models;

namespace WebTinTuc.Controllers
{
    public class NewsController : Controller
    {
        // GET: News

        dbWebTinTucDataContext data = new dbWebTinTucDataContext();

        private List<New> GetNews(int count)
        {
            return data.News.OrderByDescending(a => a.DateSubmitted).Take(count).ToList();
        }

        public ActionResult News()
        {
            var news = GetNews(5);
            return PartialView(news);
        }

        public ActionResult ListNews()
        {
            var listnews = data.ListNews();
            return PartialView(listnews);
        }

        public ActionResult Details(int id)
        {
            var news = from News in data.News where News.ID == id select News;
            return View(news.Single());
        }
    }
}