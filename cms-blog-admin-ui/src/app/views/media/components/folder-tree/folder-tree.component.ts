import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MediaFolder } from '../../models/media-folder.model';

@Component({
  selector: 'app-folder-tree',
  templateUrl: './folder-tree.component.html',
  styleUrls: ['./folder-tree.component.scss']
})
export class FolderTreeComponent {
  @Input() folders: MediaFolder[] = [];
  @Output() select = new EventEmitter<MediaFolder>();
}
