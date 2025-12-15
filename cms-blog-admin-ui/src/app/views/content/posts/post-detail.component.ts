import { Component, OnInit, EventEmitter, OnDestroy, ChangeDetectorRef, ViewChild, AfterViewInit } from '@angular/core';
import { Validators, FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { Editor } from 'primeng/editor';
import { AdminApiPostApiClient, AdminApiPostCategoryApiClient, PostCategoryDto, PostDto } from 'src/app/api/admin-api.service.generated';
import { UploadService } from 'src/app/shared/services/upload.service';
import { environment } from 'src/environments/environment';

interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

@Component({
  templateUrl: 'post-detail.component.html',
})
export class PostDetailComponent implements OnInit, OnDestroy, AfterViewInit {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public form!: FormGroup;
  public title!: string;
  public btnDisabled = false;
  public saveBtnName!: string;
  public postCategories: any[] = [];
  public contentTypes: any[] = [];
  public series: any[] = [];

  selectedEntity = {} as PostDto;
  public thumbnailImage: string = '';

  @ViewChild('editor') editor: Editor | undefined;

  tags: string[] | undefined;
  filteredTags: string[] | undefined;
  postTags!: string[];

  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private utilService: UtilityService,
    private fb: FormBuilder,
    private postApiClient: AdminApiPostApiClient,
    private postCategoryApiClient: AdminApiPostCategoryApiClient,
    private uploadService: UploadService,
    private cdr: ChangeDetectorRef
  ) { }
  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngAfterViewInit(): void {
    // If form already exists and content is present, ensure editor shows it
    setTimeout(() => {
      this.applyContentToEditor();
    }, 0);
  }

  public generateSlug() {
    var slug = this.utilService.makeSeoTitle(this.form.get('name').value);
    this.form.controls['slug'].setValue(slug);
  }
  // Validate
  noSpecial: RegExp = /^[^<>*!_~]+$/;
  validationMessages = {
    name: [
      { type: 'required', message: 'Bạn phải nhập tên' },
      { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
    ],
    slug: [{ type: 'required', message: 'Bạn phải URL duy nhất' }],
    description: [{ type: 'required', message: 'Bạn phải nhập mô tả ngắn' }],
  };

  ngOnInit() {
    //Load data to form
    var categories = this.postCategoryApiClient.getPostCategories();
    var tags = this.postApiClient.getAllTags(); 
    this.toggleBlockUI(true);
    forkJoin({
      categories,
      tags
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (repsonse: any) => {
          this.tags = repsonse.tags as string[];
          //Push categories to dropdown list
          var categories = repsonse.categories as PostCategoryDto[];
          categories.forEach(element => {
            this.postCategories.push({
              value: element.id,
              label: element.name,
            });
          });
          if (this.utilService.isEmpty(this.config.data?.id) == false) {
               this.postApiClient
              .getPostTags(this.config.data.id)
              .subscribe((res) => {
                this.postTags = res;
                this.loadFormDetails(this.config.data?.id);
              });
          } else {
            //Init form for create new post
            this.buildForm();
            this.toggleBlockUI(false);
          }
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  loadFormDetails(id: string) {
    this.postApiClient
      .getPostById(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostDto) => {
          this.selectedEntity = response;
            console.log('Loaded post content:', this.selectedEntity?.content);
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }


  onFileChange(event) {
    if (event.target.files && event.target.files.length) {
      this.uploadService.uploadImage('posts', event.target.files)
        .subscribe({
          next: (response: any) => {
            this.form.controls['thumbnail'].setValue(response.path);
            this.thumbnailImage = environment.API_URL + response.path;
          },
          error: (err: any) => {
            console.log(err);
          }
        });
    }
  }
  saveChange() {
    this.toggleBlockUI(true);
    this.saveData();
  }

  private saveData() {
    this.toggleBlockUI(true);
    
    // Extract ID from category object
    const formData = this.form.value;
    if (formData.categoryId && typeof formData.categoryId === 'object') {
      formData.categoryId = formData.categoryId.value;
    }
    
    if (this.utilService.isEmpty(this.config.data?.id)) {
      this.postApiClient
        .createPost(formData)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.ref.close(formData);
            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } else {
      this.postApiClient
        .updatePost(this.config.data?.id, formData)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.toggleBlockUI(false);

            this.ref.close(this.form.value);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    }
  }
  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.btnDisabled = true;
      this.blockedPanelDetail = true;
    } else {
      setTimeout(() => {
        this.btnDisabled = false;
        this.blockedPanelDetail = false;
      }, 1000);
    }
  }
  buildForm() {
    // Find the selected category object from postCategories array
    const selectedCategory = this.postCategories.find(cat => cat.value === this.selectedEntity.categoryId);
    
    this.form = this.fb.group({
      name: new FormControl(
        this.selectedEntity.name || null,
        Validators.compose([
          Validators.required,
          Validators.maxLength(255),
          Validators.minLength(3),
        ])
      ),
      slug: new FormControl(this.selectedEntity.slug || null, Validators.required),
      categoryId: new FormControl(selectedCategory || null),
      description: new FormControl(this.selectedEntity.description || null, Validators.required),
      seoDescription: new FormControl(this.selectedEntity.seoDescription || null),
      content: new FormControl(this.selectedEntity.content || null),
      thumbnail: new FormControl(
        this.selectedEntity.thumbnail || null
      ),
       tags: new FormControl(this.postTags),
    });
    if (this.selectedEntity.thumbnail) {
      this.thumbnailImage = environment.API_URL + this.selectedEntity.thumbnail;
    }
    // Ensure the rich text editor gets the content value (helps when editor initializes
    // after the form control is created). Explicitly set the value and trigger change
    // detection so PrimeNG `p-editor` shows the loaded HTML content.
    try {
      const contentCtrl = this.form.get('content');
      if (contentCtrl && this.selectedEntity.content !== undefined && this.selectedEntity.content !== null) {
        contentCtrl.setValue(this.selectedEntity.content);
      }
    } catch (e) {
      // ignore
    }
    this.cdr.detectChanges();
    // also try to apply content directly to Quill editor if available
    setTimeout(() => {
      this.applyContentToEditor();
    }, 0);
  }

  private applyContentToEditor() {
    try {
      const html = this.selectedEntity?.content || this.form?.get('content')?.value || '';
      if (this.editor && this.editor.quill) {
        // Use Quill's API to insert HTML safely
        this.editor.quill.clipboard.dangerouslyPasteHTML(html);
      }
    } catch (e) {
      // ignore failures
    }
  }
    filterTag(event: AutoCompleteCompleteEvent) {
    let filtered: string[] = [];
    let query = event.query;

    for (let i = 0; i < (this.tags as string[]).length; i++) {
      let tag = (this.tags as string[])[i];
      if (tag.toLowerCase().indexOf(query.toLowerCase()) == 0) {
        filtered.push(tag);
      }
    }
    if (filtered.length == 0) {
      filtered.push(query);
    }
    this.filteredTags = filtered;
  }

}