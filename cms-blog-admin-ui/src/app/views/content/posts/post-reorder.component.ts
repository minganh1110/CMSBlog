import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { AdminApiPostApiClient, PostInListDto, UpdatePostSortOrderRequest } from 'src/app/api/admin-api.service.generated';
import { AlertService } from 'src/app/shared/services/alert.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
    selector: 'app-post-reorder',
    templateUrl: './post-reorder.component.html',
    styleUrls: ['./post-reorder.component.scss']
})
export class PostReorderComponent implements OnInit {
    posts: PostInListDto[] = [];
    blockedPanel: boolean = false;

    constructor(
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        private postApiClient: AdminApiPostApiClient,
        private alertService: AlertService
    ) { }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this.toggleBlockUI(true);
        const categoryId = this.config.data?.categoryId;

        this.postApiClient.getPostsPaging('', categoryId, 1, 100).subscribe({
            next: (response) => {
                this.posts = response.results;
                // Sort by SortOrder to ensure initial order is correct
                this.posts.sort((a, b) => (a.sortOrder || 0) - (b.sortOrder || 0));
                this.toggleBlockUI(false);
            },
            error: () => {
                this.toggleBlockUI(false);
            }
        });
    }

    drop(event: CdkDragDrop<string[]>) {
        moveItemInArray(this.posts, event.previousIndex, event.currentIndex);
    }

    saveOrder() {
        this.toggleBlockUI(true);
        const request = new UpdatePostSortOrderRequest();
        request.postIds = this.posts.map(p => p.id);

        this.postApiClient.updateSortOrder(request).subscribe({
            next: () => {
                this.alertService.showSuccess('Cập nhật thứ tự thành công');
                this.ref.close(true);
                this.toggleBlockUI(false);
            },
            error: () => {
                this.alertService.showError('Có lỗi xảy ra khi cập nhật thứ tự');
                this.toggleBlockUI(false);
            }
        });
    }

    close() {
        this.ref.close();
    }

    private toggleBlockUI(enabled: boolean) {
        this.blockedPanel = enabled;
    }
}