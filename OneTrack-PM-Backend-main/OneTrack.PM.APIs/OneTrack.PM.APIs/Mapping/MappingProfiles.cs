using AutoMapper;
using OneTrack.PM.Entities.DTO.Security;
using OneTrack.PM.Entities.DTOs.Security;
using OneTrack.PM.Entities.Models;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Mapping.Resolvers;

namespace OneTrack.PM.APIs.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Security

            CreateMap<UsersLoginCreateDTO, SecUsersLogin>();

            CreateMap<UsersRefreshTokensCreateDTO, SecUsersRefreshToken>();

            CreateMap<ContactbJobTitlesUpdateActivationDTO, SecContactJobTitle>();
            CreateMap<SecContactJobTitle, ContactJobTitlesDTO>()
                 .ForMember(d => d.Entity, m => m.MapFrom(s => s.Entity.FullName))
                 .ForMember(d => d.JobTitle, m => m.MapFrom(s => s.JobTitle.Name));
            CreateMap<SecContactJobTitle, ContactJobTitlesByIdDTO>();
            CreateMap<ContactJobTitlesUpdateDTO, SecContactJobTitle>();
            CreateMap<ContactJobTitlesCreateDTO, SecContactJobTitle>();
            CreateMap<SecContactJobTitle, ContactJobTitlesLookupDTO>()
                 .ForMember(d => d.Entity, m => m.MapFrom(s => s.Entity.FullName))
                 .ForMember(d => d.JobTitle, m => m.MapFrom(s => s.JobTitle.Name));

            CreateMap<ContactsUpdateEmailDTO, SecContact>();
            CreateMap<SecContact, ContactsDTO>()
                .ForMember(d => d.Roles, m => m.MapFrom(s => string.Join(" - ", s.SecContactRolesLinks.Select(x => x.Role.Name))))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
                .ForMember(d => d.RolesIds, m => m.MapFrom(s => s.SecContactRolesLinks))
                .ForMember(d => d.FullName, m => m.MapFrom(s => s.TitleId!=null? s.Title.Name+" "+s.FullName:s.FullName))
                .ForMember(d => d.Type, m => m.MapFrom(s => s.ContactType.Name));
            CreateMap<SecContact, ContactsByIdDTO>()
                .ForMember(d => d.Roles, m => m.MapFrom(s => s.SecContactRolesLinks));
            CreateMap<ContactsUpdateDTO, SecContact>();
            CreateMap<ContactsCreateDTO, SecContact>();
            CreateMap<SecContact, ContactsLookupDTO>()
                .ForMember(d => d.FullName, m => m.MapFrom(s => s.TitleId != null ? s.Title.Name + " " + s.FullName : s.FullName))
                .ForMember(d => d.Phones, m => m.MapFrom(s => string.Join('&', s.SecContactPhones.Select(x => x.Number))));
            CreateMap<ContactsUpdateStatusDTO, SecContact>();

            CreateMap<SecTitle, TitlesDTO>();
            CreateMap<SecTitle, TitlesLookupDTO>();
            CreateMap<SecTitle, TitlesByIdDTO>();
            CreateMap<TitleUpdateDTO, SecTitle>();
            CreateMap<TitlesCreateDTO, SecTitle>();

            CreateMap<SecJobTitle, JobTitlesLookupDTO>();
            CreateMap<SecJobTitle, JobTitlesDTO>()
                .ForMember(d => d.Barcode, m => m.MapFrom(s => Constants.JobTitlesBarcodePrefix+s.Code.ToString().PadLeft((int)CodesLengthEnum.JobTitles,'0')))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name));
            CreateMap<SecJobTitle, JobTitlesByIdDTO>()
                .ForMember(d => d.Barcode, m => m.MapFrom(s => Constants.JobTitlesBarcodePrefix + s.Code.ToString().PadLeft((int)CodesLengthEnum.JobTitles, '0')));
            CreateMap<JobTitleUpdateDTO, SecJobTitle>();
            CreateMap<JobTitlesCreateDTO, SecJobTitle>();
            CreateMap<JobTitleUpdateStatusDTO, SecJobTitle>();

            CreateMap<SecContactRole, ContactRolesLookupDTO>();

            CreateMap<ContactRolesLinkCreateDTO, SecContactRolesLink>();
            CreateMap<SecContactRolesLink, ContactRolesLinkDTO>()
                .ForMember(d => d.Role, m => m.MapFrom(s => s.Role.Name));

            CreateMap<ContactAddressesCreateDTO, SecContactAddress>();
            CreateMap<ContactAddressesUpdateDTO, SecContactAddress>();
            CreateMap<SecContactAddress, ContactAddressesDTO>()
                .ForMember(d => d.Governorate, m => m.MapFrom(s => s.Governorate.Name))
                .ForMember(d => d.Country, m => m.MapFrom(s => s.Country.Name))
                .ForMember(d => d.City, m => m.MapFrom(s => s.City.Name));
            CreateMap<SecContactAddress, ContactAddressesEditDTO>()
                .ForMember(d => d.Deleted, m => m.MapFrom(s => 0));

            CreateMap<ContactPhonesCreateDTO, SecContactPhone>();
            CreateMap<ContactPhonesUpdateDTO, SecContactPhone>();
            CreateMap<SecContactPhone, ContactPhonesAppDTO>();
            CreateMap<SecContactPhone, ContactPhonesEditDTO>()
                .ForMember(d => d.Deleted, m => m.MapFrom(s => 0));

            CreateMap<ContactImagesCreateDTO, SecContactImage>();
            CreateMap<ContactImagesUpdateDTO, SecContactImage>();
            CreateMap<SecContactImage, ContactImagesDTO>()
                .ForMember(d => d.Filename, m => m.MapFrom(s => s.Filename.Replace(@"\", @"/")));
            CreateMap<SecContactImage, ContactImagesEditDTO>()
                .ForMember(d => d.Deleted, m => m.MapFrom(s => 0))
                .ForMember(d => d.Filename, m => m.MapFrom(s => s.Filename));

            CreateMap<SecContactGender, ContactGenderDTO>();

            CreateMap<UsersCreateDTO, SecUser>();
            CreateMap<UsersChangePasswordDTO, SecUser>();
            CreateMap<UsersResetPasswordDTO, SecUser>();
            CreateMap<SecUser, UsersDTO>()
                .ForMember(d => d.Group, m => m.MapFrom(s => s.Group.Name))
                .ForMember(d => d.FullName, m => m.MapFrom(s => s.Contact.TitleId != null ? s.Contact.Title.Name + " " + s.Contact.FullName:s.Contact.FullName))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
                .ForMember(d => d.Email, m => m.MapFrom(s => s.Contact.Email));
            CreateMap<UsersUpdateDTO, SecUser>();
            CreateMap<UsersCreateDTO, SecUser>();
            CreateMap<UsersUpdateStatusDTO, SecUser>();
            CreateMap<SecUser, UsersByIdDTO>()
                .ForMember(d => d.Group, m => m.MapFrom(s => s.Group.Name))
                .ForMember(d => d.FullName, m => m.MapFrom(s => s.Contact.TitleId != null ? s.Contact.Title.Name + " " + s.Contact.FullName:s.Contact.FullName))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
                .ForMember(d => d.Email, m => m.MapFrom(s => s.Contact.Email));

            CreateMap<SecActionType, ActionTypesDTO>();

            CreateMap<SecModuleForm, ModuleFormsDTO>()
                .ForMember(d => d.Name, m => m.MapFrom<ParentModuleFormResolver>())
                .ForMember(d => d.ModuleName, m => m.MapFrom(s => s.Module.Name))
                .ForMember(d => d.FormActionTypes, m => m.MapFrom(s => s.SecFormActionTypes));

            CreateMap<SecFormActionType, FormActionTypesDTO>();

            CreateMap<GroupActionsCreateDTO, SecGroupAction>();
            CreateMap<SecGroupAction, GroupActionsDTO>()
                .ForMember(d => d.FormId, m => m.MapFrom(s => s.FormActionType.FormId))
                .ForMember(d => d.ActionTypeId, m => m.MapFrom(s => s.FormActionType.ActionTypeId));

            CreateMap<SecGroup, GroupsDTO>()
                .ForMember(d => d.MainModule, m => m.MapFrom(s => s.MainModule.Name))
                .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name));
            CreateMap<GroupsUpdateDTO, SecGroup>();
            CreateMap<GroupsCreateDTO, SecGroup>();
            CreateMap<GroupsUpdateStatusDTO, SecGroup>();
            CreateMap<SecGroup, GroupsByIdDTO>()
                .ForMember(d => d.Actions, m => m.MapFrom(s => s.SecGroupActions));
            CreateMap<SecGroup, GroupsLookupDTO>();
            #endregion
        }
    }
}
