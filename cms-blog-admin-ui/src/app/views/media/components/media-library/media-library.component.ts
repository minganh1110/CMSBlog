import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MediaService } from '../../services/media.service';
import { MediaFile } from '../../models/media-file.model';
import { MediaFolder } from '../../models/media-folder.model';
import { Observable, forkJoin } from 'rxjs';


type SelectType = 'folder' | 'asset';

interface SelectedItem {
  id: string;
  type: SelectType;
}

@Component({
  selector: 'app-media-library',
  templateUrl: './media-library.component.html',
  styleUrls: ['./media-library.component.scss']
})
export class MediaLibraryComponent implements OnInit {
  @Output() toggle = new EventEmitter<void>();


  folders: MediaFolder[] = [];
  assets: MediaFile[] = [];
  items: any[] = [];        // Dùng chung cho MODE LIST
  selectedFolder?: MediaFolder;
  isDark = false;
  isGrid = true;
  rootFolderId = "11111111-1111-1111-1111-111111111111";
  path: MediaFolder[] = [];

  constructor(private mediaService: MediaService) {}

  ngOnInit(): void {
    this.mediaService.getFolderById(this.rootFolderId).subscribe(folder => {
      this.selectedFolder = folder;
      this.path = [];

      this.folders = folder.children ?? [];
      this.loadAssets(folder.id);

      // *** QUAN TRỌNG: thêm dòng này để LIST có dữ liệu ***
      this.loadFolderById(folder.id);
    });
  }

  // Load folder + files → items[]
  loadFolderById(folderId: string) {
    const folder$ = this.mediaService.getFolderById(folderId);
    const files$ = this.mediaService.getInFolder(folderId);

    forkJoin([folder$, files$]).subscribe(([folder, files]) => {
      this.folders = folder.children ?? [];

      this.items = [
        // FOLDER
        ...(folder.children || []).map(f => ({
          isFolder: true,
          name: f.folderName,
          extension: null,
          size: null,
          created: f.dateCreated,
          data: f
        })),

        // FILE
        ...files.map(f => ({
          isFolder: false,
          name: f.fileName,
          extension: f.mediaType,
          size: f.fileSize,
          created: f.dateCreated,
          data: f
        }))
      ];
    });
  }


  // Load assets riêng cho chế độ grid
  loadAssets(folderId?: string) {
    const obs = folderId ? this.mediaService.getInFolder(folderId) : this.mediaService.getAll();
    obs.subscribe(list => this.assets = list || []);
  }

  getThumbUrl(file: MediaFile): string | null {
    return file?.formats?.thumbnail?.url ?? file?.formats?.small?.url ?? null;
  }

  onFolderSelected(folder: MediaFolder) {
    this.selectedFolder = folder;
    this.path = [...this.path, folder];

    this.loadFolderById(folder.id);
    this.loadAssets(folder.id);
  }

  goRoot() {
    this.ngOnInit();
  }

  goTo(folder: MediaFolder) {
    this.selectedFolder = folder;

    const index = this.path.findIndex(f => f.id === folder.id);
    if (index !== -1) this.path = this.path.slice(0, index + 1);

    this.loadFolderById(folder.id);
    this.loadAssets(folder.id);
  }

  toggleView() {
    this.isGrid = !this.isGrid;
  }

  onItemClick(item: any) {
    if (item.isFolder) {
      this.onFolderSelected(item.data);
    } else {
      console.log("File clicked:", item.data);
    }
  }

  onItemToggle() {
    this.checkAll = this.assets.every(a => a.isSelected);
  }


  checkAll = false;
  filterText = "";

  get filteredItems() {
    return this.items.filter(i =>
      !this.filterText ||
      i.name.toLowerCase().includes(this.filterText.toLowerCase())
    );
  }

  addAsset() {
    console.log("Add asset clicked");
  }

  toggleTheme() {
    this.isDark = !this.isDark;
  }

