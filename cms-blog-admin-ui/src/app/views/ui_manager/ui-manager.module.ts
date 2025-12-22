import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import {
    CardModule,
    GridModule,
    ButtonModule as CoreUiButtonModule,
    FormModule,
    TableModule as CoreUiTableModule,
    UtilitiesModule
} from '@coreui/angular';
import { IconModule } from '@coreui/icons-angular';

import { UiManagerRoutingModule } from './ui-manager-routing.module';
import { MenuComponent } from './menu/menu.component';
import { MenuDetailComponent } from './menu/menu-detail.component';

// PrimeNG Modules
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { CalendarModule } from 'primeng/calendar';
import { SliderModule } from 'primeng/slider';
import { MultiSelectModule } from 'primeng/multiselect';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { ProgressBarModule } from 'primeng/progressbar';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToolbarModule } from 'primeng/toolbar';
import { RatingModule } from 'primeng/rating';
import { RadioButtonModule } from 'primeng/radiobutton';
import { InputNumberModule } from 'primeng/inputnumber';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { CheckboxModule } from 'primeng/checkbox';
import { PanelModule } from 'primeng/panel';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BadgeModule } from 'primeng/badge';
import { ConfirmationService, MessageService } from 'primeng/api';

@NgModule({
    declarations: [
        MenuComponent,
        MenuDetailComponent
    ],
    imports: [
        CommonModule,
        UiManagerRoutingModule,
        ReactiveFormsModule,
        FormsModule,

        // CoreUI
        CardModule,
        GridModule,
        CoreUiButtonModule,
        FormModule,
        CoreUiTableModule,
        IconModule,
        UtilitiesModule,

        // PrimeNG
        TableModule,
        ToastModule,
        CalendarModule,
        SliderModule,
        MultiSelectModule,
        ContextMenuModule,
        DialogModule,
        ButtonModule,
        DropdownModule,
        ProgressBarModule,
        InputTextModule,
        FileUploadModule,
        ToolbarModule,
        RatingModule,
        RadioButtonModule,
        InputNumberModule,
        ConfirmDialogModule,
        InputTextareaModule,
        CheckboxModule,
        PanelModule,
        BlockUIModule,
        ProgressSpinnerModule,
        BadgeModule
    ],
    providers: [MessageService, ConfirmationService]
})
export class UiManagerModule { }
