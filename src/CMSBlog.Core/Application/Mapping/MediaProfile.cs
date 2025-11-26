using AutoMapper;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Domain.Media;

namespace CMSBlog.Core.Application.Mapping
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            // MediaFile → MediaFileDto
            CreateMap<MediaFile, MediaFileDto>()
                .ForMember(dest => dest.FileUrl,
                    opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FolderName,
                    opt => opt.MapFrom(src => src.Folder != null ? src.Folder.FolderName : null))
                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src =>
                        src.MediaFileTags != null
                            ? src.MediaFileTags.Select(t => t.MediaTag.TagName).ToList()
                            : new List<string>()
                    ));

            // CreateMediaFileDto → MediaFile
            CreateMap<CreatedMediaFileDto, MediaFile>()
                .ForMember(dest => dest.FileExtension,
                    opt => opt.MapFrom(src => Path.GetExtension(src.FileName)))
                .ForMember(dest => dest.FileSize,
                    opt => opt.MapFrom(src => src.FileContent.Length))
                .ForMember(dest => dest.Description, opt => opt.Ignore());


            // MediaFolder → MediaFolderDto
            CreateMap<MediaFolder, MediaFolderDto>()
                .ForMember(dest => dest.Children,
                    opt => opt.MapFrom(src => src.ChildFolders))
                .ForMember(dest => dest.Files,
                    opt => opt.MapFrom(src => src.MediaFiles))
                .ForMember(dest => dest.ParentId,
                    opt => opt.MapFrom(src => src.ParentFolderId))
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ID));

            // CreateMediaFolderDto → MediaFolder
            CreateMap<CreateMediaFolderDto, MediaFolder>();
        }
    }
}
