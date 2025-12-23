import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { UiRoutingModule } from './ui-routing.module';

// PrimeNG
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DialogModule } from 'primeng/dialog';
import { TabViewModule } from 'primeng/tabview';
import { PanelModule } from 'primeng/panel';
// Components
import { FooterComponent } from './footer/footer.component';
import { FooterSettingsDetailComponent } from './footer/footer-settings-detail.component';
import { FooterLinksComponent } from './footer/footer-links.component';
import { FooterLinkDetailComponent } from './footer/footer-link-detail.component';

@NgModule({
  declarations: [
    FooterComponent,
    FooterSettingsDetailComponent,
    FooterLinksComponent,
    FooterLinkDetailComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UiRoutingModule,
    PanelModule,
    // PrimeNG
    TableModule,
    ButtonModule,
    InputTextModule,
    BlockUIModule,
    ProgressSpinnerModule,
    DialogModule,
    TabViewModule,
  ],
})
export class UiModule {}
