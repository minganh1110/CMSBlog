import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../environments/environment';
import { AlertService } from '../../../shared/services/alert.service';
import { FooterSettingsDto } from './footer-settings.dto';

@Component({
  selector: 'app-footer-settings-detail',
  templateUrl: './footer-settings-detail.component.html',
})
export class FooterSettingsDetailComponent implements OnInit {
  form!: FormGroup;
  blockedPanelDetail = false;
  selectedEntity?: FooterSettingsDto;

  private readonly apiUrl = `${environment.API_URL}/api/admin/footer/settings`;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private fb: FormBuilder,
    private http: HttpClient,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    const id = this.config.data?.id;
    if (id) {
      this.loadDetail(id);
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      title: ['', Validators.maxLength(250)],
      subtitle: ['', Validators.maxLength(500)],
      copyrightText: ['', Validators.maxLength(500)],
      footerNote: ['', Validators.maxLength(500)],
      contactEmail: ['', [Validators.maxLength(250), Validators.email]],
      phone: ['', Validators.maxLength(50)],
      address: ['', Validators.maxLength(500)],
      logoUrl: ['', Validators.maxLength(500)],
      isActive: [true],
    });
  }

  private loadDetail(id: string): void {
    this.toggleBlockUI(true);

    this.http.get<FooterSettingsDto>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.selectedEntity = res;
        this.form.patchValue(res);
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Không tải được dữ liệu footer');
      },
    });
  }

  saveChange(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.toggleBlockUI(true);
    const payload = this.form.value;

    // UPDATE
    if (this.selectedEntity?.id) {
      this.http
        .put(`${this.apiUrl}/${this.selectedEntity.id}`, payload)
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);
            this.alertService.showSuccess('Cập nhật footer thành công');
            this.ref.close(true);
          },
          error: () => {
            this.toggleBlockUI(false);
            this.alertService.showError('Có lỗi xảy ra khi cập nhật');
          },
        });
      return;
    }

    // CREATE
    this.http.post(this.apiUrl, payload).subscribe({
      next: () => {
        this.toggleBlockUI(false);
        this.alertService.showSuccess('Tạo footer thành công');
        this.ref.close(true);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Có lỗi xảy ra khi tạo mới');
      },
    });
  }

  private toggleBlockUI(enable: boolean): void {
    if (enable) {
      this.blockedPanelDetail = true;
      this.form.disable();
    } else {
      setTimeout(() => {
        this.blockedPanelDetail = false;
        this.form.enable();
      }, 500);
    }
  }
}