  isSearchOpen = false;
  toggleSearch(): void {
    this.isSearchOpen = !this.isSearchOpen;

    if (!this.isSearchOpen) {
      this.filterText = ''; // optional
    }
  }

  closeSearch(): void {
    this.isSearchOpen = false;
    this.filterText = '';
  }

  // Upload asset
  uploadFolderId: string | null = null;
  showAddAsset = false;

  openAddAsset() {
    this.selectedFiles = [];

    // mặc định upload vào folder đang chọn
    this.uploadFolderId = this.selectedFolder?.id ?? this.rootFolderId;

    // mở modal
    this.showAddAsset = true;

    //Mặc định selected files rỗng
    this.selectedFiles = [];
    this.previews = []; 

    // // load tree để chọn folder upload vào
    // this.mediaService.getTree().subscribe(res => {
    //   this.folderTree = res;
    // });
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  selectedFiles: File[] = [];
  previews: string[] = [];

  onFileSelect(event: any) {
    const files = Array.from(event.target.files) as File[];
    this.handleFiles(files);
  }

  onFileDrop(event: DragEvent) {
    event.preventDefault();
    const files = Array.from(event.dataTransfer?.files || []);
    this.handleFiles(files);
  }

  handleFiles(files: File[]) {
      this.selectedFiles = files;
      this.previews = [];

    files.forEach(file => {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.previews.push(e.target.result);
      };
      reader.readAsDataURL(file);
    });
  }



  submitUpload() {
  if (!this.selectedFiles.length) return;

  const formData = new FormData();
  this.selectedFiles.forEach(file => {
    formData.append("files", file);  // API nên nhận List<IFormFile>
    formData.append("FolderId", this.uploadFolderId || ""); // Thêm parentId vào form data
  });

  this.mediaService.upload(formData)
    .subscribe({
      next: () => {
        alert("Upload thành công!");
        this.showAddAsset = false;
        this.selectedFiles = [];
        this.previews = [];
        this.loadFolderById(this.selectedFolder?.id || this.rootFolderId);
        this.loadAssets(this.selectedFolder?.id || this.rootFolderId);
      },
      error: () => alert("Upload thất bại!")
    });
  }

// Add folder or edit folder
  showAddFolder = false;
  newFolderName = '';
  newFolderParentId: string | null = null;
  folderTree: any[] = [];
  folderError: string | null = null;

  // openAddFolder() {
  //   this.newFolderName = '';
  //   this.newFolderParentId = this.selectedFolder?.id ?? null;
  //   this.showAddFolder = true;

  //   this.mediaService.getTree().subscribe(res => {
  //     this.folderTree = res;
  //   });
  // }

  //open Modal add folder
  openAddFolder() {
    this.folderModalMode = 'add';
    this.editingFolder = null;

    this.folderForm = {
      name: '',
      parentId: this.selectedFolder?.id || null
    };

    this.folderModalOpen = true;
    // Load tree
    this.mediaService.getTree().subscribe(res => {
        this.folderTree = res;
      });
  }
submitAddFolder() {
    if (!this.newFolderName.trim()) {
      alert("Tên folder không được để trống!");
      return;
    }

    const payload = {
      folderName: this.newFolderName,
      parentFolderId: this.newFolderParentId   // <-- NOW CORRECT
    };
    console.log("Selected parentId = ", this.newFolderParentId);

    this.mediaService.createFolder(payload).subscribe({
      next: () => {
        this.showAddFolder = false;
        this.loadFolderById(this.selectedFolder?.id || this.rootFolderId);
        alert("Tạo folder thành công!");
      },
      error: () => alert("Có lỗi xảy ra!")
    });
  }
  //open Modal edit folder
  openEditFolder(folder: any) {
    this.folderModalMode = 'edit';
    this.editingFolder = folder;

    this.folderForm = {
      name: folder.folderName,
      parentId: this.selectedFolder?.id || null
    };

    this.folderError = null;
    this.folderModalOpen = true;
    // Load tree
    this.mediaService.getTree().subscribe(res => {
        this.folderTree = res;
      });
  }


