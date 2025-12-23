import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { Subject, takeUntil, forkJoin } from 'rxjs';
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

        this.loadData();
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    loadData() {
        // Load dependencies first
        const parents$ = this.menuService.getMenuItems();
        const posts$ = this.postService.getPostsPaging('', '', 1, 100);

        forkJoin({
            parents: parents$,
            posts: posts$
        }).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
            next: (result: any) => {
                // Process Parents
                this.parents = result.parents.map((x: any) => ({ label: x.name, value: x.id }));
                this.parents.unshift({ label: 'None', value: null });

                // Process Posts
                this.posts = result.posts.results || [];

                // Check Validation
                if (this.config.data?.id) {
                    this.itemId = this.config.data.id;
                    this.loadDetail(this.itemId!);
                } else {
                    this.onLinkTypeChange();
                }
            },
            error: (err) => {
                console.error(err);
                this.ref.close(); // Close if deps fail? Or allow continue? Use caution.
            }
        });
    }

    loadDetail(id: string) {
        this.menuService.getMenuItem(id).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
            next: (item: MenuItemDto) => {
                // Populate entities based on saved link type
                this.updateEntities(item.linkType);

                // Find the matching object for entityId
                let selectedEntity = null;
                if (item.entityId && this.entities.length > 0) {
                    selectedEntity = this.entities.find(x => x.value === item.entityId);
                }

                // Find the matching object for parentId
                let selectedParent = null;
                if (this.parents.length > 0) {
                    if (item.parentId) {
                        selectedParent = this.parents.find(x => x.value === item.parentId);
                    } else {
                        // Select 'None' option if parentId is null
                        selectedParent = this.parents.find(x => x.value === null);
                    }
                }

                this.menuForm.patchValue({
                    name: item.name,
                    parentId: selectedParent,
                    sortOrder: item.sortOrder,
                    isActive: item.isActive,
                    menuGroup: item.menuGroup,
                    linkType: item.linkType,
                    entityId: selectedEntity, // Bind the object
                    customUrl: item.customUrl,
                    openInNewTab: item.openInNewTab
                });

                // Set enable/disable state without resetting values
                if (item.linkType === 'Custom') {
                    this.menuForm.get('customUrl')?.enable();
                    this.menuForm.get('entityId')?.disable();
                } else {
                    this.menuForm.get('customUrl')?.disable();
                    this.menuForm.get('entityId')?.enable();
                }
            },
            error: (err) => {
                console.error(err);
                this.ref.close();
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

        // Extract ID if entityId is an object
        let entityIdValue = formValue.entityId;
        if (entityIdValue && typeof entityIdValue === 'object') {
            entityIdValue = entityIdValue.value;
        }

        // Extract ID if parentId is an object
        let parentIdValue = formValue.parentId;
        if (parentIdValue && typeof parentIdValue === 'object') {
            parentIdValue = parentIdValue.value;
        }

        if (this.itemId) {
            // Update
            const updateReq = new UpdateMenuItemRequest({
                name: formValue.name,
                parentId: parentIdValue,
                sortOrder: formValue.sortOrder,
                isActive: formValue.isActive,
                menuGroup: formValue.menuGroup,
                linkType: formValue.linkType,
                entityId: entityIdValue,
                customUrl: formValue.customUrl,
                openInNewTab: formValue.openInNewTab
            });
            this.menuService.updateMenuItem(this.itemId, updateReq).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
                next: () => {
                    this.btnDisabled = false;
                    this.ref.close(true);
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
                parentId: parentIdValue,
                sortOrder: formValue.sortOrder,
                isActive: formValue.isActive,
                menuGroup: formValue.menuGroup,
                linkType: formValue.linkType,
                entityId: entityIdValue,
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
