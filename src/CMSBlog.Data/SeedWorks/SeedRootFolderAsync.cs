using CMSBlog.Core.Domain.Constants;
//using CMSBlog.Core.Domain.Entities;
using CMSBlog.Core.Domain.Media;
using Microsoft.Extensions.DependencyInjection;
using CMSBlog.Data;
using System;

public static class SeedWorks
{
    public static async Task SeedRootFolderAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CMSBlogContext>();

        if (!db.MediaFolders.Any())
        {
            var root = new MediaFolder
            {
                Id = MediaFolderConstants.RootFolderId,
                FolderName = "Root",
                SlugName = "root",
                ParentFolderId = null,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            db.MediaFolders.Add(root);
            await db.SaveChangesAsync();

            // PathId lúc này mới sinh ra
            root.Path = "/" + root.PathId;
            db.Attach(root);
            db.Entry(root).Property(x => x.Path).IsModified = true;

            await db.SaveChangesAsync();
        }
    }
}
