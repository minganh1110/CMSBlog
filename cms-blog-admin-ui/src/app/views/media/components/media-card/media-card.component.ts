import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MediaFile, MediaType } from '../../models/media-file.model';

@Component({
  selector: 'app-media-card',
  templateUrl: './media-card.component.html',
  styleUrls: ['./media-card.component.scss']
})
export class MediaCardComponent {
  @Input() file!: MediaFile;
  @Input() listView = false;
  @Input() isFolder: boolean = false;
  @Input() isSelected: boolean = false;
  @Input() MediaType = MediaType;

  @Output() toggle = new EventEmitter<void>();
  @Output() edit = new EventEmitter<MediaFile>();



  getThumb(): string | null {
    const base = this.file.formats?.thumbnail?.url ?? this.file.formats?.small?.url ?? null;
    if (!base) return null;

    return `${base}?v=${this.file.dateModified}`;
  }

}