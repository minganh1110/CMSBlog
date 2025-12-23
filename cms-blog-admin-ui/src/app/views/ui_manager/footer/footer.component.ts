import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';

import { environment } from '../../../../environments/environment';
import { AlertService } from '../../../shared/services/alert.service';
import { FooterSettingsDto } from './footer-settings.dto';
import { FooterSettingsDetailComponent } from './footer-settings-detail.component';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  providers: [DialogService],
})
export class FooterComponent implements OnInit {
  settings: FooterSettingsDto[] = [];
  selectedSettings: FooterSettingsDto[] = [];
  blockedPanel = false;

  private readonly apiUrl = `${environment.API_URL}/api/admin/footer`;
  ref!: DynamicDialogRef;

  constructor(
    private http: HttpClient,
    private dialogService: DialogService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadSettings();
  }

  loadSettings(): void {
    this.toggleBlockUI(true);
    this.http.get<FooterSettingsDto[]>(`${this.apiUrl}/settings`).subscribe({
      next: (res) => {
        this.settings = res;
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Không tải được thông tin footer');
      },
    });
  }

  showAddModal(): void {
    this.ref = this.dialogService.open(FooterSettingsDetailComponent, {
      header: 'Thêm thông tin footer',
      width: '700px',
    });

    this.ref.onClose.subscribe((reload) => {
      if (reload) {
        this.loadSettings();
      }
    });
  }

  showEditModal(): void {
    if (this.selectedSettings.length !== 1) return;

    const id = this.selectedSettings[0].id;

    this.ref = this.dialogService.open(FooterSettingsDetailComponent, {
      header: 'Cập nhật thông tin footer',
      width: '700px',
      data: { id },
    });

    this.ref.onClose.subscribe((reload) => {
      if (reload) {
        this.loadSettings();
      }
    });
  }

  deleteItems(): void {
    if (this.selectedSettings.length === 0) return;

    const ids = this.selectedSettings.map((x) => x.id);

    this.toggleBlockUI(true);
    this.http
      .delete(`${this.apiUrl}/settings`, {
        params: { ids: ids.join(',') },
      })
      .subscribe({
        next: () => {
          this.alertService.showSuccess('Xóa thành công');
          this.selectedSettings = [];
          this.loadSettings();
        },
        error: () => {
          this.toggleBlockUI(false);
          this.alertService.showError('Xóa thất bại');
        },
      });
  }

  private toggleBlockUI(enable: boolean): void {
    if (enable) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => (this.blockedPanel = false), 300);
    }
  }
}
