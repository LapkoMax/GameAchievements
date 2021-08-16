import { Injectable } from '@angular/core';
import { GameDto } from './game-dto.model';
import { GenreDto } from './genre-dto.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MetaData } from './meta-data.model';
import { AuthorizationService } from './authorization.service';

@Injectable({
  providedIn: 'root'
})
export class GameDtoService {

  constructor(public http: HttpClient, public authService: AuthorizationService) { }

  readonly _baseUrl = "https://localhost:7003/api/games";
  readonly _baseGenreUrl = "https://localhost:7003/api/genres";
  formData: GameDto = new GameDto();
  genreIds: string = '';
  options: string = '';
  fields: string = "name description rating genres";
  gameGenres: GenreDto[] = [];
  list: GameDto[] = [];
  metaData: MetaData = new MetaData;
  genresList: GenreDto[] = [];

  postGame() {
    return this.http.post(this._baseUrl, this.formData);
  }

  putGame() {
    return this.http.put(this._baseUrl + "/" + this.formData.id, this.formData);
  }

  deleteGame(id: number) {
    return this.http.delete(this._baseUrl + "/" + id);
  }

  updateGenres(genreIds: string) {
    const params = new HttpParams()
      .set('genreIds', genreIds.trim());
    return this.http.put(this._baseUrl + "/genres/" + this.formData.id, this.formData, { params: params });
  }

  getGameGenres(id: number) {
    return this.http.get(this._baseUrl + "/" + id + "/gameGenres")
      .toPromise()
      .then(res => this.gameGenres = res as GenreDto[]);
  }

  refreshList() {
    if (this.authService.isAuth) {
      this.http.get(this._baseUrl + "/metaData" + this.options)
        .toPromise()
        .then(res => this.metaData = res as MetaData);
      this.http.get(this._baseUrl + this.options)
        .toPromise()
        .then(res => this.list = res as GameDto[]);
    }
  }
}
