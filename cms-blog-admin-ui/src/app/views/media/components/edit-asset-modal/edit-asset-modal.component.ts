import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MediaFile } from '../../models/media-file.model';
import { MediaService } from '../../services/media.service';

@Component({
  selector: 'app-edit-asset-modal',
  templateUrl: './edit-asset-modal.component.html',
  styleUrls: ['./edit-asset-modal.component.scss']
})
export class EditAssetModalComponent implements OnInit {

  @Input() asset!: MediaFile;
  @Input() folderTree: any[] = [];

  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  // form fields
  form = {
    fileName: '',
    description: '',
    altText: '',
    caption: '',
    folderId: null as string | null
  };

  // replace file
  replaceFile: File | null = null;
  replacePreview: string | null = null;

  isTreeOpen = false;
  isSubmitting = false;

  constructor(private mediaService: MediaService) {}

  ngOnInit(): void {
    if (!this.asset) return;

    this.form = {
      fileName: this.asset.fileName,
      description: this.asset.description ?? '',
      altText: this.asset.altText ?? '',
      caption: this.asset.caption ?? '',
      folderId: this.asset.folderId ?? null
    };
  }

  /* ================= PREVIEW ================= */

  get previewUrl(): string | null {
    if (this.replacePreview) return this.replacePreview;
    return (
      this.asset?.formats?.thumbnail?.url ||
      this.asset?.formats?.small?.url ||
      null
    );
  }

  /* ================= REPLACE FILE ================= */

  onReplaceSelect(event: any) {
    const file = event.target.files?.[0];
    if (!file) return;

    // TODO: optional validate same media type
    this.replaceFile = file;

    const reader = new FileReader();
    reader.onload = e => (this.replacePreview = e.target?.result as string);
    reader.readAsDataURL(file);
  }

  /* ================= FOLDER TREE ================= */

  toggleTree() {
    this.isTreeOpen = !this.isTreeOpen;
  }

  onFolderSelected(folder: any) {
    if (
      this.form.folderId &&
      this.form.folderId !== this.asset.folderId
    ) {
      const ok = confirm('Bạn có chắc muốn đổi folder của asset này?');
      if (!ok) return;
    }

    this.form.folderId = folder.id;
    this.isTreeOpen = false;
  }

  getFolderName(id: string | null): string {
    if (!id) return 'Select folder';

    const find = (list: any[]): any => {
      for (const f of list) {
        if (f.id === id) return f;
        if (f.children) {
          const r = find(f.children);
          if (r) return r;
        }
      }
      return null;
    };

    return find(this.folderTree)?.folderName || 'Select folder';
  }

  /* ================= SUBMIT ================= */

  submit() {
    if (!this.form.fileName.trim()) {
      alert('File name không được để trống');
      return;
    }

    this.isSubmitting = true;

    const formData = new FormData();
    formData.append('FileName', this.form.fileName);
    formData.append('Description', this.form.description || '');
    formData.append('AltText', this.form.altText || '');
    formData.append('Caption', this.form.caption || '');
    if (this.form.folderId) {
      formData.append('FolderId', this.form.folderId);
    }
    if (this.replaceFile) {
      formData.append('File', this.replaceFile);
    }

    this.mediaService.update(this.asset.id, formData).subscribe({
      next: () => {
        alert('Cập nhật asset thành công');
        this.isSubmitting = false;
        this.saved.emit();
        this.close.emit();
      },
      error: () => {
        this.isSubmitting = false;
        alert('Cập nhật asset thất bại');
      }
    });
  }
}
