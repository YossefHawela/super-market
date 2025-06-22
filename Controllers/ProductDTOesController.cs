using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Data;
using SuperMarket.DTO;
using SuperMarket.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperMarket.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ProductDTOesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductDTOesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductDTOes

        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductDTO.ToListAsync());
        }

        // GET: ProductDTOes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDTO = await _context.ProductDTO
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productDTO == null)
            {
                return NotFound();
            }

            return View(productDTO);
        }

        // GET: ProductDTOes/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductDTOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter<LogActionFilter>]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                productDTO.Id = Guid.NewGuid();
                _context.Add(productDTO);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productDTO);
        }

        // GET: ProductDTOes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDTO = await _context.ProductDTO.FindAsync(id);
            if (productDTO == null)
            {
                return NotFound();
            }
            return View(productDTO);
        }

        // POST: ProductDTOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter<LogActionFilter>]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Price")] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDTO);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDTOExists(productDTO.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productDTO);
        }

        // GET: ProductDTOes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDTO = await _context.ProductDTO
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productDTO == null)
            {
                return NotFound();
            }

            return View(productDTO);
        }

        // POST: ProductDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ServiceFilter<LogActionFilter>]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var productDTO = await _context.ProductDTO.FindAsync(id);
            if (productDTO != null)
            {
                _context.ProductDTO.Remove(productDTO);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductDTOExists(Guid id)
        {
            return _context.ProductDTO.Any(e => e.Id == id);
        }
    }
}
