import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MediaLibraryComponent } from './components/media-library/media-library.component';

const routes: Routes = [
  { path: '', component: MediaLibraryComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MediaRoutingModule {}
