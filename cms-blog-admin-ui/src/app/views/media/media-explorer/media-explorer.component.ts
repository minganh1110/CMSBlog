import { Component, OnInit } from '@angular/core';
import { MediaFolderService } from '../services/media-folder.service';
import { MediaFolder } from '../models/media-folder.model';

@Component({
  selector: 'app-media-explorer',
  templateUrl: './media-explorer.component.html',
  styleUrls: ['./media-explorer.component.scss']
})
export class MediaExplorerComponent implements OnInit {

  assets: any[] = [];
  selectedFolderId: string = "11111111-1111-1111-1111-111111111111"; // root
  currentPath = "";
  viewMode: 'thumbnail' | 'list' = 'thumbnail';
  filterText = "";

  folders: MediaFolder[] = [];
  flatFolders: { id: string; fullPath: string }[] = [];

  constructor(private folderService: MediaFolderService) {}

  ngOnInit() {
    this.loadFolders();
  }

  loadFolders() {
    this.folderService.getTree().subscribe(tree => {
      this.folders = tree;

      this.flatFolders = [];
      this.flattenTree(tree[0], "");

      this.updatePath();
    });
  }

  flattenTree(node: MediaFolder, parent: string) {
    const p = parent ? `${parent}/${node.slugName}` : node.slugName;

    this.flatFolders.push({ id: node.id, fullPath: p });

    node.children?.forEach(child => this.flattenTree(child, p));
  }

  onFolderSelected(folder: MediaFolder) {
    this.selectedFolderId = folder.id;
    this.updatePath();
  }

  onFolderChange() {
    this.updatePath();
  }

  updatePath() {
    const found = this.flatFolders.find(f => f.id === this.selectedFolderId);
    this.currentPath = found ? found.fullPath : "";
  }

  toggleView() {
    this.viewMode = this.viewMode === 'thumbnail' ? 'list' : 'thumbnail';
  }

  createFolder() {
    const name = prompt("Folder name:");
    if (!name) return;
    const parentFolderId = prompt("Parent Folder:")

    this.folderService.create(name, this.selectedFolderId).subscribe(() => {
      this.loadFolders();
    });
  }
}
