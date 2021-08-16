export class UserForAuthenticationDto {
  userName: string;
  password: string;
  constructor() {
    this.userName = "";
    this.password = "";
  }
}
