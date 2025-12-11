import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MediaLibraryComponent } from './components/media-library/media-library.component';
import { MediaCardComponent } from './components/media-card/media-card.component';
import { FolderTreeComponent } from './components/folder-tree/folder-tree.component';
import { MediaRoutingModule } from './media-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    MediaLibraryComponent,
    MediaCardComponent,
    FolderTreeComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    MediaRoutingModule
  ]
})
export class MediaModule {}
