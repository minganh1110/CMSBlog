import { Component, OnInit, OnDestroy } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { AdminApiMenuApiClient, MenuItemDto } from '../../../api/admin-api.service.generated';
import { AlertService } from '../../../shared/services/alert.service';
import { MessageConstants } from '../../../shared/constants/messages.constant';
import { MenuDetailComponent } from './menu-detail.component';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit, OnDestroy {
    // System variables
    private ngUnsubscribe = new Subject<void>();
    public blockedPanel: boolean = false;

    // Paging variables
    public pageIndex: number = 1;
    public pageSize: number = 10;
    public totalCount: number = 0;

    // Business variables
    public items: MenuItemDto[] = [];
    public selectedItems: MenuItemDto[] = [];
    public keyword: string = '';

    constructor(
        private menuService: AdminApiMenuApiClient,
        public dialogService: DialogService,
        private alertService: AlertService,
        private confirmationService: ConfirmationService
    ) { }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this.toggleBlockUI(true);
        // Note: getMenuItems currently returns all items. 
        // If backend supports paging, use it. For now assuming client-side or small list.
        this.menuService.getMenuItems()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe({
                next: (data: MenuItemDto[]) => {
                    this.items = data;
                    if (this.keyword) {
                        this.items = this.items.filter(x => (x.name || '').toLowerCase().includes(this.keyword.toLowerCase()));
                    }
                    this.totalCount = this.items.length;
                    this.toggleBlockUI(false);
                },
                error: () => {
                    this.toggleBlockUI(false);
                }
            });
    }

    showAddModal() {
        const ref = this.dialogService.open(MenuDetailComponent, {
            header: 'Thêm mới menu',
            width: '70%'
        });
        const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
        const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
        const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
        dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
        ref.onClose.subscribe((data: MenuItemDto) => {
            if (data) {
                this.selectedItems = [];
                this.loadData();
                this.alertService.showSuccess(MessageConstants.CREATED_OK_MSG);
            }
        });
    }

    showEditModal() {
        if (this.selectedItems.length == 0) {
            this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
            return;
        }
        var id = this.selectedItems[0].id; // Assuming ID is string or compatible
        const ref = this.dialogService.open(MenuDetailComponent, {
            data: {
                id: id
            },
            header: 'Cập nhật menu',
            width: '70%'
        });
        const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
        const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
        const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
        dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
        ref.onClose.subscribe((data: MenuItemDto) => {
            if (data) {
                this.selectedItems = [];
                this.loadData();
                this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
            }
        });
    }

    deleteItems() {
        if (this.selectedItems.length == 0) {
            this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
            return;
        }
        var ids: string[] = [];
        this.selectedItems.forEach(element => {
            ids.push(element.id);
        });
        this.confirmationService.confirm({
            message: MessageConstants.CONFIRM_DELETE_MSG,
            accept: () => {
                this.deleteItemsConfirm(ids);
            }
        });
    }

    deleteItemsConfirm(ids: string[]) {
        this.toggleBlockUI(true);
        let completed = 0;
        let errors: any[] = [];

        ids.forEach(id => {
            this.menuService.deleteMenuItem(id).subscribe({
                next: () => {
                    completed++;
                    if (completed === ids.length) {
                        this.finalizeDelete(errors);
                    }
                },
                error: (err) => {
                    completed++;
                    errors.push(err);
                    if (completed === ids.length) {
                        this.finalizeDelete(errors);
                    }
                }
            });
        });
    }

    finalizeDelete(errors: any[]) {
        this.toggleBlockUI(false);
        if (errors.length > 0) {
            this.alertService.showError('Có lỗi xảy ra khi xóa ' + errors.length + ' bản ghi. Vui lòng kiểm tra lại.');
            console.error(errors);
        } else {
            this.alertService.showSuccess(MessageConstants.DELETED_OK_MSG);
            this.selectedItems = [];
        }
        this.loadData();
    }

    private toggleBlockUI(enabled: boolean) {
        if (enabled == true) {
            this.blockedPanel = true;
        }
        else {
            setTimeout(() => {
                this.blockedPanel = false;
            }, 1000);
        }
    }
}
