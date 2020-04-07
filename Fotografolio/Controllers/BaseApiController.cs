using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fotografolio.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fotografolio.Controllers
{
    [Route("api")]
    public class BaseApiController : Controller
    {
        public BaseApiController(IUnitOfWork unitOfWork)
        {
            
        }
    }
}