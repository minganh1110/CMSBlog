import { MediaFile } from '../models/media-asset.model';
export interface MediaFolder {
  id: string;
  folderName: string;
  children?: MediaFolder[];
  slugName: string;
  parentFolderId?: string | null;
  path: string;
  dateCreated: Date
  file?: MediaFile[];
}
