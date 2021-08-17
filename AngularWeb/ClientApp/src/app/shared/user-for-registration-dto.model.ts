import { UserDto } from "./user-dto.model";

export class UserForRegistrationDto extends UserDto{
  password: string;
  constructor() {
    super();
    this.firstName = "";
    this.lastName = "";
    this.userName = "";
    this.password = "";
    this.email = "";
    this.phoneNumber = "";
    this.roles = ["User"];
  }
}
