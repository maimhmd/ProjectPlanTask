using AutoMapper;
using OneTrack.PM.Core;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Mapping.Resolvers
{
    public class ParentModuleFormResolver : IValueResolver<SecModuleForm, ModuleFormsDTO, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ParentModuleFormResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Resolve(SecModuleForm source, ModuleFormsDTO destination, string destMember, ResolutionContext context)
        {
           return (source.ParentId!=null? _unitOfWork.Repository<SecModuleForm>().GetById(source.ParentId??0).Name + " / ":string.Empty)+source.Name;
        }
    }
}
