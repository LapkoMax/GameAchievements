import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataSeedService {

  constructor(public http: HttpClient) { }

  _baseUrl = "https://localhost:7003/api/dataSeed";

  seedData(tableName: string, count: number, gameId: number = 0) {
    return this.http.get(this._baseUrl + "?tableName=" + tableName + "&count=" + count + "&gameId=" + gameId);
  }
}
