export class UserForRegistrationDto {
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
  email: string;
  phoneNumber: string;
  roles: string[];
  constructor() {
    this.firstName = "";
    this.lastName = "";
    this.userName = "";
    this.password = "";
    this.email = "";
    this.phoneNumber = "";
    this.roles = ["User"];
  }
}
