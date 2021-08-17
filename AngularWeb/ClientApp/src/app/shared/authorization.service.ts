import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserForAuthenticationDto } from './user-for-authentication-dto.model';
import { UserForRegistrationDto } from './user-for-registration-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  constructor(public http: HttpClient) {
    this.user.userName = localStorage.getItem('userName') as string;
    this.user.password = localStorage.getItem('userPassword') as string;
    this.authenticate();
    setInterval(() => {
      this.authenticate()
    }, 360000);
  }

  readonly _baseUrl = "https://localhost:7003/api/authenticate";
  newUser: UserForRegistrationDto = new UserForRegistrationDto;
  user: UserForAuthenticationDto = new UserForAuthenticationDto;
  isAuth: boolean = localStorage.getItem('isAuth') as unknown as boolean;
  userName: string = localStorage.getItem('userName') as string;
  userRoles: string = localStorage.getItem('userRoles') as string;
  status: string = "200";
  errors: string[] = [];

  registerUser() {
    return this.http.post(this._baseUrl, this.newUser, { observe: 'response' })
      .toPromise()
      .then(res => {
        this.status = res.status.toString();
      },
      err => {
        this.status = err.status;
        if (err.error.errors) this.errors = JSON.stringify(err.error.errors).replace(/{|}|\[|\]|"/g, '').split(",");
        else this.errors = JSON.stringify(err.error).replace(/{|}|\[|\]|"/g, '').split(",");
      });
  }

  authenticate() {
    if (this.user.userName != "" && this.user.password != "") {
      return this.http.post(this._baseUrl + "/login", this.user, { observe: 'response' })
        .toPromise()
        .then(res => {
          this.status = res.status.toString();
          localStorage.setItem('accessToken', JSON.parse(JSON.stringify(res)).body.token);
          localStorage.setItem('isAuth', "true");
          localStorage.setItem('userName', this.user.userName);
          localStorage.setItem('userPassword', this.user.password);
          this.userName = this.user.userName;
          this.isAuth = true;
          this.http.get(this._baseUrl + "/roles?userName=" + this.user.userName).toPromise().then(res => {
            this.userRoles = (res as Array<string>).join(' ');
            localStorage.setItem('userRoles', this.userRoles);
          });
        },
          err => {
            this.status = err.status.toString();
            if (err.error.errors) this.errors = JSON.stringify(err.error.errors).replace(/{|}|\[|\]|"/g, '').split(",");
            else this.errors = JSON.stringify(err.error).replace(/{|}|\[|\]|"/g, '').split(",");
          });

    }
    else return null;
  }

  logout() {
    localStorage.setItem('accessToken', '');
    localStorage.setItem('isAuth', '');
    localStorage.setItem('userName', '');
    localStorage.setItem('userPassword', '');
    localStorage.setItem('userRoles', '');
    this.userName = "";
    this.isAuth = false;
    this.user = new UserForAuthenticationDto();
  }

}
