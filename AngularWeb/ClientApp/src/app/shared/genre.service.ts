import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthorizationService } from './authorization.service';
import { GenreDto } from './genre-dto.model';
import { MetaData } from './meta-data.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor(private http: HttpClient, public authService: AuthorizationService) { }

  readonly _baseUrl = "https://localhost:7003/api/genres";
  formData: GenreDto = new GenreDto();
  options: string = '';
  fields: string = "name description";
  list: GenreDto[] = [];
  metaData: MetaData = new MetaData;

  postGenre() {
    return this.http.post(this._baseUrl, this.formData);
  }

  putGenre() {
    return this.http.put(this._baseUrl + "/" + this.formData.id, this.formData);
  }

  deleteGenre(id: number) {
    return this.http.delete(this._baseUrl + "/" + id);
  }

  refreshList() {
    if (this.authService.isAuth) {
      this.http.get(this._baseUrl + "/metaData" + this.options)
        .toPromise()
        .then(res => this.metaData = res as MetaData);
      this.http.get(this._baseUrl + this.options)
        .toPromise()
        .then(res => this.list = res as GenreDto[]);
    }
  }
}
