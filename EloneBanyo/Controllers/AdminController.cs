using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace EloneBanyo.Controllers
{
   
    public class AdminController : Controller
    {
        // GET: Admin       
        #region Object
        private CategoryManager cm = new CategoryManager();
        private CategorySubtitleManager csm = new CategorySubtitleManager();
        ProductManager pm = new ProductManager();
        OfferManager om = new OfferManager();
        UserManager um = new UserManager();
        #endregion
        #region Hash 
    private string HashPasswordBcrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string enteredPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
    }

    #endregion
        #region Login And Logout       
       [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(User user, string returnUrl)
        {
            var response = Request["g-recaptcha-response"];
            var client = new HttpClient();
            var secretKey = "6LfCy08qAAAAALVxFij6k407sowdgyw31BzEDLkU";
            var result = client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}").Result;
            dynamic jsonData = JsonConvert.DeserializeObject(result);
            if (jsonData.success == true)
            {
                var userFromDb = um.GetAllBL().FirstOrDefault(x => x.UserEmail == user.UserEmail);
                if (userFromDb != null && VerifyPassword(user.UserPassword, userFromDb.UserPassword))
                {
                    FormsAuthentication.SetAuthCookie(user.UserEmail, false);
                    Session["Ad"] = userFromDb.UserName;
                    Session["Soyad"] = userFromDb.UserSurname;
                    Session["KullaniciID"] = userFromDb.UserID;
                    return Redirect(returnUrl ?? Url.Action("Users", "Admin"));
                }
         
            else
            {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı. Tekrar deneyiniz";
                    return View();
            }  
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen doğrulama kutucuğunu işaretleyiniz.";
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Admin");
        }
        #endregion
        #region Table List
        [HttpGet]
        public ActionResult Users(int page=1)
        {
            var userValues = um.GetAllBL().Where(x=>x.Status==true).ToPagedList(page,3);
            return PartialView("Users", userValues);
        }

        [HttpGet]
        public ActionResult Subtitle(int page=1)
        {
            var subtitleValues = csm.GetAllBL().Where(x=>x.SubtitleStatus==true).ToPagedList(page,3);
            return PartialView("Subtitle", subtitleValues);
        }

        public ActionResult GetSubtitle()
        {
            var subtitle = csm.GetAllBL();
            return PartialView("_GetSubtitle", subtitle);
        }

        public ActionResult GetCategories()
        {
            var categories = cm.GetAllBL();
            return PartialView("_GetCategory", categories);
        }

        [HttpGet]
        public ActionResult Product(int page=1)
        {
            var productValues = pm.GetAllBL().Where(x => x.ProductStatus == true).ToPagedList(page, 3); 
            return PartialView("Product", productValues);
        }

        [HttpGet]
        public ActionResult Offer(int page=1)
        {
            var offerValues = om.GetAllBL().Where(x=>x.Status==false).ToPagedList(page,3);
            return PartialView("Offer", offerValues);
        }
        [HttpGet]
        public ActionResult Category(int page=1)
        {
            var categoryValues = cm.GetAllBL().Where(x=>x.CategoryStatus==true).ToPagedList(page,3);
            return PartialView("Category", categoryValues);
        }

        [HttpGet]
        public ActionResult ReadOffer(int page=1)
        {
            var readofferValues = om.GetAllBL().Where(x=>x.Status==true).ToPagedList(page,3);
            return PartialView("ReadOffer", readofferValues);
        }
        #endregion
        #region Page Views

        //SAYFA GÖRÜNÜMLERİ
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddSubtitle()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        #endregion
        #region Add Post
        [HttpPost]
        public ActionResult AddUser(User user)
        {
            var control = um.GetAllBL().Any(x => x.UserEmail==user.UserEmail);
            if(!control)
            {
    if (ModelState.IsValid)
            {
                try
                {
                    var addUser = new User
                    {
                        UserName = user.UserName,
                        UserSurname = user.UserSurname,
                        UserEmail = user.UserEmail,
                        UserPassword = HashPasswordBcrypt(user.UserPassword),
                        Status = true
                    };
                    um.AddBL(addUser);
                    TempData["SuccessMessage"] = "Kullanıcı Başarıyla Eklendi.";
                    return RedirectToAction("Users", "Admin");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Users", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Kullanıcı bilgileri geçersiz.";
                return RedirectToAction("Users");
            }
            }
            else
            {
                TempData["ErrorMessage"] = "Böyle bir kullanıcı zaten mevcut.";
                return RedirectToAction("Users");
            }
        
        }

        [HttpPost]
        public ActionResult AddSubtitle(CategorySubtitle subtitle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addSubtitle = new CategorySubtitle
                    {
                        SubtitleName = subtitle.SubtitleName,
                        CategoryID = subtitle.CategoryID,
                        UserID = HttpContext.Session["KullaniciID"] as int?,
                        SubtitleStatus = true
                    };
                    csm.CategoryAddBL(addSubtitle);
                    TempData["SuccessMessage"] = "Altbaşlık Başarıyla Eklendi.";
                    return RedirectToAction("Subtitle", "Admin");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Altbaşlık eklenirken bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Subtitle", "Admin");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Altbaşlık bilgileri geçersiz.";
                return RedirectToAction("Subtitle", "Admin");
            }
        }
        [HttpPost]
        public ActionResult AddProduct(HttpPostedFileBase ProductImage, Product product)
        {

            if (ProductImage != null && ProductImage.ContentLength > 0 && ModelState.IsValid)
            {
                try
                {
                    string fileName = Path.GetFileName(ProductImage.FileName);
                    string folderPath = Server.MapPath("~/assets/urunler");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string path = Path.Combine(folderPath, fileName);
                    ProductImage.SaveAs(path);

                    var addProduct = new Product
                    {
                        ProductName = product.ProductName,
                        ProductDescription = product.ProductDescription,
                        ProductImage = "/assets/urunler/" + fileName,
                        SubtitleID = product.SubtitleID,
                        UserID = Convert.ToInt32(HttpContext.Session["KullaniciID"]),
                        ProductStatus = true,
                        ProductAdditionDate = DateTime.Now.AddTicks(-(DateTime.Now.Ticks % TimeSpan.TicksPerSecond))
                    };

                    // addProduct kullanarak veritabanına ekleme yap
                    pm.CategoryAddBL(addProduct);

                    TempData["SuccessMessage"] = "Ürün Başarıyla Eklendi.";
                    return RedirectToAction("Product", "Admin");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Ürün eklenirken bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Product", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Ürün bilgileri geçersiz.";
                return RedirectToAction("Product", "Admin");
            }

        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addCategory = new Category
                    {
                        CategoryName = category.CategoryName,
                        CategoryStatus = true,
                        UserID = Convert.ToInt32(HttpContext.Session["KullaniciID"]),
                    };
                    cm.CategoryAddBL(addCategory);
                    TempData["SuccessMessage"] = "Kategori Başarıyla Eklendi.";
                    return RedirectToAction("Category", "Admin");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Kategori eklenirken bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Category", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Kategori bilgileri geçersiz.";
                return RedirectToAction("Category", "Admin");
            }
        }
        #endregion
        #region Delete Post
        [HttpPost]
        public ActionResult UsersDelete(int id)
        {
            User user = um.GetAllBL().FirstOrDefault(x => x.UserID == id);
            if (user != null)
            {
                user.Status = false;
                um.UpdateeBL(user);
            }
            return RedirectToAction("Users", "Admin");
        }
        [HttpPost]
        public ActionResult SubtitleDelete(int id)
        {
            CategorySubtitle subtitle = csm.GetAllBL().FirstOrDefault(x => x.SubtitleID == id);
            if (subtitle != null)
            {
                subtitle.SubtitleStatus = false;
                csm.UpdateeBL(subtitle);
            }
            return RedirectToAction("Subtitle", "Admin");
        }
        [HttpPost]
        public ActionResult ProductDelete(int id)
        {
            Product product = pm.GetAllBL().FirstOrDefault(x => x.ProductID == id);
            if (product != null)
            {
                product.ProductStatus = false;
                pm.UpdateeBL(product);
            }
            return RedirectToAction("Product", "Admin");
        }
        [HttpPost]
        public ActionResult CategoryDelete(int id)
        {
            Category category = cm.GetAllBL().FirstOrDefault(x => x.CategoryID == id);
            if (category != null)
            {
                category.CategoryStatus = false;
                cm.UpdateeBL(category);
            }
            return RedirectToAction("Category", "Admin");
        }
        #endregion
        #region Update Post
        [HttpPost]
        public ActionResult UpdateCategory(int id)
        {
            Category category = cm.GetAllBL().FirstOrDefault(x => x.CategoryID == id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Kategori bulunamadı.";
                return RedirectToAction("Category", "Admin");
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult UpdateCategoryPost(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoryToUpdate = cm.GetAllBL().FirstOrDefault(x => x.CategoryID == category.CategoryID);
                    if (categoryToUpdate != null)
                    {
                        categoryToUpdate.CategoryName = category.CategoryName;
                        categoryToUpdate.UserID = Convert.ToInt32(HttpContext.Session["KullaniciID"]);
                        cm.UpdateeBL(categoryToUpdate);
                        TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                        return RedirectToAction("Category", "Admin");

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kategori bulunamadı.";
                        return RedirectToAction("Category", "Admin");
                    }
                }
                catch (Exception ex)
                {

                    TempData["ErrorMessage"] = "Güncelleme yapılırken hata oluştu." + ex.Message;
                    return RedirectToAction("Category", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Güncelleme işlemi başarısız. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Category", "Admin");
            }
        }

        [HttpPost]
        public ActionResult UpdateSubtitle(int id)
        {
            CategorySubtitle category = csm.GetAllBL().FirstOrDefault(x => x.SubtitleID == id);
            return View(category);
        }
        [HttpPost]
        public ActionResult UpdateSubtitlePost(CategorySubtitle subtitle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var subtitleUpdate = csm.GetAllBL().FirstOrDefault(x => x.SubtitleID == subtitle.SubtitleID);
                    if (subtitleUpdate != null)
                    {
                        subtitleUpdate.SubtitleName = subtitle.SubtitleName;
                        subtitleUpdate.CategoryID = subtitle.CategoryID;
                        subtitleUpdate.UserID = Convert.ToInt32(HttpContext.Session["KullaniciID"]);
                        csm.UpdateeBL(subtitleUpdate);
                        TempData["SuccessMessage"] = "AltKategori başarıyla güncellendi.";
                        return RedirectToAction("Subtitle", "Admin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "AltKategori bulunamadı.";
                        return RedirectToAction("Subtitle", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Güncelleme yapılırken hata oluştu." + ex.Message;
                    return RedirectToAction("Subtitle", "Admin");
                }

            }
            else
            {
                TempData["ErrorMessage"] = "Güncelleme işlemi başarısız. Lütfen tekrar deneyiniz.";
                return RedirectToAction("Subtitle", "Admin");
            }
        }

        [HttpPost]
        public ActionResult UpdateProduct(int id)
        {
            Product product = pm.GetAllBL().FirstOrDefault(x => x.ProductID == id);
            return View(product);

        }
        [HttpPost]
        public ActionResult UpdateProductPost(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateProduct = pm.GetAllBL().FirstOrDefault(x => x.ProductID == product.ProductID);
                    if (updateProduct != null)
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files["ProductImage"];
                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/assets/update/"), fileName);
                                file.SaveAs(path);
                                updateProduct.ProductImage = "/assets/update/" + fileName;
                            }
                        }

                        updateProduct.ProductName = product.ProductName;
                        updateProduct.ProductDescription = product.ProductDescription;
                        updateProduct.ProductAdditionDate = DateTime.Now.AddTicks(-(DateTime.Now.Ticks % TimeSpan.TicksPerSecond));
                        updateProduct.SubtitleID = product.SubtitleID;
                        updateProduct.UserID = Convert.ToInt32(HttpContext.Session["KullaniciID"]);

                        pm.UpdateeBL(updateProduct);

                        TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                        return RedirectToAction("Product", "Admin");
                    }

                    TempData["ErrorMessage"] = "Ürün bulunamadı. Lütfen tekrar deneyin.";
                    return View(product);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Product", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Güncelleme işlemi başarısız. Lütfen formu kontrol edin.";
                return RedirectToAction("Product", "Admin");
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(int id)
        {
            User user = um.GetAllBL().FirstOrDefault(x => x.UserID == id);
            return View(user);
        }

        [HttpPost]
        public ActionResult UpdateUserPost(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userUpdate = um.GetAllBL().FirstOrDefault(x => x.UserID == user.UserID);
                    if (userUpdate != null)
                    {
                        userUpdate.UserName = user.UserName;
                        userUpdate.UserSurname = user.UserSurname;
                        userUpdate.UserEmail = user.UserEmail;

                        if (!string.IsNullOrEmpty(user.UserPassword))
                        {
                            if (user.UserPassword.Length < 8 || user.UserPassword.Length > 16)
                            {
                                TempData["ErrorMessage"] = "Şifre 8-16 karakter arasında olmalıdır.";
                                return View(user);
                            }
                            userUpdate.UserPassword = HashPasswordBcrypt(user.UserPassword);
                        }

                        um.UpdateeBL(userUpdate);
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                        return RedirectToAction("Users", "Admin");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                        return RedirectToAction("Users", "Admin");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Güncelleme yapılırken bir hata oluştu: " + ex.Message;
                    return RedirectToAction("Users", "Admin");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Güncelleme işlemi başarısız. Lütfen formu kontrol edin ve tekrar deneyin.";
                return RedirectToAction("Users", "Admin");
            }
        }
        #endregion
        #region ReadOffer
        [HttpPost]
        public ActionResult Read(Contact contactPage)
        {
            var contact = om.GetAllBL().FirstOrDefault(x => x.ContactID == contactPage.ContactID);

            if (contact == null)
            {
                return HttpNotFound("İletişim kaydı bulunamadı.");
            }
            return PartialView("Read", contact);
        }

        [HttpPost]
        public ActionResult Approval(int ContactID)
        {
            var contactRead = om.GetAllBL().FirstOrDefault(x => x.ContactID == ContactID);

            if (contactRead != null)  
            {
                contactRead.Status = true;  
                om.UpdateeBL(contactRead);  
            }
            return RedirectToAction("Offer", "Admin"); 
        }
        #endregion
    }
}

