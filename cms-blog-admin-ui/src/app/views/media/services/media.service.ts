import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MediaFile, MediaType } from '../models/media-file.model';
import { MediaFolder } from '../models/media-folder.model';
import { environment } from '../../../../environments/environment';

const API_BASE = environment.API_URL + '/api'; // <-- adjust

@Injectable({ providedIn: 'root' })
export class MediaService {
  constructor(private http: HttpClient) {}

  // MediaFile endpoints
  getAll(): Observable<MediaFile[]> {
    return this.http.get<MediaFile[]>(`${API_BASE}/media`);
  }

  getById(id: string) {
    return this.http.get<MediaFile>(`${API_BASE}/media/${id}`);
  }

  getInFolder(folderId: string) {
    return this.http.get<MediaFile[]>(`${API_BASE}/media/folder/${folderId}`);
  }

  upload(formData: FormData) {
    return this.http.post(`${API_BASE}/media/upload-multiple`, formData);
  }

  move(id: string, newParentId: string | null) {
    return this.http.patch(`${API_BASE}/media/${id}/move`, { parentId: newParentId });
  }

  update(id: string, payload: any) {
    return this.http.patch(`${API_BASE}/media/${id}/update`, payload);
  }

  delete(id: string) {
    return this.http.delete(`${API_BASE}/media/${id}`);
  }

  // MediaFolder endpoints
  createFolder(payload: any) {
    return this.http.post(`${API_BASE}/folders`, payload);
  }

  getTree() {
    return this.http.get<MediaFolder[]>(`${API_BASE}/folders/tree`);
  }

  getFolderById(id: string) {
    return this.http.get<MediaFolder>(`${API_BASE}/folders/${id}`);
  }

  getFolderChildren(id: string) {
    return this.http.get<MediaFolder[]>(`${API_BASE}/folders/${id}/children`);
  }

  getAllInfolder(id: string) {
    return this.http.get<MediaFolder[]>(`${API_BASE}/folders/${id}/files`);
  }

  moveFolder(id: string, newParentId: string | null) {
    return this.http.patch(`${API_BASE}/folders/${id}/move`, { parentId: newParentId });
  }

  editFolder(id: string, payload: any) {
    return this.http.patch(`${API_BASE}/folders/${id}/edit`, payload);
  }

  deleteFolder(id: string) {
    return this.http.delete(`${API_BASE}/folders/${id}`);
  }


}
