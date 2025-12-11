import { MediaFile } from './media-file.model';

export interface MediaFolder {
  id: string;
  folderName: string;
  slugName?: string;
  parentFolderId?: string | null;
  path?: string;
  dateCreated?: string;
  children?: MediaFolder[];
  files?: MediaFile[]; // files in this folder
}
