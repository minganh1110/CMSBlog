import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { FooterLinkDto } from './footer-links.dto';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FooterLinkDetailComponent } from './footer-link-detail.component';
import { AlertService } from '../../../shared/services/alert.service';

@Component({
  selector: 'app-footer-links',
  templateUrl: './footer-links.component.html',
  providers: [DialogService],
})
export class FooterLinksComponent implements OnInit, OnDestroy {
  links: FooterLinkDto[] = [];
  selectedLinks: FooterLinkDto[] = [];
  blockedPanel = false;

  private apiUrl = `${environment.API_URL}/api/admin/footer/links`;
  private ref?: DynamicDialogRef;

  constructor(
    private http: HttpClient,
    private dialogService: DialogService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngOnDestroy(): void {
    this.ref?.close();
  }

  loadData(): void {
    this.toggleBlockUI(true);

    this.http.get<FooterLinkDto[]>(this.apiUrl).subscribe({
      next: (res) => {
        this.links = res;
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
        this.alertService.showError('Không tải được danh sách liên kết');
      },
    });
  }

  showAddModal(): void {
    this.ref = this.dialogService.open(FooterLinkDetailComponent, {
      header: 'Thêm liên kết footer',
      width: '600px',
      data: null,
      closable: true,
    });

    this.ref.onClose.subscribe((result) => {
      if (result) {
        this.loadData();
      }
    });
  }

  showEditModal(): void {
    if (this.selectedLinks.length !== 1) {
      return;
    }

    const id = this.selectedLinks[0].id;

    this.ref = this.dialogService.open(FooterLinkDetailComponent, {
      header: 'Cập nhật liên kết footer',
      width: '600px',
      data: { id },
      closable: true,
    });

    this.ref.onClose.subscribe((result) => {
      if (result) {
        this.loadData();
      }
    });
  }

  deleteItems(): void {
    if (this.selectedLinks.length === 0) {
      return;
    }

    const ids = this.selectedLinks.map((x) => x.id);

    this.toggleBlockUI(true);

    this.http
      .request('delete', this.apiUrl, {
        body: ids,
      })
      .subscribe({
        next: () => {
          this.alertService.showSuccess('Xóa thành công');
          this.selectedLinks = [];
          this.toggleBlockUI(false);
          this.loadData();
        },
        error: () => {
          this.toggleBlockUI(false);
          this.alertService.showError('Xóa thất bại');
        },
      });
  }

  toggleBlockUI(enabled: boolean): void {
    if (enabled) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => (this.blockedPanel = false), 300);
    }
  }
}
