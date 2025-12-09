using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CMSBlog.Core.Application.DTOs.Media;
using CMSBlog.Core.Domain.Media;

namespace CMSBlog.Core.Application.Mapping
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            // -----------------------------------------------------
            // 1. MediaFile → MediaFileDto
            // -----------------------------------------------------
            CreateMap<MediaFile, MediaFileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ID))

                .ForMember(dest => dest.FileUrl,
                    opt => opt.MapFrom(src => src.FilePath))

                .ForMember(dest => dest.FolderId,
                    opt => opt.MapFrom(src =>
                        src.FileFolderLinks != null
                            ? src.FileFolderLinks.First().MediaFolderId
                            : (Guid?)null))

                .ForMember(dest => dest.FolderName,
                    opt => opt.MapFrom(src =>
                        src.FileFolderLinks != null
                            ? src.FileFolderLinks.First().MediaFolder.FolderName
                            : null))

                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src =>
                        src.MediaFileTags != null
                            ? src.MediaFileTags.Select(t => t.MediaTag.TagName).ToList()
                            : new List<string>()))

                .ForMember(dest => dest.Formats,
                    opt => opt.MapFrom(src => src.Formats));

            // -----------------------------------------------------
            // 2. CreatedMediaFileDto → MediaFile (Upload)
            // -----------------------------------------------------
            CreateMap<CreatedMediaFileDto, MediaFile>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.FileExtension,
                    opt => opt.MapFrom(src => Path.GetExtension(src.FileName)))
                .ForMember(dest => dest.FileSize,
                    opt => opt.MapFrom(src => src.FileContent.Length))
                .ForMember(dest => dest.MimeType,
                    opt => opt.MapFrom(src => src.MimeType))
                .ForMember(dest => dest.FileFolderLinks, opt => opt.Ignore())
                .ForMember(dest => dest.MediaFileTags, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.Provider, opt => opt.Ignore());

            // -----------------------------------------------------
            // 3. MediaFolder → MediaFolderDto
            // -----------------------------------------------------
            CreateMap<MediaFolder, MediaFolderDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ParentFolderId,
                    opt => opt.MapFrom(src => src.ParentFolderId))
                .ForMember(dest => dest.Children,
                    opt => opt.MapFrom(src => src.ChildFolders))
                .ForMember(dest => dest.Files,
                    opt => opt.MapFrom(src =>
                        src.FileFolderLinks != null
                            ? src.FileFolderLinks.Select(l => l.MediaFile)
                            : new List<MediaFile>()));

            // -----------------------------------------------------
            // 4. CreateMediaFolderDto → MediaFolder
            // -----------------------------------------------------
            CreateMap<CreateMediaFolderDto, MediaFolder>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SlugName,
                    opt => opt.MapFrom(src => src.FolderName.ToLower().Replace(" ", "-")))
                .ForMember(dest => dest.Path, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ForMember(dest => dest.DateModified, opt => opt.Ignore());

            // -----------------------------------------------------
            // 5. UpdateMediaFolderDto → MediaFolder
            // -----------------------------------------------------
            CreateMap<UpdateMediaFolderDto, MediaFolder>()
                .ForMember(dest => dest.PathId, opt => opt.Ignore())
                .ForMember(dest => dest.ParentFolderId,
                    opt => opt.MapFrom(src => src.NewFolderId));

            //
            //
            //
            CreateMap<UpdateMediaFileDto, MediaFile>();

              
        }
    }
}
