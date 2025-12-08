import { MediaFolderService } from '../services/media-folder.service';
import { MediaFolder } from '../models/media-folder.model';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-media-folder-tree',
  templateUrl: './media-folder-tree.component.html',
  styleUrls: ['./media-folder-tree.component.scss']
})


export class MediaFolderTreeComponent implements OnInit {

  @Output() folderSelected = new EventEmitter<MediaFolder>();

  folders: MediaFolder[] = [];
  selectedFolderId: string | null = null;

  constructor(private folderService: MediaFolderService) {}

  ngOnInit() {
  this.folderService.getTree().subscribe(res => {
    this.folders = res;

    // auto select root
    const root = this.folders[0];
    this.selectFolder(root);
  });
}


  loadTree() {
    this.folderService.getTree().subscribe(res => {
      this.folders = res;
    });
  }

  selectFolder(folder: MediaFolder) {
    this.selectedFolderId = folder.id;
  }

  createFolder() {
    const name = prompt("Folder name:");
    if (!name) return;

    this.folderService.create(name, this.selectedFolderId).subscribe(() => {
      this.loadTree();
    });
  }
}
