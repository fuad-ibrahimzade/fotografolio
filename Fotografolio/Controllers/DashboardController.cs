using Fotografolio.Data;
using Fotografolio.Data.Interfaces;
using Fotografolio.Data.Models;
using Fotografolio.Data.ViewModels;
using Fotografolio.Helpers;
using Fotografolio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Controllers
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.JWT)]
    public class DashboardController : BaseApiController
    {
        public DashboardController(IUnitOfWork unitOfWork,
            ICloudniaryService cloudniaryService) : base(unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.CloudniaryService = cloudniaryService;
        }

        public IUnitOfWork UnitOfWork { get; private set; }
        public ICloudniaryService CloudniaryService { get; private set; }

        [HttpGet("dashboard")]
        public async Task<IActionResult> DashboardAsync()
        {
            var galleries = await UnitOfWork.GalleryRepository.GetAllAsync();
            var photos = await UnitOfWork.PhotoRepository.GetAllAsync();

            return Ok(new { galleries, photos });
        }

        [HttpDelete("gallery/{galleryid}/delete")]
        public async Task<IActionResult> DestroyGalleryAsync([FromRoute] int galleryid)
        {
            var gallery = await UnitOfWork.GalleryRepository.GetByIDAsync(galleryid);
            var photos = new List<Photo>();
            foreach (var photo in gallery.Photos)
            {
                photos.Add(photo);
            }
            UnitOfWork.GalleryRepository.Delete(gallery.id);
            await UnitOfWork.SaveAsync();
            foreach (var photo in photos)
            {
                if (photo.id > FotografolioDbContext.SeededMaxIds[typeof(Photo).Name])
                    CloudniaryService.DeleteImage(photo.name);
            }
            return Ok(new { destroyGallery = "Gallery has been deleted successfully!" });
        }

        [HttpDelete("photo/{photoid}/delete")]
        public async Task<IActionResult> DestroyPhotoAsync([FromRoute] int photoid)
        {
            var photo = await UnitOfWork.PhotoRepository.GetByIDAsync(photoid);
            UnitOfWork.PhotoRepository.Delete(photo);
            await UnitOfWork.SaveAsync();
            if (photo.id > FotografolioDbContext.SeededMaxIds[typeof(Photo).Name])
                CloudniaryService.DeleteImage(photo.name);
            return Ok(new { destroyPhoto = "Photo has been deleted successfully!" });
        }

        [HttpPost("gallery/new")]
        public async Task<IActionResult> NewGalleryAsync([FromForm] string name)
        {
            var gallery = new Gallery();
            gallery.name = name;
            UnitOfWork.GalleryRepository.Insert(gallery);
            await UnitOfWork.SaveAsync();
            return Ok(new { createGallery = "Gallery has been created successfully!" });
        }

        [HttpPost("photo/new")]
        public async Task<IActionResult> AddPhotoAsync([FromForm] PhotosViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { fileError = "Yalnız jpeg, png, bmp, gif və ya svg sonluqlu fayllar yükləyə bilərsiniz!" });

            foreach (var rphoto in model.photos)
            {
                using (var stream = rphoto.OpenReadStream())
                {
                    var publicUrl = CloudniaryService.UploadImage(rphoto.FileName, stream);

                    var photo = new Photo();
                    photo.Gallery = await UnitOfWork.GalleryRepository.GetByIDAsync(model.gallery_id);
                    photo.name = publicUrl;
                    UnitOfWork.PhotoRepository.Insert(photo);
                    await UnitOfWork.SaveAsync();
                }
            }

            return Ok(new { addPhoto = "Photos has been added successfully!" });
        }

        [HttpGet("slide")]
        public async Task<IActionResult> SlideAsync()
        {
            var slide = await UnitOfWork.SlideRepository.GetAllAsync();
            return Ok(new { slide });
        }

        [HttpPost("slide/new")]
        public async Task<IActionResult> AddSlideAsync([FromForm] SlideViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { fileError = ModelState.Errors() });
            //return Ok(new { fileError = "Yalnız jpeg, png, bmp, gif və ya svg sonluqlu fayllar yükləyə bilərsiniz!" });

            foreach (var rphoto in model.photos)
            {
                using (var stream = rphoto.OpenReadStream())
                {
                    var publicUrl = CloudniaryService.UploadImage(rphoto.FileName, stream);
                    var slide = new Slide();
                    slide.name = publicUrl;
                    UnitOfWork.SlideRepository.Insert(slide);
                    await UnitOfWork.SaveAsync();
                }
            }

            return Ok(new { addPhoto = "Photos has been added successfully!" });

        }

        [HttpDelete("slide/{slideid}/delete")]
        public async Task<IActionResult> DestroySlideAsync([FromRoute] int slideid)
        {
            Slide slide = await UnitOfWork.SlideRepository.GetByIDAsync(slideid);
            UnitOfWork.SlideRepository.Delete(slide);
            await UnitOfWork.SaveAsync();
            if (slide.id > FotografolioDbContext.SeededMaxIds[typeof(Slide).Name])
                CloudniaryService.DeleteImage(slide.name);
            return Ok(new { destroyPhoto = "Photo has been deleted successfully!" });
        }

        [HttpGet("about/edit")]
        public async Task<IActionResult> AboutAsync()
        {
            var about = await UnitOfWork.AboutRepository.GetAllAsync();
            return Ok(new { about = about.FirstOrDefault() });
        }

        [HttpPost("about/update")]
        public async Task<IActionResult> UpdateAboutAsync([FromForm] AboutViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { fileError = "Yalnız jpeg, png, bmp, gif və ya svg sonluqlu fayllar yükləyə bilərsiniz!" });

            var about = (await UnitOfWork.AboutRepository.GetAllAsync()).FirstOrDefault();

            if (model.photo != null && model.photo.Length > 0)
            {
                if (about.id > FotografolioDbContext.SeededMaxIds[typeof(About).Name])
                    CloudniaryService.DeleteImage(about.photo);
                using (var stream = model.photo.OpenReadStream())
                {
                    var photo = CloudniaryService.UploadImage(model.photo.FileName, stream);
                    about.photo = photo;
                }
            }
            about.title = model.title;
            about.desc = model.desc;
            UnitOfWork.AboutRepository.Update(about);
            await UnitOfWork.SaveAsync();

            return Ok(new { redirectTo = "/about" });
        }

        [HttpGet("logo/get")]
        public IActionResult logo()
        {
            return Ok();
        }

        [HttpPost("logo/update")]
        public async Task<IActionResult> UpdateLogoAsync([FromForm] LogoViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { fileError = "Yalnız jpeg, png, bmp, gif və ya svg sonluqlu fayllar yükləyə bilərsiniz!" });

            var logo = (await UnitOfWork.LogoRepository.GetAllAsync()).FirstOrDefault();

            if (model.photo != null && model.photo.Length > 0)
            {
                if (logo.id > FotografolioDbContext.SeededMaxIds[typeof(Logo).Name])
                    CloudniaryService.DeleteImage(logo.photo);
                using (var stream = model.photo.OpenReadStream())
                {
                    var photo = CloudniaryService.UploadImage(model.photo.FileName, stream);
                    logo.photo = photo;
                }
            }
            UnitOfWork.LogoRepository.Update(logo);
            await UnitOfWork.SaveAsync();

            return Ok(new { redirectTo = "/" });
        }

        [HttpGet("link")]
        public async Task<IActionResult> LinkAsync()
        {
            var link = await UnitOfWork.LinkRepository.GetAllAsync();
            return Ok(new { link });
        }

        [HttpPost("link/update")]
        public async Task<IActionResult> UpdateLinkAsync([FromForm] LinkViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { fileError = ModelState.Errors() });

            var link = (await UnitOfWork.LinkRepository.GetAllAsync()).ToList();

            link[0].name = model.facebook;
            link[1].name = model.instagram;
            link[2].name = model.twitter;
            link[3].name = model.youtube;
            foreach (var item in link)
            {
                UnitOfWork.LinkRepository.Update(item);
            }
            await UnitOfWork.SaveAsync();

            return Ok(new { redirectTo = "/" });
        }
    }
}
