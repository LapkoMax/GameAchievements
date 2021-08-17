import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Role } from './role';
import { UserDto } from './user-dto.model';
import { UserForRegistrationDto } from './user-for-registration-dto.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(public http: HttpClient) {
    this.http.get(this._baseUrl + "/allRoles")
      .toPromise()
      .then(res => this.roles = res as Role[]);
  }

  readonly _baseUrl = "https://localhost:7003/api/authenticate";
  formData: UserForRegistrationDto = new UserForRegistrationDto();
  list: UserDto[] = [];
  userIdForUpdate: string = "";
  errors: string[] = [];
  roles: Role[] = [];
  userName: string = "";
  rolesToAdd: string = "";

  postUser() {
    this.userName = this.formData.userName;
    return this.http.post(this._baseUrl, this.formData);
  }

  putUser() {
    return this.http.put(this._baseUrl + "/user/" + this.userIdForUpdate, this.formData);
  }

  deleteUser(id: string) {
    return this.http.delete(this._baseUrl + "/user/" + id);
  }

  updateRolesForUser() {
    return this.http.put(this._baseUrl + "/user/" + this.userName + "/roles/" + this.rolesToAdd.trim(), this.formData);
  }

  refreshList() {
    this.http.get(this._baseUrl + "/users")
      .toPromise()
      .then(res => this.list = res as UserDto[]);
  }
}
