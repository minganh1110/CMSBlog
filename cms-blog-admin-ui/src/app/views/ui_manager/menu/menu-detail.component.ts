import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { AdminApiMenuApiClient, AdminApiPostApiClient, CreateMenuItemRequest, MenuItemDto, PostInListDto, UpdateMenuItemRequest } from '../../../api/admin-api.service.generated';
import { MessageConstants } from '../../../shared/constants/messages.constant';
import { SelectItem } from 'primeng/api';

@Component({
    selector: 'app-menu-detail',
    templateUrl: './menu-detail.component.html'
})
export class MenuDetailComponent implements OnInit, OnDestroy {
    private ngUnsubscribe = new Subject<void>();
    menuForm: FormGroup;
    public submitted: boolean = false;
    public btnDisabled = false;
    public itemId?: string;

    // Dropdowns
    menuGroups: SelectItem[] = [];
    linkTypes: SelectItem[] = [];
    parents: SelectItem[] = [];
    entities: SelectItem[] = [];
    posts: PostInListDto[] = [];

    constructor(
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        private fb: FormBuilder,
        private menuService: AdminApiMenuApiClient,
        private postService: AdminApiPostApiClient
    ) {
        this.menuForm = this.fb.group({
            name: ['', Validators.required],
            parentId: [null],
            sortOrder: [0],
            isActive: [true],
            menuGroup: ['Main', Validators.required],
            linkType: ['Custom', Validators.required],
            entityId: [null],
            customUrl: [''],
            openInNewTab: [false]
        });
    }

    ngOnInit(): void {
        // Init Dropdown Data
        this.menuGroups = [
            { label: 'Main Menu', value: 'Main' },
            { label: 'Footer Menu', value: 'Footer' },
            { label: 'Sidebar Menu', value: 'Sidebar' }
        ];

        this.linkTypes = [
            { label: 'Custom Link', value: 'Custom' },
            { label: 'Post', value: 'Post' },
            { label: 'Category (Series)', value: 'Series' }
        ];

        this.loadParents();
        this.loadPosts();

        // Check if Update mode
        if (this.config.data?.id) {
            this.itemId = this.config.data.id;
            this.loadDetail(this.itemId);
        } else {
            // Create mode defaults
            this.onLinkTypeChange();
        }
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    loadDetail(id: string) {
        this.menuService.getMenuItem(id).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
            next: (item: MenuItemDto) => {
                this.menuForm.patchValue({
                    name: item.name,
                    parentId: item.parentId,
                    sortOrder: item.sortOrder,
                    isActive: item.isActive,
                    menuGroup: item.menuGroup,
                    linkType: item.linkType,
                    entityId: item.entityId,
                    customUrl: item.customUrl,
                    openInNewTab: item.openInNewTab
                });
                this.onLinkTypeChange();
            },
            error: (err) => {
                console.error(err);
                this.ref.close();
            }
        });
    }

    loadParents() {
        this.menuService.getMenuItems().pipe(takeUntil(this.ngUnsubscribe)).subscribe(items => {
            this.parents = items.map(x => ({ label: x.name, value: x.id }));
            this.parents.unshift({ label: 'None', value: null });
            // If update mode, filter out self to avoid recursion? Validation usually handles backend, but good UX here.
        });
    }

    loadPosts() {
        // Load simplified list or rely on specific search API. 
        // For now, loading first page as before.
        this.postService.getPostsPaging('', '', 1, 100).pipe(takeUntil(this.ngUnsubscribe)).subscribe(res => {
            this.posts = res.results || [];
            // If current type is post, update entities
            if (this.menuForm.get('linkType')?.value === 'Post') {
                this.updateEntities('Post');
            }
        });
    }

    onLinkTypeChange() {
        const type = this.menuForm.get('linkType')?.value;
        this.updateEntities(type);

        if (type === 'Custom') {
            this.menuForm.get('customUrl')?.enable();
            this.menuForm.get('entityId')?.disable();
            this.menuForm.get('entityId')?.setValue(null);
        } else {
            this.menuForm.get('customUrl')?.disable();
            this.menuForm.get('customUrl')?.setValue(null);
            this.menuForm.get('entityId')?.enable();
        }
    }

    updateEntities(type: string) {
        this.entities = [];
        if (type === 'Post') {
            this.entities = this.posts.map(p => ({ label: p.name || 'Untitled', value: p.id }));
        } else if (type === 'Series') {
            // Mock or load series if service available
            this.entities = [{ label: 'Technology', value: 'tech-id-mock' }];
        }
    }

    saveChange() {
        this.submitted = true;
        if (this.menuForm.invalid) {
            return;
        }
        this.btnDisabled = true;
        const formValue = this.menuForm.value;

        if (this.itemId) {
            // Update
            const updateReq = new UpdateMenuItemRequest({
                name: formValue.name,
                parentId: formValue.parentId,
                sortOrder: formValue.sortOrder,
                isActive: formValue.isActive,
                menuGroup: formValue.menuGroup,
                linkType: formValue.linkType,
                entityId: formValue.entityId,
                customUrl: formValue.customUrl,
                openInNewTab: formValue.openInNewTab
            });
            this.menuService.updateMenuItem(this.itemId, updateReq).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
                next: () => {
                    this.btnDisabled = false;
                    this.ref.close(true); // Return true to indicate success
                },
                error: (err) => {
                    this.btnDisabled = false;
                    console.error('Update Menu Error', err);
                }
            });
        } else {
            // Create
            const createReq = new CreateMenuItemRequest({
                name: formValue.name,
                parentId: formValue.parentId,
                sortOrder: formValue.sortOrder,
                isActive: formValue.isActive,
                menuGroup: formValue.menuGroup,
                linkType: formValue.linkType,
                entityId: formValue.entityId,
                customUrl: formValue.customUrl,
                openInNewTab: formValue.openInNewTab
            });
            this.menuService.createMenuItem(createReq).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
                next: (res) => {
                    this.btnDisabled = false;
                    this.ref.close(res);
                },
                error: (err) => {
                    this.btnDisabled = false;
                    console.error('Create Menu Error', err);
                }
            });
        }
    }
}