  submitFolder() {

    if (this.folderModalMode === 'add') {
      this.addFolder();
    } 
    else if (this.folderModalMode === 'edit') {
      this.updateFolder();
    } 
    else if (this.folderModalMode === 'move') {
      this.submitMove();
    }
  }

  addFolder() {
    const payload = {
      folderName: this.folderForm.name,
      parentFolderId: this.folderForm.parentId
    };

    this.mediaService.createFolder(payload).subscribe({
      next: () => {
        this.folderModalOpen = false;
        this.loadFolderById(this.selectedFolder?.id || this.rootFolderId);
        alert("Tạo folder thành công!");
      },
      error: () => alert("Có lỗi xảy ra!")
    });
  }

  updateFolder() {
    this.folderError = null;

    if (!this.validateEditFolder()) return;

    const payload = {
      folderName: this.folderForm.name.trim(),
      parentFolderId: this.folderForm.parentId
    };

    this.mediaService
      .editFolder(this.editingFolder.id, payload)
      .subscribe({
        next: () => {
          this.loadFolderById(this.selectedFolder?.id || this.rootFolderId);
          this.folderModalOpen = false;
        },
        error: (err) => {
          // ƯU TIÊN message từ backend
          this.folderError =
            err?.error?.message ||
            err?.error ||
            'Failed to update folder';
        }
      });
  }


  isTreeOpen = false;

  toggleTree() {
    this.isTreeOpen = !this.isTreeOpen;
  }

  onParentSelected(folder: any) {
    this.folderForm.parentId = folder.id;     // <-- CHỌN CHÍNH XÁC
    this.isTreeOpen = false;
  }

  getFolderName(id: string | null): string {
    if (!id) return "Select folder";
    if (id === this.selectedFolder?.id) return "(Current folder)";

    const found = this.findFolderById(this.folderTree, id);
    return found?.folderName || "Select folder";
  }

  findFolderById(list: any[], id: string | null): any {
    for (const item of list) {
      if (item.id === id) return item;
      if (item.children) {
        const res = this.findFolderById(item.children, id);
        if (res) return res;
      }
    }
    return null;
  }
  //select and edit folder
  selectedFolderIds = new Set<string>();

  toggleFolderSelect(folder: any) {
    if (this.selectedFolderIds.has(folder.id)) {
      this.selectedFolderIds.delete(folder.id);
    } else {
      this.selectedFolderIds.add(folder.id);
    }
  }

  folderModalOpen = false;
  folderModalMode: 'add' | 'edit' | 'move' = 'add';


  editingFolder: any = null;

  folderForm = {
    name: '',
    parentId: null as string | null
  };

  //validation for folder form
  private validateFolderForm(): boolean {
    if (!this.folderForm.name || !this.folderForm.name.trim()) {
      this.folderError = 'Folder name cannot be empty';
      return false;
    }

    return true;
  }

  //check folder is descendant
  private isDescendant(parentId: string | null, folder: any): boolean {
    if (!parentId || !folder.children) return false;

    for (const child of folder.children) {
      if (child.id === parentId) return true;
      if (this.isDescendant(parentId, child)) return true;
    }

    return false;
  }

  private validateEditFolder(): boolean {
    if (!this.validateFolderForm()) return false;

    if (
      this.folderForm.parentId &&
      this.isDescendant(this.folderForm.parentId, this.editingFolder)
    ) {
      this.folderError = 'Cannot move folder into its child folder';
      return false;
    }

    return true;
  }

  get disabledFolderIds(): Set<string> {
    const set = new Set<string>();

    if (this.folderModalMode === 'edit' && this.editingFolder) {
      this.collectDescendants(this.editingFolder, set);
      set.add(this.editingFolder.id);
    }

    return set;
  }

