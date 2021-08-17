export class UserDto {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  roles: string[];
  constructor() {
    this.id = "";
    this.firstName = "";
    this.lastName = "";
    this.userName = "";
    this.email = "";
    this.phoneNumber = "";
    this.roles = [];
  }
}
