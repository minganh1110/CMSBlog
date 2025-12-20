import { Formats } from './formats.model';

export enum MediaType {
  Image = 1,
  Video = 2,
  Audio = 3,
  Document = 4,
  Other = 5
}

export interface MediaFile {
  id: string;
  fileName: string;
  filePath: string;
  description?: string | null;
  fileSize: number;
  dateCreated: string;
  dateModified?: string;
  folderId?: string | null;
  folderName?: string | null;
  fileUrl?: string;
  mediaType: MediaType;
  tags?: string[];
  formats?: Formats | null;
  altText?: string | null;
  caption?: string | null;
  createdBy?: string | null;
  updatedBy?: string | null;

  isSelected?: boolean;
}
