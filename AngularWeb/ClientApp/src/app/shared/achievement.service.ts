import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AchievementDto } from './achievement-dto.model';
import { MetaData } from './meta-data.model';

@Injectable({
  providedIn: 'root'
})
export class AchievementService {

  constructor(private http: HttpClient) { }

  readonly _baseUrl = "https://localhost:7003/api/games/";
  gameId: number = 0;
  formData: AchievementDto = new AchievementDto();
  fields: string = "name description condition";
  options: string = "";
  list: AchievementDto[] = [];
  metaData: MetaData = new MetaData;

  postAchievement() {
    return this.http.post(this._baseUrl + this.gameId + "/achievements", this.formData);
  }

  putAchievement() {
    return this.http.put(this._baseUrl + this.gameId + "/achievements/" + this.formData.id, this.formData);
  }

  deleteAchievement(id: number) {
    return this.http.delete(this._baseUrl + this.gameId + "/achievements/" + id);
  }

  refreshList() {
    this.http.get(this._baseUrl + this.gameId + "/achievements/metaData" + this.options)
      .toPromise()
      .then(res => this.metaData = res as MetaData);
    this.http.get(this._baseUrl + this.gameId + "/achievements" +  this.options)
      .toPromise()
      .then(res => this.list = res as AchievementDto[]);
  }
}
