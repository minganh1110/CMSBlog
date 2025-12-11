import { Component, Input } from '@angular/core';
import { MediaFile } from '../../models/media-file.model';

@Component({
  selector: 'app-media-card',
  templateUrl: './media-card.component.html',
  styleUrls: ['./media-card.component.scss']
})
export class MediaCardComponent {
  @Input() file!: MediaFile;
  @Input() listView = false;

  getThumb(): string | null {
    return this.file.formats?.thumbnail?.url ?? this.file.formats?.small?.url ?? null;
  }
}
