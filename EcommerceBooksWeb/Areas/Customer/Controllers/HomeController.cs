﻿using Books.DataAccess.Repository.IRepository;
using Books.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EcommerceBooksWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		public HomeController(ILogger<HomeController> logger,IUnitOfWork unit)
        {
            _logger = logger;
            _unitOfWork = unit;
        }

        public IActionResult Index()
        {
			IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType");

			return View(productList);
		}
        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType"),
            };

            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);


            if (cartFromDb == null)
            {

                _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.IncrementCount(cartFromDb, shoppingCart.Count);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}