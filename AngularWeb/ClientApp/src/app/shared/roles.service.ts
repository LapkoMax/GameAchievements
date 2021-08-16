import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Role } from './role';

@Injectable({
  providedIn: 'root'
})
export class RolesService {

  constructor(public http: HttpClient) { }

  readonly _baseUrl = "https://localhost:7003/api/authenticate/allRoles";
  formData: Role = new Role();
  list: Role[] = [];
  errors: string[] = [];
  isCreate: boolean = true;

  postRole() {
    return this.http.post(this._baseUrl, this.formData);
  }

  putRole() {
    return this.http.put(this._baseUrl + "/" + this.formData.id, this.formData);
  }

  deleteUser(id: string) {
    return this.http.delete(this._baseUrl + "/" + id);
  }

  refreshList() {
    this.http.get(this._baseUrl)
      .toPromise()
      .then(res => this.list = res as Role[]);
  }
}
