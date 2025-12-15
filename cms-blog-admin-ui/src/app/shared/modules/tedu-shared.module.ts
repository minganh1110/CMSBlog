import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ValidationMessageComponent } from '../modules/validation-message/validation-message.component';
import { PermissionDirective } from './permission.directive';
@NgModule({
  imports: [CommonModule],
  declarations: [ValidationMessageComponent,PermissionDirective],
  exports: [ValidationMessageComponent],
})
export class TeduSharedModule {}