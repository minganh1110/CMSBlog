using CMSBlog.Core.Application.Interfaces.Media;
using CMSBlog.Core.Domain.Media;
using CMSBlog.Data;
using Microsoft.EntityFrameworkCore;

public class FileFolderLinkRepository : IFileFolderLinkRepository
{
    private readonly CMSBlogContext _db;

    public FileFolderLinkRepository(CMSBlogContext context)
    {
        _db = context;
    }

    public async Task AddLinkAsync(Guid fileId, Guid folderId)
    {
        var link = new MediaFileFolderLink
        {
            MediaFileId = fileId,
            MediaFolderId = folderId
        };

        _db.MediaFileFolderLinks.Add(link);
        await _db.SaveChangesAsync();
    }

    public async Task RemoveLinkAsync(Guid fileId, Guid folderId)
    {
        var link = await _db.MediaFileFolderLinks
            .FirstOrDefaultAsync(x => x.MediaFileId == fileId && x.MediaFolderId == folderId);

        if (link != null)
        {
            _db.MediaFileFolderLinks.Remove(link);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Guid>> GetFileIdsInFolderAsync(Guid folderId)
    {
        return await _db.MediaFileFolderLinks
            .Where(x => x.MediaFolderId == folderId)
            .Select(x => x.MediaFileId)
            .ToListAsync();
    }

    public async Task<List<Guid>> GetFolderIdsOfFileAsync(Guid fileId)
    {
        return await _db.MediaFileFolderLinks
            .Where(x => x.MediaFileId == fileId)
            .Select(x => x.MediaFolderId)
            .ToListAsync();
    }

    public async Task DeleteLinksByFolderAsync(Guid folderId)
    {
        var links = await _db.MediaFileFolderLinks
            .Where(x => x.MediaFolderId == folderId)
            .ToListAsync();

        _db.MediaFileFolderLinks.RemoveRange(links);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteLinksByFileAsync(Guid fileId)
    {
        var links = await _db.MediaFileFolderLinks
            .Where(x => x.MediaFileId == fileId)
            .ToListAsync();

        _db.MediaFileFolderLinks.RemoveRange(links);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteByFolderIdsAsync(List<Guid> folderIds)
    {
        var links = await _db.MediaFileFolderLinks
            .Where(x => folderIds.Contains(x.MediaFolderId))
            .ToListAsync();

        if (links.Count > 0)
        {
            _db.MediaFileFolderLinks.RemoveRange(links);
        }
    }

}
