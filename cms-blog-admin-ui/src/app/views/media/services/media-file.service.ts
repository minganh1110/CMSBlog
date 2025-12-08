import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { MediaFolder } from '../models/media-folder.model';
import { MediaFile } from '../models/media-asset.model';

@Injectable({ providedIn: 'root' })
export class MediaFileService {

  private api = environment.API_URL + '/api/media';

  constructor(private http: HttpClient) { }

  getAssetOnFolder(folderId: string):Observable<MediaFile[]>{
    return this.http.get<MediaFile[]>(`${this.api}/folder/${folderId}`)
  }

  getAssets():Observable<MediaFile[]>{
    return this.http.get<MediaFile[]>(`${this.api}`)
  }

  

  create(name: string, parentId: string | null): Observable<any> {
    return this.http.post(`${this.api}`, { name, parentId });
  }
}