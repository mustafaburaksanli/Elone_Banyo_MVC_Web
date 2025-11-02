using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;
namespace EloneBanyo.Controllers
{
    public class HomeController : Controller
    {

        private CategoryManager cm = new CategoryManager();
        private CategorySubtitleManager csm = new CategorySubtitleManager();
        ProductManager pm = new ProductManager();
        OfferManager om = new OfferManager();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult GetSubcategories(int id)
        {
            var subcategories = csm.GetAllBL().Where(x => x.CategoryID == id).ToList();
            return PartialView("_SubcategoriesPartial", subcategories);
        }
        public ActionResult GetCategories()
        {
            var categoryValues = cm.GetAllBL();
            return PartialView("_CategoriesPartial", categoryValues);
        }

        public ActionResult Product(int id, int page=1)
        {
            var productValues = pm.GetAllBL().Where(x => x.SubtitleID == id && x.ProductStatus == true).ToPagedList(page,6);
            return PartialView("Product", productValues);
        }

        [HttpGet]
        public ActionResult Offer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Offer(Contact p)
        {
            if (ModelState.IsValid)
            {
                var response = Request["g-recaptcha-response"];
                var client = new HttpClient();
                var secretKey = "6Ld2rUwqAAAAAFYUvdCfuXHsKWXAo6XaGPRt3HQa";
                var result = client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}").Result;
                dynamic jsonData = JsonConvert.DeserializeObject(result);

                if (jsonData.success == true)
                {
                    p.Date = DateTime.Now;
                    p.Status = false;
                    om.CategoryAddBL(p);
                    TempData["SuccessMessage"] = "Teklifiniz Gönderildi.";
                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "Lütfen doğrulamayı işaretleyiniz.";
                    return View();
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Teklifiniz Gönderilemedi. Boşluk olmadığından emin olun.";
                return View();
            }
        }
    }
}

   
