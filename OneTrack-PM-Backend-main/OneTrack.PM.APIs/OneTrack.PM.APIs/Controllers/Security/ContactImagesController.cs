using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.Helpers;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTO.Security;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Entities.RequestSpecifications.ModelParameters.Security;
using OneTrack.PM.Repositories.Specifications.Security;

namespace OneTrack.PM.APIs.Controllers.Security
{
    [Route("api/security/[controller]")]
    [Authorize]
    public class ContactImagesController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactImagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(9000000000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 9000000000)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateContactImages(int id, [FromBody] IEnumerable<ContactImagesFormUpdateDTO> imagesCollection)
        {
            foreach (var i in imagesCollection)
            {
                if (i.Id != null && i.Deleted == 1)
                {
                    var image = await _unitOfWork.Repository<SecContactImage>().GetWithSpecAsync(new ContactImagesSpecifications(i.Id ?? 0), trackChanges: true);
                    if (image != null)
                    {
                        _unitOfWork.Repository<SecContactImage>().Delete(image);
                        if (System.IO.File.Exists(image.Filename))
                            System.IO.File.Delete(image.Filename);
                    }
                }
                else if (i.Id == null && i.Base64 != null && i.Base64 != string.Empty)
                {
                    FileInfo fi = new FileInfo(i.Filename);
                    var imageEntity = _mapper.Map<SecContactImage>(new ContactImagesCreateDTO
                    {
                        ContactId = id,
                        Filename = Upload.UploadFiles(fi.Extension, i.Base64, "ContactsUploads/Images"),
                        Caption = i.Caption,
                        UploadedBy = i.UploadedBy,
                        UploadDate = DateTime.Now
                    });
                    await _unitOfWork.Repository<SecContactImage>().Add(imageEntity);
                }
                else
                {
                    string url = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["url"];
                    ContactImagesUpdateDTO image = new ContactImagesUpdateDTO
                    {
                        Id = i.Id ?? 0,
                        ContactId = id,
                        Filename = i.Filename.Replace(url, string.Empty),
                        Caption = i.Caption,
                        UploadDate = i.UploadDate,
                        UploadedBy = i.UploadedBy
                    };
                    var imageEntity = _mapper.Map<SecContactImage>(image);
                    _unitOfWork.Repository<SecContactImage>().Update(imageEntity);
                }
            }
            await _unitOfWork.Complete();
            return Ok();
        }

        [HttpGet("GetByContact")]
        public async Task<ActionResult<IReadOnlyList<ContactImagesEditDTO>>> GetContactImagesByContactId(int contactId)
        {
            var query = await _unitOfWork.Repository<SecContactImage>().GetAllWithSpecAsync(new ContactImagesSpecifications(new ContactImagesParameters
            {
                ContactId = contactId
            }, false), false);
            var data = _mapper.Map<IReadOnlyList<ContactImagesEditDTO>>(query);
            return Ok(data);
        }
    }
}