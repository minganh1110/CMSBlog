import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-folder-tree',
  templateUrl: './folder-tree.component.html',
  styleUrls: ['./folder-tree.component.scss']
})
export class FolderTreeComponent {

  @Input() folders: any[] = [];
  @Input() selectedId: string | null = null;
  @Input() disabledIds: Set<string> = new Set();

  @Output() select = new EventEmitter<any>();

  toggle(folder: any, event: MouseEvent) {
    event.stopPropagation();
    folder._open = !folder._open;
  }

  onSelect(folder: any, event: MouseEvent) {
    event.stopPropagation();

    if (this.disabledIds.has(folder.id)) return;

    this.select.emit(folder);
  }

  isDisabled(folder: any): boolean {
    return this.disabledIds.has(folder.id);
  }
}
