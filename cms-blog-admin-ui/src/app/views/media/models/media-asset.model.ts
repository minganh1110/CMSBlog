
export interface MediaFile{
    id: string;
    fileName: string;
    filePath: string;
    description?: string | null;
    fileSize: DoubleRange;
    dateCreated: Date;
    dateModified: Date;
    folderId: string;
    folderName: null;
    fileUrl: string;
    tags: []
}
