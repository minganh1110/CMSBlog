import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { AlertService } from '../../../shared/services/alert.service';
import { FooterLinkDto } from './footer-links.dto';

@Component({
  templateUrl: './footer-link-detail.component.html',
})
export class FooterLinkDetailComponent implements OnInit {
  form!: FormGroup;
  blockedPanelDetail = false;
  selectedEntity!: FooterLinkDto;

  private apiUrl = `${environment.API_URL}/api/admin/footer/links`;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private fb: FormBuilder,
    private http: HttpClient,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.buildForm();

    if (this.config.data?.id) {
      this.loadDetail(this.config.data.id);
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      label: ['', [Validators.required, Validators.maxLength(250)]],
      url: ['', [Validators.required, Validators.maxLength(500)]],
      icon: ['', [Validators.maxLength(100)]],
      targetBlank: [false],
      sortOrder: [0, Validators.required],
      isActive: [true],
    });
  }

  private loadDetail(id: number): void {
    this.toggleBlockUI(true);

    this.http.get<FooterLinkDto>(`${this.apiUrl}/${id}`).subscribe({
      next: (res) => {
        this.selectedEntity = res;
        this.form.patchValue(res);
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Không tải được dữ liệu liên kết');
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
            this.alertService.showSuccess('Cập nhật thành công');
            this.ref.close(true);
          },
          error: () => {
            this.toggleBlockUI(false);
            this.alertService.showError('Cập nhật thất bại');
          },
        });
      return;
    }

    // CREATE
    this.http.post(this.apiUrl, payload).subscribe({
      next: () => {
        this.toggleBlockUI(false);
        this.alertService.showSuccess('Thêm mới thành công');
        this.ref.close(true);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Thêm mới thất bại');
      },
    });
  }

  private toggleBlockUI(enabled: boolean): void {
    if (enabled) {
      this.blockedPanelDetail = true;
    } else {
      setTimeout(() => (this.blockedPanelDetail = false), 300);
    }
  }
}
