import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { MediaFolderTreeComponent } from './media-folder-tree/media-folder-tree.component';
import { MediaFolderNodeComponent } from './media-folder-node/media-folder-node.component'; 
import { MediaExplorerComponent } from './media-explorer/media-explorer.component';  
import { MediaFolderContentComponent } from './media-folder-content/media-folder-content.component';

@NgModule({
  declarations: [
    MediaFolderTreeComponent,
    MediaFolderNodeComponent,
    MediaExplorerComponent,
    MediaFolderContentComponent 
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { path: '', component: MediaExplorerComponent }
    ])
  ]
})
export class MediaModule { }
