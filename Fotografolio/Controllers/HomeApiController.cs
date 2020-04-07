using Fotografolio.Data;
using Fotografolio.Data.Interfaces;
using Fotografolio.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Controllers
{
    public class HomeApiController : BaseApiController
    {
        public HomeApiController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; private set; }

        [HttpGet("homedata")]
        public async Task<IActionResult> GetHomeDataAsync()
        {
            UnitOfWork.CreateDefaults();
            var galleries = await UnitOfWork.GalleryRepository.GetAllAsync();
            var logo = await UnitOfWork.LogoRepository.GetAllAsync();
            var link = await UnitOfWork.LinkRepository.GetAllAsync();

            return Ok(new { galleries, logo = logo.FirstOrDefault(), link });
        }
        [HttpGet("photos")]
        public async Task<IActionResult> GetPhotosDataAsync()
        {
            var photos = await UnitOfWork.SlideRepository.GetAllAsync();
            return Ok(new { photos = photos });
        }

        [HttpGet("about")]
        public async Task<IActionResult> GetAboutData()
        {
            var about = await UnitOfWork.AboutRepository.GetAllAsync();
            return Ok(new { about = about.FirstOrDefault() });
        }

        [HttpGet("gallery/{gallery?}")]
        public async Task<IActionResult> GetGalleryData([FromRoute] string? gallery)
        {
            var images = await UnitOfWork.PhotoRepository.GetAllAsync();
            var photos = new List<Photo>();
            foreach (var item in images)
            {
                if (item.Gallery.name == gallery) {
                    photos.Add(item);
                }
            }
            return Ok(new { photos });
        }
    }
}
