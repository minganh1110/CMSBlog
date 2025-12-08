import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { MediaFolder } from '../models/media-folder.model';
import { MediaFile } from '../models/media-asset.model'

@Injectable({ providedIn: 'root' })
export class MediaFolderService {

  private api = environment.API_URL + '/api/folders';

  constructor(private http: HttpClient) { }

  getTree(): Observable<MediaFolder[]> {
    return this.http.get<MediaFolder[]>(`${this.api}/tree`);
  }

  

  create(name: string, parentId: string | null): Observable<any> {
    return this.http.post(`${this.api}`, { name, parentId });
  }
}