  collectDescendants(folder: any, set: Set<string>) {
    if (!folder.children) return;

    for (const child of folder.children) {
      set.add(child.id);
      this.collectDescendants(child, set);
    }
  }

  //Select to move or edit
  
  selected = new Map<string, SelectedItem>();
    getKey(type: 'folder' | 'asset', id: string) {
    return `${type}:${id}`;
  }

  isSelected(type: 'folder' | 'asset', id: string): boolean {
    return this.selected.has(this.getKey(type, id));
  }

  toggleSelect(type: 'folder' | 'asset', id: string) {
    const key = this.getKey(type, id);
    this.selected.has(key)
      ? this.selected.delete(key)
      : this.selected.set(key, { id, type });
  }

  clearSelection() {
    this.selected.clear();
  }

  // parent
  onAssetToggle(asset: MediaFile) {
    this.toggleSelect('asset', asset.id);
  }

  toggleAll() {
    const shouldSelect = this.selected.size === 0;

    this.items.forEach(i => {
      const type = i.isFolder ? 'folder' : 'asset';
      const key = this.getKey(type, i.data.id);

      shouldSelect
        ? this.selected.set(key, { id: i.data.id, type })
        : this.selected.delete(key);
    });
  }

  get selectedCount(): number {
    return this.selected.size;
  }

  // delete selected items
  deleteSelected() {
    if (this.selected.size === 0) return;

    const ok = confirm(
      `Bạn có chắc muốn xóa ${this.selected.size} item đã chọn?\n` +
      `Folder sẽ bị xóa cùng toàn bộ asset con bên trong.`
    );
    if (!ok) return;

    const tasks: Observable<any>[] = [];

    this.selected.forEach(item => {
      if (item.type === 'folder') {
        tasks.push(this.mediaService.deleteFolder(item.id));
      } else {
        tasks.push(this.mediaService.delete(item.id));
      }
    });

    forkJoin(tasks).subscribe({
      next: () => {
        this.clearSelection();
        this.reloadCurrentFolder();
        alert('Xóa thành công!');
      },
      error: () => {
        alert('Xóa thất bại!');
      }
    });
  }


  //move selected items
  moveSelected() {
    if (this.selected.size === 0) return;

    this.folderModalMode = 'move';
    this.editingFolder = null;

    this.folderForm = {
      name: '',
      parentId: this.selectedFolder?.id || null
    };

    this.folderModalOpen = true;

    this.mediaService.getTree().subscribe(res => {
      this.folderTree = res;
    });
  }

  submitMove() {
    const targetParentId = this.folderForm.parentId ?? null;
    if (!targetParentId) return;

    const ok = confirm(
      `Bạn có chắc muốn di chuyển ${this.selected.size} item ` +
      `đến folder đã chọn?`
    );
    if (!ok) return;

    const tasks: Observable<any>[] = [];

    this.selected.forEach(item => {
      if (item.type === 'folder') {
        tasks.push(
          this.mediaService.moveFolder(item.id, targetParentId)
        );
      } else {
        tasks.push(
          this.mediaService.move(item.id, targetParentId)
        );
      }
    });

    forkJoin(tasks).subscribe({
      next: () => {
        this.folderModalOpen = false;
        this.clearSelection();
        this.reloadCurrentFolder();
        alert('Di chuyển thành công!');
      },
      error: () => {
        alert('Di chuyển thất bại!');
      }
    });
  }


  reloadCurrentFolder() {
    const id = this.selectedFolder?.id || this.rootFolderId;
    this.loadFolderById(id);
    this.loadAssets(id);
  }

  //edit asset
  editingAsset: MediaFile | null = null;

  openEditAsset(asset: MediaFile) {
    this.editingAsset = asset;
  console.log('EDIT ASSET', asset);
    // nếu folderTree chưa load thì nên load ở đây
    if (!this.folderTree || this.folderTree.length === 0) {
      this.mediaService.getTree().subscribe(tree => {
        this.folderTree = tree;
      });
    }
  }


}
