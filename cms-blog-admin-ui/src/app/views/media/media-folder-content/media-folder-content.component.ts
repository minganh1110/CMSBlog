import { Component, Input, OnChanges } from '@angular/core';
import { MediaFolderService } from '../services/media-folder.service';
import { MediaFileService } from '../services/media-file.service';

@Component({
  selector: 'app-media-folder-content',
  templateUrl: './media-folder-content.component.html',
  styleUrls: ['./media-folder-content.component.scss']
})
export class MediaFolderContentComponent implements OnChanges {

  @Input() folderId!: string;
  @Input() viewMode!: 'thumbnail' | 'list';
  @Input() filter = "";

  assets: any[] = [];
  filteredAssets: any[] = [];

  constructor(
        //private folderService: MediaFolderService,
        private fileService: MediaFileService
    ) {}


  ngOnChanges() {
    if (!this.folderId) return;

    this.fileService.getAssetOnFolder(this.folderId).subscribe(res => {
      this.assets = res;
      this.applyFilter();
    });
  }

  applyFilter() {
    this.filteredAssets = this.assets.filter(a =>
      a.fileName.toLowerCase().includes(this.filter.toLowerCase())
    );
  }
}
