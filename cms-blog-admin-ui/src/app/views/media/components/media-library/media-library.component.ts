import { Component, OnInit } from '@angular/core';
import { MediaService } from '../../services/media.service';
import { MediaFile } from '../../models/media-file.model';
import { MediaFolder } from '../../models/media-folder.model';

@Component({
  selector: 'app-media-library',
  templateUrl: './media-library.component.html',
  styleUrls: ['./media-library.component.scss']
})
export class MediaLibraryComponent implements OnInit {
  folders: MediaFolder[] = [];
  assets: MediaFile[] = [];
  selectedFolder?: MediaFolder;
  isGrid = true;

  constructor(private mediaService: MediaService) {}

  ngOnInit(): void {
    this.loadFolders();
    this.loadAssets();
  }

  loadFolders() {
    this.mediaService.getTree().subscribe(f => this.folders = f);
  }

  loadAssets(folderId?: string) {
    const obs = folderId ? this.mediaService.getInFolder(folderId) : this.mediaService.getAll();
    obs.subscribe(list => {
      this.assets = list || [];
    });
  }

  onFolderSelected(folder: MediaFolder) {
    this.selectedFolder = folder;
    this.loadAssets(folder.id);
  }

  getThumbUrl(file: MediaFile): string | null {
    return file?.formats?.thumbnail?.url ?? file?.formats?.small?.url ?? null;
  }
}
