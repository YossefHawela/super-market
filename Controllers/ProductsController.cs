using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Data;
using SuperMarket.Mapper;

namespace SuperMarket.Controllers
{

    [Authorize(Roles = "Admin,User")]

    public class ProductsController : Controller
    {

        private readonly DataConnector _connector;

        public ProductsController(DataConnector connector)
        {
            _connector = connector;
        }
        public IActionResult Index()
        {
            return View(_connector.Products.ToProductModels());
        }
    }
}
