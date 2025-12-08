import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MediaFolder } from '../models/media-folder.model';

@Component({
  selector: 'app-media-folder-node',
  templateUrl: './media-folder-node.component.html',
  styleUrls: ['./media-folder-node.component.scss']
})
export class MediaFolderNodeComponent {
  @Input() folder!: MediaFolder;
  @Input() selectedFolderId: string | null = null;

  @Output() select = new EventEmitter<MediaFolder>();

  onSelect() {
    this.select.emit(this.folder);
  }
}
